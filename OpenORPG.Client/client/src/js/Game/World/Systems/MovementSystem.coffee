PacketTypes = require('./../../../Infrastructure/PacketTypes.coffee')

module.exports = 

	# Controls the movement of entities on the screen and is responsible for managing
	# tweening and interpolating entities across the screen
	class MovementSystem

		# The frequency in which we choose to dispatch movement events to the server		
		MOVE_TICK_FREQ: 300

		constructor: (@game, @entity) ->
			@game.net.registerPacket PacketTypes.SMSG_ENTITY_MOVE, (packet) =>
				@_handleEntityMove(packet)

			
			# Build stuff we need
			#@game.time.events.loop(Phaser.Timer.SECOND, @_generateMovementTicket, @)

			setInterval =>
				@_generateMovementTicket()
			, @MOVE_TICK_FREQ

		_generateMovementTicket: =>
			if @entity.body.velocity.isZero() == false
				@game.net.sendMovement( Math.floor(@entity.x), Math.floor(@entity.y) )			

		update: ->
			if @game.input.keyboard.isDown(Phaser.Keyboard.LEFT)     
				@entity.body.velocity.setTo(-120, 0)
			if @game.input.keyboard.isDown(Phaser.Keyboard.RIGHT)     
				@entity.body.velocity.setTo(120, 0)
			if @game.input.keyboard.isDown(Phaser.Keyboard.UP)     
				@entity.body.velocity.setTo(0, -120)
			if @game.input.keyboard.isDown(Phaser.Keyboard.DOWN)     
				@entity.body.velocity.setTo(0, 120)
			if @game.input.keyboard.isDown(Phaser.Keyboard.SPACEBAR)								
				@entity.body.velocity.setTo(0, 0)

			# This is asking for trouble, probably
			# TODO: This will only work for movement
			for k,entity of @game.entities
				if entity.body.velocity.isZero()
					entity.animations.stop(null, true)

		# Used to handle when an entity has moved from their intial location
		_handleEntityMove: (packet) =>
			console.log("Loud and clear, folks!" )
			console.log(@)			
			id = packet.id
			console.log(id)
			position = packet.position
			entity = @game.entities[id]

			if entity == undefined
				return

			x = position.x
			y = position.y

			if entity.tween != undefined
				entity.tween.stop()

			tween = @game.add.tween(entity).to
				x: x
				y: y
			, @MOVE_TICK_FREQ + 100, Phaser.Easing.Linear.None, true

			entity.tween = tween






