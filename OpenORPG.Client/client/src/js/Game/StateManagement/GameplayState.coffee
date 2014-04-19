#
# The gameplay state is where the core gameplay of the game runs.
# This has everything required to run. operate, and update the game world.
#
# There should only ever be one of these states in existance.
#

PacketTypes = require('./../../Infrastructure/PacketTypes.coffee')
Player = require('../Entities/Player.coffee')
Entity = require('../../Infrastructure/Entities/Entity.coffee')
DirectoryHelper = require('../../Infrastructure/DirectoryHelper.coffee')
MovementSystem = require ('./../World/Systems/MovementSystem.coffee')
SpriteManager = require('./../Resources/SpriteManager.coffee')
Zone = require('./../World/Zone.coffee')

module.exports =
  class GameplayState

    constructor: (game) ->
      @game = game

      # The current zone we're operating on
      @currentZone = null


    create: ->

      # Let the game know we intend to use the arcade physics module from phaser
      @game.physics.startSystem(Phaser.Physics.ARCADE)

      # Prepare for a zone change
      @game.net.registerPacket PacketTypes.SMSG_ZONE_CHANGED, (packet) =>
        
        # If we're connected somewhere already, tear it down
        @currentZone?.clearZone()

        # Create our new zone with the packet map ID
        @currentZone = new Zone(@game, packet.zoneId)        

        # Traverse our entities to process
        for entity in packet.entities

          # Add our network entity
          oEntity = @currentZone.addNetworkEntityToZone(entity)

          # Setup our camera as required to follow if needed
          if entity.id is packet.heroId
            @currentZone.movementSystem.attachEntity(oEntity)
            @game.camera.follow oEntity            

    update: ->
      # Update the current zone if it is required
      @currentZone?.update()





    # Load all the assets that might be required at some point
    preload: ->
      
      # Load map assets
      @game.load.tilemap "map_1", "assets/Maps/1.json", null, Phaser.Tilemap.TILED_JSON
      @game.load.tilemap "map_2", "assets/Maps/2.json", null, Phaser.Tilemap.TILED_JSON
      @game.load.image "tilesheet", "assets/Maps/tilesheet_16.png"

      # Load sprite definitions
      SpriteManager.loadSpriteImages(@game)