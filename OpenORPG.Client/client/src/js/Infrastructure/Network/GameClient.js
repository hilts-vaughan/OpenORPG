/*
    The game client is used to network with the server and serves as a controller to other 
    game related tasks. We use this to pump out and send messages.
*/


module.exports = function() {

GameClient = Class.extend({

  /*
      Intializes the game client to connect
  */
  init: function(game, host, port) {

    this.connection = null; //websocket currently attached to the server
    this.host = host; // a string which is the host of the server
    this.port = port; // a port to the server

    this.handlers = [] // a set of handlers that are used for recieving network packets
    this.onConnectionCallback = null;
    this.onConnectionErrorCallback = null;
    this.serializeMethod = "JSON";


  },


  /*
    Connects the game client to the server specified by the game client
  */
  connect: function(dispatcherMode) {
    var url = "ws://" + this.host + ":" + this.port + "/",
      self = this;

    //log.info("Trying to connect to server : " + url);

    if (window.MozWebSocket) {
      this.connection = new MozWebSocket(url);
    } else {
      this.connection = new WebSocket(url);
    }
    this.connection.onopen = function(e) {
      // Notify the person who cares
      self.onConnectionCallback();
    };

    this.connection.onmessage = function(packet) {
      self.recieveMessage(packet);
    };

    this.connection.onerror = function(e) {
      self.onConnectionErrorCallback();
    };

    this.connection.onclose = function() {
      // do something when the user has disconnected
    };
  },


  onConnection: function(callback) {
    this.onConnectionCallback = callback;
  },


  onConnectionError: function(callback) {
    this.onConnectionErrorCallback = callback;
  },

  /* 
    We use this function to parse and send data back to those who request it.
    There is no need to parse data out, the system is responsbile for this.
  */
  recieveMessage: function(response) {

    var packet = JSON.parse(response.data);
    var opCode = packet.opCode;

    if (this.handlers[opCode])
      this.handlers[opCode](packet); // Send the data to the person who has requested this

    console.log(packet);
  },


  sendPacket: function(packet) {

    // Prepare the data to be sent
    var packetString = JSON.stringify(packet);

    // Send a message through socket
    if (this.connection)
      this.connection.send(packetString);
  },


  /*
    Indiciates to the game client network operation that a system is requesting updates to a particular packet type
    We store the callback it has requested inside the handlers
  */
  registerPacket: function(packetType, callback) {

    this.handlers[packetType] = callback;
  },


  // Packets being sent are stored below, here
  sendLogin: function(username, password) {

    var opCode = PacketTypes.CMSG_LOGIN_REQUEST;
    // This is the format of our login packet that is expected on the server
    var packet = {
      "opCode": opCode,
      "username": username,
      "password": password
    }

    this.sendPacket(packet);
  }




});


}();