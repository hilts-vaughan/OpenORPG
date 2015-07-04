module OpenORPG {
    export class NetworkManager {
        /* Networking */
        private static _hostname: string = null;

        static get hostname(): string {
            if (this._hostname == null) {
                return window.location.hostname;
            }

            return this._hostname;
        }

        static set hostname(val: string) {
            this._hostname = val;
        }

        private static _instance: NetworkManager = null;

        private _host: string;
        private _port: number = 0;
        private _socket: WebSocket = null;

        // Connection callbacks for usage
        public onConnectionCallback: () => void;
        public onRecieveMessageCallback: () => void;
        public onConnectionErrorCallback: () => void;

        // Our internal packet callbacks
        private _packetCallbacks: any = [];

        constructor(host: string, port: number) {
            if (NetworkManager._instance) {
                throw new Error("Error: Instantiation failed: Use SingletonDemo.getInstance() instead of new.");
            }
            NetworkManager._instance = this;

            this._host = host;
            this._port = port;
        }

        public static getInstance(): NetworkManager {
            if (NetworkManager._instance === null) {
                NetworkManager._instance = new NetworkManager(this.hostname, 4488);
            }
            return NetworkManager._instance;
        }

        public sendPacket(packet: any) {
            var json = JSON.stringify(packet);
            this._socket.send(json);
        }

        public registerPacket(opCode: OpCode, callback: (packet: any) => void) {

            if (!this._packetCallbacks[opCode])
                this._packetCallbacks[opCode] = [];

            this._packetCallbacks[opCode].push(callback);
        }

        public connect() {
            // Create our socket
            var url = "ws://" + this._host + ":" + this._port + "/";
            this._socket = new WebSocket(url);

            this._socket.onopen = (event: Event) => {
                this.onConnectionCallback();
            };

            this._socket.onerror = (event: Event) => {
                this.onConnectionErrorCallback();
            };

            this._socket.onmessage = (event: any) => {
                this.parseMessage(event);
            };
        }

        // Parses an incoming message accordingly
        private parseMessage(response: MessageEvent) {
            var packet: any = JSON.parse(response.data);
            if (this._packetCallbacks[packet.opCode] != undefined) {
                _.forEach(this._packetCallbacks[packet.opCode], (value: Function) => {
                    value(packet);
                });
            } else
                Logger.warn("A packet with the ID of " + packet.opCode + " received but not handled");       
        }
    }
}