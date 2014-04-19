DirectoryHelper = require('../../Infrastructure/DirectoryHelper.coffee')

module.exports =

	class SpriteManager

		# Loads all sprite data into the browser at once
		# This isn't usually ideal but Phaser will not
		# let us load once the game is over, so we try
		# and get this out of the way now
		@loadSpriteInfo: (game) ->
			game.load.json("spritepack", DirectoryHelper.SPRITE_ENTITY_PATH + "sprites.json")			
		@loadSprites: (game) ->
			
			# Iterate all the key pairs
			for k,v of game.cache.getJSON("spritepack")
				game.load.json("spritedef_" + k, DirectoryHelper.SPRITE_ENTITY_PATH + v + ".json")

		@loadSpriteImages: (game) ->
			json = game.cache.getJSON("spritepack")
			console.log(json)
			for k,v of json
				spriteDef = game.cache.getJSON("spritedef_" + k)
				game.load.spritesheet(k, DirectoryHelper.SPRITE_ENTITY_PATH + v + ".png", spriteDef.width, spriteDef.height)