
MovementSystem = require ('./Systems/MovementSystem.coffee')
CombatSystem = require('./../Combat/CombatSystem.coffee')

Entity = require('../../Infrastructure/Entities/Entity.coffee')
PacketTypes = require('./../../Infrastructure/PacketTypes.coffee')
FloatingText = require('./../Entities/FloatingText.coffee')

module.exports = 
	
    #     A zone represents a single given subspace in the world that can be contained.
    #     This is similar to a 'map' or 'area' in traditional MMO worlds.
    #     The world within the zone contains the actual physical mainfestation and collection of objects.

	class Zone

		constructor: (game, mapId) ->

			# Store our game instance and keep things handy
      @game = game
      @game.entities = []

			# A list of entities to add/remove from the zone
      @toAdd = []
      @toAdd = []

			# A list of systems to maintain
      @systems = []

			# Create our movement system, push it
      @movementSystem = new MovementSystem(@game, null)
      @systems.push(@movementSystem)


      combatSystem = new CombatSystem(@game)
      @systems.push(combatSystem)
			# Setup the tilemap
			self = @
			@map = self.game.add.tilemap("map_" + mapId)
			@map.addTilesetImage "tilesheet"


			# Init our tilemap logic here
			$.each self.map.layers, (key, value) ->
				layer = self.map.createLayer(value.name)
				layer.resizeWorld()

			# Add the entity into the zone
			@game.net.registerPacket PacketTypes.SMSG_MOB_CREATE, (packet) =>
				@addNetworkEntityToZone(packet.mobile)
			
			# Handle mobile destructions
			@game.net.registerPacket PacketTypes.SMSG_MOB_DESTROY, (packet) =>
				@toRemove.push(packet.id)        

			# Sync entities
			@game.net.registerPacket PacketTypes.SMSG_ENTITY_PROPERTY_CHANGE, (packet) =>
        
				# Sync our entity collection here
				entity = @game.entities[packet.entityId]
				entity.mergeWith(packet.properties)

				# Activate new properties
				for k,v of packet.properties
					entity.propertyChanged(k, v)



		# Add a network entity that has been serialize to the current game world
		addNetworkEntityToZone: (networkEntity) ->
			entity = networkEntity
			oEntity = new Entity(@game, 0, 0)
			oEntity.mergeWith(entity)
			@game.entities[oEntity.id] = oEntity
			@game.add.existing(oEntity)

			# Trigger property updates
			for k,v of entity
				oEntity.propertyChanged(k, v)
			return oEntity


		# This method is called when tearing down the world
		# it performs tear-down and prepare a new clear slate
		# for an incoming client to use
		clearZone: ->
			
			# Kill off entity stuff
			for k,entity of @game.entities
				console.log(entity)
				entity.spriteText?.destroy()
				entity.destroy()
			@game.entities = []

			# Remove the tile map
			@map.destroy()

			# Kill all systems that need to be
			for system in @systems
				system.destroy?()




		update: ->

			# Send a request to go north on space bar
			if @game.input.keyboard.justReleased(Phaser.Keyboard.SPACEBAR, 100)
				@game.net.sendZoneChangeRequest( 2 )
			if @game.input.keyboard.justReleased(Phaser.Keyboard.A, 100)
				@game.net.sendZoneChangeRequest( 0 )

			if @game.input.keyboard.justReleased(Phaser.Keyboard.Q, 100)
				@game.net.sendSkillUse(1, 1)


			# Remove and delete entities as needed
			for remove in @toRemove
				@game.entities[remove].spriteText?.destroy()
				@game.entities[remove].destroy()
				delete @game.entities[remove]
				@toRemove = []

			# Update systems
			for system in @systems
				system.update()			