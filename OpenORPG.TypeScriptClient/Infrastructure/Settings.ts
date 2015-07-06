module OpenORPG {
    var jsonConverters: { [typeName: string]: (value: any) => any } = {};
    jsonConverters["function"] = (value: any) => undefined;
    jsonConverters["boolean"] = (value: any) => (value ? 1 : 0);

    function convert(value: any): any {
        var converter = jsonConverters[typeof value];

        if (converter) {
            return converter(value);
        }

        return value;
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
                _.extend(this, JSON.parse(settings));
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

                return convert(value);
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