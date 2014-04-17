Entity = require('../../Infrastructure/Entities/Entity.coffee')

module.exports = 
	class Player extends Entity

		constructor: (game, x, y) ->
			super game, x, y

