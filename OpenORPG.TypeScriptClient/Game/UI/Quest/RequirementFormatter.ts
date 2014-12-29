module OpenORPG {
  
    /*
     *  The RequirementFormatter is responsible for maintaing a simple map of formatters and printing out the 
     *  quest requirements in an easy to digest, and easy to read way. It utilizes the local manager internally
     *  to ensure these are completely localized.
     * 
     *  NOTE: Do not hard-code any requirement strings in here
     */
    export class RequirementFormatter {
        
        private formatters : Object;

        constructor() {
            this.formatters = new Object();

            this.formatters["QuestMonstersKilledRequirement"] = new MonstersKilledFormatter();
            this.formatters["QuestHasItemRequirement"] = new ItemRequirementFormatter();
        }

        getFormattedRequirement(type: string, info: any, callback: Function) {            
            if (this.formatters[type]) {
                this.formatters[type].getLocalizedString(info, (result) => {
                    callback(result);
                });
            } else {
                Logger.warn("The quest requirement formatter for the type of " + type + " could not be found. It should be added or UI elements will look wrong.");
                callback(LocaleManager.getInstance().getString("RequirementFormatterMissing", []));
            }

        }


    }
      
} 