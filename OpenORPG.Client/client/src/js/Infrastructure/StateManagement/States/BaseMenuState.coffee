module.exports =
  class BaseMenuState
    constructor: (game) ->
      @game = game
      return

    create: ->
      @scrollBack = @game.add.sprite(1024 / 2, 768 / 2, "scroll_bg", 0)
      @scrollBack.anchor = new Phaser.Point(0.5, 0.5)
      return

    preload: ->
      
      # Do some asset loading
      @game.load.image "scroll_bg", "../assets/ui/scroll.png"
      return