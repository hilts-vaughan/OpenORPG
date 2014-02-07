module.exports = function() {

  var Player = Character.extend({

    init: function(game, x, y, key, frame) {

      // Super chain the call down to the entity
      this._super(game, x, y, key, frame);
    },


  });


}();