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
      @smoothed = false
      @movementQueue = []


      @alpha = 0
      @x = x
      @y = y

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
      @spriteText?.update()

      # Play the graphic for the right state
      if @skillUseAnim is undefined
        switch @characterState
          when "Idle"
            @_playIdle()
          when "UsingSkill"
            @_playAttack()
          when "Moving"
            @_playWalk()
      else
        if @skillUseAnim.isFinished
          @skillUseAnim = undefined



    mapDirectionTo: ->
      switch @direction
        when 0
          return "down"
        when 1
          return "left"
        when 2
          return "up"
        when 3
          return "right"

    _playIdle: ->
      dir = @mapDirectionTo()
      @animations.play("idle_" + dir)

    _playAttack: ->
      dir = @mapDirectionTo()
      @animations.play("atk_" + dir)

    _playWalk: ->
      dir = @mapDirectionTo()
      @animations.play("walk_" + dir)

    # Play your animation
    playSkillAnimation: ->
      dir = @mapDirectionTo()
      @skillUseAnim = @animations.play("atk_" + dir, 12, false)

    # This method is invoked when a property has changed
    propertyChanged: (name, value) ->
      
      # Do a switch on the name and update accordingly
      switch name

        when "name"
          @spriteText?.destroy()
          @spriteText = new SpriteText(@game, @x, @y)
          @spriteText.attachTo(@)
          @spriteText.setText(value)

        when "sprite"

          # We should load the JSON from the remote server

          texId = "entity_sprite_" + value
          spriteImage = @game.cache.getImage(texId)

          @loadTexture(texId, 0)


          # We can use our sprite definition to find out what we need to for sure
          spriteDef = @game.cache.getJSON("spritedef_" + texId)
          rowFrames = spriteImage.width / spriteDef.width

          for k,v of spriteDef.animations

            startingIndex = rowFrames * v.row            
            frames = []
            for i in [0..v.length - 1]
              frames.push(startingIndex + i)
            @animations.add(k, frames, 4, true, true)

          @_playIdle()
    
    #
    #    Merges an entity with a given object. This operation is dangerous if you're not careful.
    #    This is only used when recieving an entity packet over the wire -- it will directly
    #    update every single property on the entity.
    #  
    mergeWith: (object) ->
      $.extend this, object # jQuery extend, merges two objects using a shallow merge
      return