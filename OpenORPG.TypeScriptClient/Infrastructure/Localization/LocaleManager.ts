module OpenORPG {

    export class LocaleManager {

        private static _instance: LocaleManager = null;
        private locale : string;
        private currentLocaleData : Object;

        constructor(locale: string) {
            LocaleManager._instance = this;

            // Set the locale
            this.setLocale(locale);            
        }

        public getString(messageType: string) {
            return this.currentLocaleData[messageType];
        }

        public setLocale(locale : string) {
            $.getJSON("assets/locale/" + locale + ".json", (data) => {
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