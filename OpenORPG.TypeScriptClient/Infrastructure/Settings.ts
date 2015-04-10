module OpenORPG {

    export class Settings {

        private static _instance: Settings = null;
        private settingsNamespace = "orpg_settings";

        // Some setting properties can go here
        public autoLoginSet: boolean = true;
        public savedUsername: string;
        public savedPassword: string;

        public playBGM: boolean = true;
        public playSE : boolean = true;

        // Debug related flags
        public debugShowBodies: boolean;
        public debugShowInterpolationPaths: boolean;
        public debugShowPlayerInfo : boolean;

        private _handler: Function;
        private _context : any;

        constructor() {
            var settings = localStorage[this.settingsNamespace];

            // Set some defaults if required
            if (!settings) {
                this.autoLoginSet = true;
                this.save();
            } else {
                // Copy the entire settings into here
                _.extend(this, JSON.parse(settings, (key, value) => {
                    if (typeof value == "function" || value == this._context) {
                        return undefined;
                    }
                    return value;
                }));
                this.save();
            }
        }


        public static getInstance(): Settings {
            if (Settings._instance === null) {
                Settings._instance = new Settings();
            }
            return Settings._instance;
        }

        
        onChange(handler: Function, context : any) {            
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
            var json = JSON.stringify(this);
            localStorage[this.settingsNamespace] = json;
        }




    }

} 