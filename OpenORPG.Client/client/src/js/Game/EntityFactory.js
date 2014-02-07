/*
  An entity lives, breathes and exists within a world. They are the core of the online
  world and everything within our interactive world is one of these.

  An entity inherits from a Phaser

*/


module.exports = function() {

EntityFactory = Class.extend({

  init: function(game) {
    this.game = game;
    this.id = null; // This is the network ID of the object on the server
  },


  /*
    Obtains the distance between this current entity context and the one specificed in the
    parameter.  Useful for checking if something is in range or not.
  */
  distanceFrom: function(entity) {
    var distX = Math.abs(entity.gridX - this.gridX),
      distY = Math.abs(entity.gridY - this.gridY);

    return (distX > distY) ? distX : distY;
  },


  /*
    Merges an entity with a given object. This operation is dangerous if you're not careful.
    This is only used when recieving an entity packet over the wire -- it will directly
    update every single property on the entity.
  */
  mergeWith: function(object) {
    $.extend(this, object); // jQuery extend, merges two objects using a shallow merge
  }



});

}();