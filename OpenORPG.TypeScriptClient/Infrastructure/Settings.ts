module OpenORPG {
    var jsonSerializer: { [typeName: string]: (value: any) => any } = {};
    jsonSerializer["function"] = (value: any) => undefined;
    jsonSerializer["boolean"] = (value: any) => (value ? 1 : 0);
    
    var jsonDeserializer: { [typeName: string]: (value: any) => any } = {};
    jsonDeserializer["boolean"] = (value: any) => (Number(value) === 0 ? false : true);

    function safeConverter(func: (value: any) => any): (value: any) => any {
        if (func) return func;
        return (value: any) => { return value; };
    }

    export class Settings {
        private static _instance: Settings = null;

        public static getInstance(): Settings {
            if (Settings._instance === null) {
                Settings._instance = new Settings();
            }

            return Settings._instance;
        }

        private namespace = "orpg_settings";

        /* Login */
        public autoLoginSet: boolean = true;

        public saveUsername: boolean = false;
        public savedUsername: string;

        public savePassword: boolean = false;
        public savedPassword: string;

        public playBGM: boolean = true;
        public playSE: boolean = true;
        public _volume: number = 100;

        get volume(): any {
            return this._volume;
        }

        set volume(val: any) {
            this._volume = parseInt(val);
        }

        // Debug related flags
        public debugShowBodies: boolean;
        public debugShowInterpolationPaths: boolean;
        public debugShowPlayerInfo: boolean;

        // This flag is responsible for forcing WebGL views
        public debugForceWebGl: boolean = false;

        private _handler: Function;
        private _context: any;

        constructor() {
            this.attemptLoad();
        }

        private attemptLoad(): void {
            var settings = localStorage[this.namespace];

            // Set some defaults if required
            if (!settings) {
                this.autoLoginSet = true;
                this.saveUsername = false;
                this.savePassword = false;
            } else {
                // Copy the entire settings into here
                var that = this;
                _.extend(this, JSON.parse(settings, (key: any, value: any) => {
                    return safeConverter(jsonDeserializer[typeof that[key]])(value);
                }));
            }

            this.save();
        }

        public onChange(handler: Function, context: any): void {
            this._handler = handler;
            this._context = context;
        }

        /*
         * Persists the entire settings to local storage
         */
        public save(): void {
            this.flush();

            var json = JSON.stringify(this,(key: string, value: any) => {
                if (value == this._context) {
                    return undefined;
                }

                return safeConverter(jsonSerializer[typeof value])(value);
            });

            localStorage[this.namespace] = json;
        }

        public flush(): void {
            if (this._handler) this._handler.call(this._context);
        }

        public reset(): void {
            localStorage.removeItem(this.namespace);
            this.attemptLoad();
        }
    }
} 