#
#    The game client is used to network with the server and serves as a controller to other 
#    game related tasks. We use this to pump out and send messages.
#

PacketTypes = require("../PacketTypes.coffee") 

module.exports =
  class GameClient
    
    #
    #      Intializes the game client to connect
    #  
    constructor: (game, host, port) ->
      @connection = null #websocket currently attached to the server
      @host = host # a string which is the host of the server
      @port = port # a port to the server
      @handlers = [] # a set of handlers that are used for recieving network packets
      @onConnectionCallback = null
      @onConnectionErrorCallback = null
      @serializeMethod = "JSON"
      return

    
    #
    #    Connects the game client to the server specified by the game client
    #  
    connect: (dispatcherMode) ->
      url = "ws://" + @host + ":" + @port + "/"
      self = this
      
      #log.info("Trying to connect to server : " + url);
      if window.MozWebSocket
        @connection = new MozWebSocket(url)
      else
        @connection = new WebSocket(url)
      @connection.onopen = (e) ->
        
        # Notify the person who cares
        self.onConnectionCallback()
        return

      @connection.onmessage = (packet) ->
        self.recieveMessage packet
        return

      @connection.onerror = (e) ->
        self.onConnectionErrorCallback()
        return

      @connection.onclose = ->

      return

    
    # do something when the user has disconnected
    onConnection: (callback) ->
      @onConnectionCallback = callback
      return

    onConnectionError: (callback) ->
      @onConnectionErrorCallback = callback
      return

    
    # 
    #    We use this function to parse and send data back to those who request it.
    #    There is no need to parse data out, the system is responsbile for this.
    #  
    recieveMessage: (response) ->
      packet = JSON.parse(response.data)
      opCode = packet.opCode
      @handlers[opCode] packet  if @handlers[opCode] # Send the data to the person who has requested this
      #console.log packet
      return

    sendPacket: (packet) ->
      
      # Prepare the data to be sent
      packetString = JSON.stringify(packet)
      
      # Send a message through socket
      @connection.send packetString  if @connection
      return

    
    #
    #    Indiciates to the game client network operation that a system is requesting updates to a particular packet type
    #    We store the callback it has requested inside the handlers
    #  
    registerPacket: (packetType, callback) ->
      @handlers[packetType] = callback
      return

    
    sendMovement: (x, y) ->
      packet =
        opCode: PacketTypes.CMSG_MOVEMENT_REQUEST
        currentPosition:
          x: x
          y: y

      @sendPacket packet


    # Sends a zone change request with the given direction
    sendZoneChangeRequest: (direction) ->
      packet = 
        opCode: PacketTypes.CMSG_ZONE_CHANGE
        direction: direction

      @sendPacket packet

    # Packets being sent are stored below, here
    sendLogin: (username, password) ->
      opCode = PacketTypes.CMSG_LOGIN_REQUEST
      
      # This is the format of our login packet that is expected on the server
      packet =
        opCode: opCode
        username: username
        password: password

      @sendPacket packet
      return