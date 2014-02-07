/*
	You can select your hero here, for now it auto selects the first for you.
*/


module.exports = function() {

	HeroSelectState = Class.extend({

		init: function(game) {
			this.game = game;
		},

		create: function() {

			var selectPacket = {
				opCode: PacketTypes.CMSG_HERO_SELECT,
				heroId: 1
			}

			var self = this;
			self.game.state.add("game", new GameplayState(self.game));

			// Do this when the user is ready
			self.game.state.start("game");

			// Be ready to catch the response
			this.game.net.registerPacket(PacketTypes.SMSG_HERO_SELECT_RESPONSE,
				function(packet) {

				});

			this.game.net.sendPacket(selectPacket);

		},

		preload: function() {

		}



	});


}();