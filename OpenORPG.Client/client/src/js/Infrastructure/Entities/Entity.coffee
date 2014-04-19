DirectoryHelper = require('../DirectoryHelper.coffee')
SpriteText = require('./SpriteText.coffee')

#
#  An entity lives, breathes and exists within a world. They are the core of the online
#  world and everything within our interactive world is one of these.
#
#  An entity inherits from a Phaser sprite
#
#
module.exports =
  class Entity extends Phaser.Sprite

    constructor: (game, x, y, key, frame) ->
      @game = game
      super game, x, y, "male_base", 0
      @anchor.setTo 0, 0
      @game.physics.enable(@, Phaser.Physics.ARCADE)
      @body.collideWorldBounds = true

      @spriteText = new SpriteText(game, x, y)
      @spriteText.attachTo(@)
      @alpha = 0

      # Add a nice fading effect so that enities don't 'pop' into the world
      tween = @game.add.tween(@).to
        alpha: 1
      , 700, Phaser.Easing.Quadratic.In, true      
      

    
    #
    #    Obtains the distance between this current entity context and the one specificed in the
    #    parameter.  Useful for checking if something is in range or not.
    #  
    distanceFrom: (entity) ->
      distX = Math.abs(entity.gridX - @gridX)
      distY = Math.abs(entity.gridY - @gridY)
      (if (distX > distY) then distX else distY)


    update: ->
      @spriteText.update()


    # This method is invoked when a property has changed
    propertyChanged: (name, value) ->
      
      # Do a switch on the name and update accordingly
      switch name

        when "name"
          @spriteText.setText(value)

        when "sprite"

          # We should load the JSON from the remote server

          texId = "entity_sprite_" + value
          @game.load.image(texId, DirectoryHelper.SPRITE_ENTITY_PATH + value + ".png")

          @loadTexture(texId, 0)


          @animations.add('move_right', [0, 1, 2], 5, true, true)
          @animations.add('move_left', [5, 6, 7, 8], 5, true, true)
          @animations.add('move_up', [10, 11, 12, 13], 5, true, true)
          @animations.add('move_down', [35, 36, 37, 38], 5, true, true)

          @animations.play('move_right')          

    
    #
    #    Merges an entity with a given object. This operation is dangerous if you're not careful.
    #    This is only used when recieving an entity packet over the wire -- it will directly
    #    update every single property on the entity.
    #  
    mergeWith: (object) ->
      $.extend this, object # jQuery extend, merges two objects using a shallow merge
      return