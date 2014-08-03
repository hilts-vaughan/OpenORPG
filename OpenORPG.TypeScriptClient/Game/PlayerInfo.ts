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


        public inventory: Array<Item> = new Array<Item>();
        private inventoryCallbacks = [];

        constructor() {

            var that = this;

            // Listen to events about player information we might care about
            NetworkManager.getInstance().registerPacket(OpCode.SMSG_SKILL_CHANGE, (packet) => {

                that.characterSkills = new Array<Skill>();

                // Init character info
                for (var key in packet.skills) {
                    var skill = packet.skills[key];

                    $.getJSON("assets/gamesfiles/skills/" + (parseInt(key) + 1) + ".json", (fSkill) => {
                        var newSkill : any = _.extend(fSkill, skill);
                        that.characterSkills.push(new Skill(newSkill));
                    });


                }


            });

        }


        listenCharacterStatChange(callback: Function) {
            this.characterStatsCallbacks.push(callback);
        }


        onCharacterStatChange() {
            this.characterStatsCallbacks.forEach(this.callCallback);
        }




        callCallback(element: Function) {
            element();
        }




    }
}