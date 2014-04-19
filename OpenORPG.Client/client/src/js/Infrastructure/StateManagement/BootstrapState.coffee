#
#        This is a state that shows the main menu of the game, 
#        It allows the player to work within a UI to reach the gameplay screen.
#

GameClient = require('../Network/GameClient.coffee')
SpriteManager = require('./../../Game/Resources/SpriteManager.coffee')

module.exports =
  class BootstrapState

    constructor: (game) ->
      @game = game
      return

    create: ->
      
      @game.stage.disableVisibilityChange = true;

      # Display a quick image to let the user know what's going on
      @connectSplash = @game.add.sprite(1024 / 2, 786 / 2, "connecting", 0)
      @connectSplash.anchor.setTo 0.5, 0.5
      
      # We'll do any initalizing we need here, we can try connecting the game client here
      # We'll wait until there's a succeess before display the main menu, otherwise
      # we'll display the error state.
      port = 1234
      host = "localhost"
      self = this
      @game.net = new GameClient(@game, host, port)
      console.log(@game.net)
      @game.net.onConnection ->
        console.log "Connection was a success."
        self.game.state.start "mainmenu"

      @game.net.onConnectionError ->
        console.log "Connection was not a success."
        self.game.state.start "errorstate"

      
      # Make our actual network call
      @game.net.connect()

    preload: ->
      SpriteManager.loadSpriteInfo(@game)
      @game.load.image "connecting", "../assets/ui/connecting.png"