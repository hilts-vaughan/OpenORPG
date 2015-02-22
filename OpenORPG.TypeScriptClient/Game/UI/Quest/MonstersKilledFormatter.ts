module OpenORPG {
    
    export class MonstersKilledFormatter implements IQuestRequirementFormatter {
        
        private infoType: string = "QuestMonstersKilledRequirement";

        getLocalizedString(info: any, progress : number, callback : Function) {

            var monsterId: string = info.monsterId;
            var monsterAmount: string = info.monsterAmount;

            ContentManager.getInstance().getContent(ContentType.Monster, info.monsterId, (monster) => {
                 var result : string = LocaleManager.getInstance().getString(this.infoType, [monster.name, monsterAmount, progress]);
                callback(result);
            });
        }
    }

} 