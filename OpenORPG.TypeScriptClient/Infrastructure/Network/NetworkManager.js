var OpenORPG;
(function (OpenORPG) {
    var NetworkManager = (function () {
        function NetworkManager(host, port) {
            this._port = 0;
            this._socket = null;
            if (NetworkManager._instance) {
                throw new Error("Error: Instantiation failed: Use SingletonDemo.getInstance() instead of new.");
            }
            NetworkManager._instance = this;

            this._host = host;
            this._port = port;
        }
        NetworkManager.getInstance = function () {
            if (NetworkManager._instance === null) {
                NetworkManager._instance = new NetworkManager("localhost", 1234);
            }
            return NetworkManager._instance;
        };

        NetworkManager.prototype.sendPacket = function (packet) {
            var json = JSON.stringify(packet);
            this._socket.send(json);
        };

        NetworkManager.prototype.registerPacket = function (opCode, callback) {
            this._packetCallbacks[opCode] = callback;
        };

        NetworkManager.prototype.connect = function () {
            var _this = this;
            // Create our socket
            var url = "ws://" + this._host + ":" + this._port + "/";
            this._socket = new WebSocket(url);

            this._socket.onopen = function (event) {
                _this.onConnectionCallback();
            };

            this._socket.onerror = function (event) {
                _this.onConnectionErrorCallback();
            };

            this._socket.onmessage = function (event) {
                _this.parseMessage(event);
            };
        };

        // Parses an incoming message accordingly
        NetworkManager.prototype.parseMessage = function (response) {
            var packet = JSON.parse(response.data);
            this._packetCallbacks[packet.opCode](packet);
        };
        NetworkManager._instance = null;
        return NetworkManager;
    })();
    OpenORPG.NetworkManager = NetworkManager;
})(OpenORPG || (OpenORPG = {}));
//# sourceMappingURL=NetworkManager.js.map
