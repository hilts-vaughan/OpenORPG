/*
        This is a state that shows the main menu of the game, it allows the player to work within a UI to reach the gameplay screen.
*/


module.exports = function() {

BootstrapState = Class.extend({

  init: function(game) {
    this.game = game;
  },

	create: function() {


		// Display a quick image to let the user know what's going on
		this.connectSplash = this.game.add.sprite(1024 / 2, 786 / 2, "connecting", 0);
		this.connectSplash.anchor.setTo(0.5, 0.5);

		// We'll do any initalizing we need here, we can try connecting the game client here
		// We'll wait until there's a succeess before display the main menu, otherwise
		// we'll display the error state.
		var port = 1234;
		var host = "localhost"
		var self = this;
		this.game.net = new GameClient(this.game, host, port)
		this.game.net.onConnection(function() {
			console.log("Connection was a success.");
			self.game.state.start('mainmenu');
		});

		this.game.net.onConnectionError(function() {
			console.log("Connection was not a success.");
			self.game.state.start('errorstate');
		});

		// Make our actual network call
		this.game.net.connect();		

	},

	preload: function() {
		this.game.load.image("connecting", "../assets/ui/connecting.png");
	}



});


}();
