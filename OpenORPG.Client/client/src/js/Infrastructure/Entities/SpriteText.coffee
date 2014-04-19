Entity = require('./Entity.coffee')

module.exports =

	# Text that is attached to a particular entity
	class SpriteText		

		PLAYER_COLOR: "#ffff00"
		STROKE_COLOR: "#000000"

		constructor: (@game, @x, @y) ->	
			#super game, x, y

			@currentColor = @PLAYER_COLOR

			# Our style object, this can be changed accordingly
			@style = 
				font: "12px Georgia"
				fill:  @currentColor
				stroke: @STROKE_COLOR
				strokeThickness: 3
				align: "center"

			# Create our phaser text here
			@text = @game.add.text(0, 0, "", @style)
			@text.anchor.setTo 0.5, 0.5
			console.log(@text)


		attachTo: (entity) ->
			@entity = entity			


		setText: (string) ->
			# Setup as needed
			@text.text = string

		update: ->
			# Track our entity
			if @entity?
				@text.x = @entity.x + (@entity.texture.width) / 2
				@text.y = @entity.y