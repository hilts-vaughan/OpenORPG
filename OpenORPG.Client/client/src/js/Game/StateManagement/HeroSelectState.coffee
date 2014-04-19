#
#	You can select your hero here, for now it auto selects the first for you.
#

BaseMenuState = require('../../Infrastructure/StateManagement/States/BaseMenuState.coffee')
GameplayState = require('./GameplayState.coffee')
PacketTypes = require('./../../Infrastructure/PacketTypes.coffee')
SpriteManager = require('./../Resources/SpriteManager.coffee')

module.exports =
  class HeroSelectState extends BaseMenuState
    constructor: (game) ->
      @game = game
      return

    create: ->
      selectPacket =
        opCode: PacketTypes.CMSG_HERO_SELECT
        heroId: 1

      
      # Be ready to catch the response
      @game.net.registerPacket PacketTypes.SMSG_HERO_SELECT_RESPONSE, (packet) =>
        @game.state.add "game", new GameplayState(@game)      
        
        # Do this when the user is ready
        @game.state.start "game"        

      @game.net.sendPacket selectPacket
      return

    preload: ->

      # We're here, so we load some of our sprite JSON here to be sneaky
      SpriteManager.loadSprites(@game)