#
#        This is a state that shows the main menu of the game, it allows the player to work within a UI to reach the gameplay screen.
#
BaseMenuState = require('./BaseMenuState.coffee')
module.exports =
  class ErrorMenuState extends BaseMenuState
    constructor: (game) ->
      @game = game
      super
      return

    create: ->
      super
      @scrollBack = @game.add.sprite(1024 / 2, 768 / 2, "scroll_bg", 0)
      @scrollBack.anchor = new Phaser.Point(0.5, 0.5)
      return

    preload: ->
      super
      # Do some asset loading
      @game.load.image "scroll_bg", "../assets/ui/scroll.png"
      return