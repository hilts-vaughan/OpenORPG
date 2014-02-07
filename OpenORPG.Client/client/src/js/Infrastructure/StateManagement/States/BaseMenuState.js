/*
        This is a state that shows the main menu of the game, it allows the player to work within a UI to reach the gameplay screen.
*/


module.exports = function() {

BaseMenuState = Class.extend({
  
  init: function(game){
        this.game = game;
  },

  create: function()
  {
        this.scrollBack = this.game.add.sprite(1024 / 2, 768 / 2, "scroll_bg", 0);
        this.scrollBack.anchor = new Phaser.Point(0.5, 0.5);
  },


    preload: function()
  {
        // Do some asset loading
        this.game.load.image("scroll_bg", "../assets/ui/scroll.png")
  }


});


}();