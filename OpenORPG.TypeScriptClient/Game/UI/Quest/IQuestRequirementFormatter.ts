module OpenORPG {

    /*
         A basic interface that allows quest requirements to be formatted nicely on the UI side of things.
         Allows custom logic to be applied where required
     */
    export interface  IQuestRequirementFormatter {
        getLocalizedString(info : any, progress : number, callback);
    }

}