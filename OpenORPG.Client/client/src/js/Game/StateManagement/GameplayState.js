/*
	The gameplay state is where the core gameplay of the game runs.
	This has everything required to run. operate, and update the game world.

	There should only ever be one of these states in existance.
*/


module.exports = function() {

  GameplayState = Class.extend({

    init: function(game) {
      this.game = game;
      var self = this;



    },

    create: function() {




      var self = this;
      self.map = self.game.add.tilemap('map_1');
      self.map.addTilesetImage('tilesheet')

      $.each(self.map.layers, function(key, value) {
        var layer = self.map.createLayer(value.name);
        layer.resizeWorld();
      });

      this.game.net.registerPacket(PacketTypes.SMSG_ZONE_CHANGED, function(packet) {

      });

      var sprite = this.game.add.sprite(72 * 32, 150 * 32, "player_active", 0);
      this.game.camera.follow(sprite);
      

    },


    preload: function() {
      this.game.load.tilemap('map_1', 'assets/Maps/1.json', null, Phaser.Tilemap.TILED_JSON);
      this.game.load.image('tilesheet', 'assets/Maps/tilesheet_16.png');
    }


  });

}();