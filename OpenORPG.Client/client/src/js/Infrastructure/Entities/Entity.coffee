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
      super game, x, y, "", 0
      @anchor.setTo 0, 0
      @body.collideWorldBounds = true

      @spriteText = new SpriteText(game, x, y)
      @spriteText.attachTo(@)
    
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
          texId = "entity_sprite_" + value
          @game.load.image(texId, DirectoryHelper.SPRITE_ENTITY_PATH + value + ".png")
          @loadTexture(texId, 0)

          # Add new animations into place
          @animations.add('move_right', [0, 1, 2, 3], 5, true, true)
          @animations.add('move_left', [5, 6, 7, 8], 5, true, true)
          @animations.add('move_up', [10, 11, 12, 13], 5, true, true)
          @animations.add('move_down', [15, 16, 17, 18], 5, true, true)

          @animations.play('move_down')          

    
    #
    #    Merges an entity with a given object. This operation is dangerous if you're not careful.
    #    This is only used when recieving an entity packet over the wire -- it will directly
    #    update every single property on the entity.
    #  
    mergeWith: (object) ->
      $.extend this, object # jQuery extend, merges two objects using a shallow merge
      return