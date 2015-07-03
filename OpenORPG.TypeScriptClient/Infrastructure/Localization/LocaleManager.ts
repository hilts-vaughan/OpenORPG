module OpenORPG {
    
    /**
     * Provides localization services to the entire game application. 
     */
    export class LocaleManager {

        private static _instance: LocaleManager = null;
        private _localeCode : string;
        private currentLocaleData : Object;

        constructor(locale: string) {
            LocaleManager._instance = this;            
            this.setLocale(locale);            
        }
        
        /**
         * Given a dot delimited message type, applies arguments to it and returns the localized version of it
         * for the current locale code.
         */
        public getString(messageType: string, args : Array<string> ) {
            var str = this.currentLocaleData[messageType];
            if (str) {                
                if (args) {
                    for (var i = 0; i < args.length; i++)
                        str = str.split("{" + i + "}").join(args[i]);
                }
            } else {
                str = "Error! No translation found for message type of " + messageType + " given locale code of " + this._localeCode;
            }
            return str;
        }

        /**
         * Sets the locale of the manager based on a locale code. Supported locale codes are:
         * EN: English
         * FR: French
         * JP: Japanese
         * DV: Development 
         */
        public setLocale(localeCode : string) {
            this._localeCode = localeCode;
            $.getJSON("assets/locale/" + localeCode + ".json", (data) => {
                this.currentLocaleData = data;
            });
        }

        public static getInstance(): LocaleManager {
            if (LocaleManager._instance === null) {
                LocaleManager._instance = new LocaleManager("en");
            }
            return LocaleManager._instance;
        }

    }
}