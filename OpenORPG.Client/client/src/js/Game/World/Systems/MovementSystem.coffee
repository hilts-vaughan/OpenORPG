PacketTypes = require('./../../../Infrastructure/PacketTypes.coffee')

module.exports = 

	# Controls the movement of entities on the screen and is responsible for managing
	# tweening and interpolating entities across the screen
	class MovementSystem

		# The frequency in which we choose to dispatch movement events to the server		
		MOVE_TICK_FREQ: 250

		constructor: (@game, @entity) ->
			@game.net.registerPacket PacketTypes.SMSG_ENTITY_MOVE, (packet) =>
				@_handleEntityMove(packet)

			
			# Build stuff we need
			#@game.time.events.loop(Phaser.Timer.SECOND, @_generateMovementTicket, @)

			setInterval =>
				@_generateMovementTicket()
			, @MOVE_TICK_FREQ

		_generateMovementTicket: (overrride) =>
			if @entity == null
				return
				
			if @entity.body.velocity.isZero() == false or overrride
				@game.net.sendMovement( Math.floor(@entity.x), Math.floor(@entity.y) )			


		# Attaches an entity
		attachEntity: (entity) ->
			@entity = entity

		# Update our system
		update: ->

			# Don't update if we have nothing to do right now
			if not @entity
				return

			if @game.input.keyboard.isDown(Phaser.Keyboard.LEFT)     
				@entity.body.velocity.setTo(-120, 0)
				@entity.animations.play("walk_right")
				@entity.isMoving = true
			else if @game.input.keyboard.isDown(Phaser.Keyboard.RIGHT)     
				@entity.body.velocity.setTo(120, 0)
				@entity.animations.play("walk_right")
				@entity.isMoving = true
			else if @game.input.keyboard.isDown(Phaser.Keyboard.UP)     
				@entity.body.velocity.setTo(0, -120)
				@entity.animations.play("walk_up")
				@entity.isMoving = true
			else if @game.input.keyboard.isDown(Phaser.Keyboard.DOWN)     
				@entity.body.velocity.setTo(0, 120)
				@entity.animations.play("walk_down")
				@entity.isMoving = true
			else								
				if not @entity.body.velocity.isZero()
					@_generateMovementTicket(true)
				@entity.body.velocity.setTo(0, 0)
				@entity.isMoving = false


			# This is asking for trouble, probably
			# TODO: This will only work for movement
			for k,entity of @game.entities
				okay = entity.animations.currentAnim.name.indexOf("walk")
				if not entity.isMoving and okay > -1
					entity.animations.stop(null, true)


		# Used to handle when an entity has moved from their intial location
		_handleEntityMove: (packet) =>
			console.log("Loud and clear, folks!" )
			console.log(@)			
			id = packet.id
			console.log(packet)
			position = packet.position
			entity = @game.entities[id]

			if entity == undefined
				return

			x = position.x
			y = position.y


			destX = Math.floor(entity.x - x)
			destY = Math.floor(entity.y - y)

			console.log(destX)
			console.log(destY)

			if destX > 3
				entity.animations.play("walk_right")
			else if destX < -3
				entity.animations.play("walk_right")

			if destY > 3
				entity.animations.play("walk_up")
			else if destY < -3
				entity.animations.play("walk_down")

			if entity.tween != undefined
				entity.tween.stop()

			entity.isMoving = true
			tween = @game.add.tween(entity).to
				x: x
				y: y
			, @MOVE_TICK_FREQ + 100, Phaser.Easing.Linear.None, true



			func = =>
				entity.isMoving = false

			tween.onComplete.add(func, @)

			entity.tween = tween






