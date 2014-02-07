module.exports = function() {

  var Monster = Character.extend({

    init: function(game, x, y, key, frame) {

      // Super chain the call down to the entity
      this._super(game, x, y, key, frame);
    },

  });


}();