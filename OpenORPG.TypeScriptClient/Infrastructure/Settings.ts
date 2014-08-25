module OpenORPG {

    export class Settings {

        private static _instance: Settings = null;
        private settingsNamespace = "orpg_settings";

        // Some setting properties can go here
        public autoLoginSet: boolean;
        public savedUsername: string;
        public savedPassword: string;

        public playBGM : boolean;

        constructor() {
            var settings = localStorage[this.settingsNamespace];

            // Set some defaults if required
            if (!settings) {
                this.autoLoginSet = true;
                this.save();
            } else {
                // Copy the entire settings into here

                _.extend(this, JSON.parse(settings));
            }
        }


        public static getInstance(): Settings {
            if (Settings._instance === null) {
                Settings._instance = new Settings();
            }
            return Settings._instance;
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