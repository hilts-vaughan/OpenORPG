module OpenORPG {

     export class ItemRequirementFormatter implements IQuestRequirementFormatter {

         private infoType: string = "QuestHasItemRequirement";

         getLocalizedString(info: any, progress : number, callback: Function) {

             var itemId: string = info.itemId;
             var itemAmount: string = info.itemAmount;

             ContentManager.getInstance().getContent(ContentType.Item, info.itemId, (item) => {
                 var result: string = LocaleManager.getInstance().getString(this.infoType, [item.name, itemAmount]);
                 callback(result);
             });
         }
     }
 }