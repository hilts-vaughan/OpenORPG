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

        public getString(messageType: string, args : Array<string> ) {
            var str = this.currentLocaleData[messageType];

            if (str) {

                // If the arguments are presented, use them
                if (args) {
                    for (var i = 0; i < args.length; i++)
                        str = str.split("{" + i + "}").join(args[i]);
                }

            } else {
                str = "Error! No translation found for message type of " + messageType;
            }

            return str;
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