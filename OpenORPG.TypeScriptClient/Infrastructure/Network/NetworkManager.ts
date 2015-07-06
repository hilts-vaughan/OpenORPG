module OpenORPG {
    export class NetworkManager {
        /* Networking */
        private static _instance: NetworkManager = null;

        public static getInstance(): NetworkManager {
            if (NetworkManager._instance === null) {
                NetworkManager._instance = new NetworkManager(this.hostname, 4488);
            }

            return NetworkManager._instance;
        }

        private static _hostname: string = null;

        static get hostname(): string {
            if (this._hostname == null) {
                this.hostname = window.location.hostname;
            }

            return this._hostname;
        }

        static set hostname(val: string) {
            this._hostname = val;
        }

        private _host: string;
        private _port: number = 0;
        private _socket: WebSocket = null;
        private _reconnectTime: number = 5000;

        /* Event callbacks map */
        private _on: { [event: string]: { (event?: any): void; }[] } = {};

        /* Packet callbacks */
        private _packetCallbacks: any = [];

        constructor(host: string, port: number) {
            if (NetworkManager._instance) {
                throw new Error("Error: Instantiation failed: Use SingletonDemo.getInstance() instead of new.");
            }

            NetworkManager._instance = this;

            this._host = host;
            this._port = port;
            this._socket = null;

            this.onConnect.push((event?: any) => {
                console.log(event);
            });

            this.onDisconnect.push((event?: any) => {
                if (this._reconnectTime >= 0) {
                    setTimeout(() => {
                        console.log(event);
                        NetworkManager._instance.reconnect();
                    }, this._reconnectTime);
                }
            });

            this.onError.push((event?: any) => {
                console.log(event);
            });

            this.onMessage.push((event?: any) => {
                this.parseMessage(event);
            });
        }

        public get on(): { [event: string]: { (event?: any): void; }[] } {
            return this._on;
        }

        public callbacks(event: string) {
            return this._on[event] = (this._on[event] || []);
        }

        public get onConnect(): { (event?: any): void; }[] {
            return this.callbacks('connect');
        }

        public get onDisconnect(): { (event?: any): void; }[] {
            return this.callbacks('disconnect');
        }

        public get onMessage(): { (event?: any): void; }[] {
            return this.callbacks('message');
        }

        public get onError(): { (event?: any): void; }[] {
            return this.callbacks('error');
        }

        protected fire(eventName: string, event?: any): void {
            var callbacks = this.on[eventName];
            for (var i in callbacks) {
                var callback = callbacks[i];
                callback(event);
            }
        }

        public connect() {
            /* Create our socket */
            var url = "ws://" + this._host + ":" + this._port + "/";
            this._socket = new WebSocket(url);

            /* Bind events */
            this._socket.onopen = (event: Event) => {
                this.fire('connect', event);
            };

            this._socket.onerror = (event: Event) => {
                this.fire('error', event);
            };

            this._socket.onmessage = (event: any) => {
                this.fire('message', event);
            };

            this._socket.onclose = (event: CloseEvent) => {
                this.fire('disconnect', event);
            };
        }

        public disconnect() {
            if (this._socket !== null) {
                this._socket.close();
            }
        }

        public reconnect(): void {
            this.disconnect();
            this.connect();
        }

        public sendPacket(packet: any) {
            this._socket.send(JSON.stringify(packet));
        }

        public registerPacket(opCode: OpCode, callback: (packet: any) => void) {
            this._packetCallbacks[opCode] = (this._packetCallbacks[opCode] || []);
            this._packetCallbacks[opCode].push(callback);
        }

        // Parses an incoming message accordingly
        private parseMessage(response: MessageEvent) {
            console.log(response);
            var packet: any = JSON.parse(response.data);
            if (this._packetCallbacks[packet.opCode] != undefined) {
                _.forEach(this._packetCallbacks[packet.opCode], (value: Function) => {
                    value(packet);
                });
            } else {
                Logger.warn("A packet with the ID of " + packet.opCode + " received but not handled");
            }
        }
    }
}