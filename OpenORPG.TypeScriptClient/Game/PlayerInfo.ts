module OpenORPG {

    /*
     * A simple container class that has the job of simply containing amounts of information.
     * A simple flyweight class that can be passed around to tie in various pieces of info and
     * prevent duplication. Contains information like 
     * 
     */
    export class PlayerInfo {

        public name: string;

        // A small interface to character stats
        public characterStats: Array<CharacterStat> = new Array<CharacterStat>();
        public characterSkills: Array<Skill> = new Array<Skill>();

        private characterStatsCallbacks: Array<Function> = new Array<Function>();


        public inventory: any[];
        public quests: any[];
        public player : Entity;

        private inventoryCallbacks = [];

        constructor() {

            var that = this;
         


        }




    }
}