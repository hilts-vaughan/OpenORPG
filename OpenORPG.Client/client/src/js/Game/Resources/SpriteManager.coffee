DirectoryHelper = require('../../Infrastructure/DirectoryHelper.coffee')

module.exports =

	class SpriteManager

		# Loads all sprite data into the browser at once
		# This isn't usually ideal but Phaser will not
		# let us load once the game is over, so we try
		# and get this out of the way now
		@loadSpriteInfo: (game) ->
			spritePack = game.load.json("spritepack", DirectoryHelper.SPRITE_ENTITY_PATH + "sprites.json")			
		@loadSprites: (game) ->
			
			# Iterate all the key pairs
			for k,v of game.cache.getJSON("spritepack")
				spriteDef  = game.load.json(DirectoryHelper.SPRITE_ENTITY_PATH + v + ".json") 			
				game.load.spritesheet(k, DirectoryHelper.SPRITE_ENTITY_PATH + v + ".png", spriteDef.width, spriteDef.height)



