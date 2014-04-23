module.exports =

	# Floating text used to display above an entity temporarily
	class FloatingText extends Phaser.Text

		DAMAGE_COLOR: "##AA0114"
		HEAL_COLOR: "#28a428"

		constructor: (@game, @entity, @damage) ->
			
			@damage = -1

			@currentColor = @DAMAGE_COLOR
			if @damage < 0 then @currentColor = @HEAL_COLOR

			# Our style object, this can be changed accordingly
			@style = 
				font: "22px Georgia"
				fill:  @currentColor
				stroke: "#000000"
				strokeThickness: 3
				align: "center"

			# Call bse constructor
			super @game, @entity.x, @entity.y, @damage, @style

			# Anchor down
			@anchor.setTo 0.5, 0.5



			tween = @game.add.tween(@).to
				y: @entity.y - 32
				alpha: 0
			, 1000, Phaser.Easing.Linear.None, true

			func = =>
				@destroy()

			tween.onComplete.add(func, @)
			@game.add.existing(@)


		update: ->
			# Track our entity
			if @entity?
				@x = @entity.x + (@entity.texture.width) / 2
				@y = @entity.y				