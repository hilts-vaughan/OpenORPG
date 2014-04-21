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

			@movementToken = setInterval =>
				@_generateMovementTicket()
			, @MOVE_TICK_FREQ

		_generateMovementTicket: (overrride = false) =>
			if @entity == null
				return				
				
			if not @entity.body.velocity.isZero()
				console.log(overrride)
				@game.net.sendMovement( Math.floor(@entity.x), Math.floor(@entity.y), overrride, @entity.direction )			

		destroy: ->
			@entity = null

			# Clear any movement request pulses
			clearInterval(@movementToken)

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
				@entity.direction = 1
			else if @game.input.keyboard.isDown(Phaser.Keyboard.RIGHT)     
				@entity.body.velocity.setTo(120, 0)
				@entity.direction = 3
			else if @game.input.keyboard.isDown(Phaser.Keyboard.UP)     
				@entity.body.velocity.setTo(0, -120)
				@entity.direction = 2
			else if @game.input.keyboard.isDown(Phaser.Keyboard.DOWN)     
				@entity.body.velocity.setTo(0, 120)
				@entity.direction = 0
			else								
				if not @entity.body.velocity.isZero()
					@_generateMovementTicket(true)
				@entity.body.velocity.setTo(0, 0)


		# Used to handle when an entity has moved from their intial location
		_handleEntityMove: (packet) =>
		
			id = packet.id
			console.log(packet)
			position = packet.position
			entity = @game.entities[id]

			if entity == undefined
				return

			x = position.x
			y = position.y

			entity.direction = packet.direction


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






