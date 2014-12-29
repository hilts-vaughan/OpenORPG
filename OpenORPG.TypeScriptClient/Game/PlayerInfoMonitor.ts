module OpenORPG {

    export class PlayerInfoMontior {

        private playerInfo: PlayerInfo;

        // Setup some network events here and be done with it
        constructor(playerInfo: PlayerInfo) {
            this.playerInfo = playerInfo;
            var that = this;
            var network = NetworkManager.getInstance();

            setInterval(() => {
                this.updateAngularScope();
            }, 1000);

            // Register for stat changes
            network.registerPacket(OpCode.SMSG_STAT_CHANGE, (packet: any) => {

                // Update these, fire callback
                this.playerInfo.characterStats[packet.stat].currentValue = packet.currentValue;
                this.playerInfo.characterStats[packet.stat].maximumValue = packet.maximumValue;

                this.updateAngularScope();

            });

            // Hook into our network events
            network.registerPacket(OpCode.SMSG_STORAGE_HERO_SEND, (packet) => {

                this.playerInfo.inventory = [];
                this.playerInfo.inventory.push.apply(this.playerInfo.inventory, packet.itemStorage);

                this.updateAngularScope();

                Logger.info("PlayerInfoMonitor - The player inventory has been updated.");
            });

            network.registerPacket(OpCode.SMSG_QUEST_SEND_LIST, (packet) => {
           
                this.playerInfo.quests = [];

                Logger.debug("PlayerInfoMonitor - Dumping new quest data...");
                Logger.debug(packet.questLog);
                _.each(packet.questLog, (value: any) => {

                    ContentManager.getInstance().getContent(ContentType.Quest, value.quest.questId, (data) => {
                        // Copy the state over for usage
                        data.state = value.state;

                        this.playerInfo.quests.push(data);                                              
                        this.updateAngularScope();
                    });

                });


            });


            // skills monitoring

            // Listen to events about player information we might care about
            network.registerPacket(OpCode.SMSG_SKILL_CHANGE, (packet) => {

                this.playerInfo.characterSkills = [];

                // Init character info
                for (var key in packet.skills) {
                    var skill = packet.skills[key];

                    ContentManager.getInstance().getContent(ContentType.Skill, (parseInt(key) + 1), (fSkill) => {
                        fSkill.cooldown = skill.cooldown;                        
                        this.playerInfo.characterSkills.push(new Skill(fSkill));

                        // Add some logging
                     
                        Logger.info("PlayerInfoMonitor - Player skills have been updated.");
                        Logger.info(this.playerInfo.characterSkills);
                    });

             
                }


            });


        }

        /**
         * Updates the angular rootscope with the latest data after modifying the rootscope.
         */
        private updateAngularScope() {
            var $body = angular.element(document.body);   // 1    

            var service = $body.injector().get('$timeout');
            var $rootScope: any = $body.scope();

            var phase = $rootScope.$root.$$phase;
            if (phase == '$apply' || phase == '$digest') {
            } else {
                $rootScope.$apply();
            }



        }



    }


}