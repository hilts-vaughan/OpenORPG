/*
        This is a state that shows the main menu of the game, it allows the player to work within a UI to reach the gameplay screen.
*/


module.exports = function() {

ErrorMenuState = Class.extend({

  init: function(game) {
    this.game = game;
  },

  create: function() {
    this.scrollBack = this.game.add.sprite(1024 / 2, 768 / 2, "scroll_bg", 0);
    this.scrollBack.anchor = new Phaser.Point(0.5, 0.5);


    // Add our error message letting the user know what happened
    var errorMessage = "The game failed to connect to the server. Please reload.";
    this.game.add.text(1024 / 2, 768 / 2, errorMessage).anchor.setTo(0.5, 0.5);

  },


  preload: function() {
    // Do some asset loading
    this.game.load.image("scroll_bg", "../assets/ui/scroll.png")

    var style = {
      font: "bold 40pt courier",
      fill: "#ffffff",
      align: "center",
      stroke: "#ffffff",
      strokeThickness: 8
    };



  }


});


}();