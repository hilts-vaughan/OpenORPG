module OpenORPG {
    export class Settings {
        private static _instance: Settings = null;
        private settingsNamespace = "orpg_settings";

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

        private attemptLoad() {
            var settings = localStorage[this.settingsNamespace];

            // Set some defaults if required
            if (!settings) {
                this.autoLoginSet = true;
                this.saveUsername = false;
                this.savePassword = false;

                this.save();
            } else {
                // Copy the entire settings into here
                _.extend(this, JSON.parse(settings));
                this.save();
            }
        }

        public static getInstance(): Settings {
            if (Settings._instance === null) {
                Settings._instance = new Settings();
            }
            return Settings._instance;
        }


        onChange(handler: Function, context: any) {
            this._handler = handler;
            this._context = context;
        }

        flush() {
            this._handler.call(this._context);
        }

        /*
         * Persists the entire settings to local storage
         */
        save() {
            var json = JSON.stringify(this,(key, value) => {
                if (typeof value == "function" || value == this._context) {
                    return undefined;
                }

                if (typeof value == "boolean") {
                    return (value ? 1 : 0);
                }

                return value;
            });

            localStorage[this.settingsNamespace] = json;
        }

        reset() {
            localStorage.removeItem(this.settingsNamespace);
            this.attemptLoad();
        }
    }
} 