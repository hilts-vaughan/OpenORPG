module OpenORPG {

    export class PlayerInfoMontior {

        private playerInfo: PlayerInfo;

        // Setup some network events here and be done with it
        constructor(playerInfo: PlayerInfo) {
            this.playerInfo = playerInfo;
            var that = this;
            var network = NetworkManager.getInstance();

            setInterval(() => {
                AngularInterop.updateAngularScope();
            }, 1000);

            // Register for stat changes on ourselves, and apply them
            network.registerPacket(OpCode.SMSG_STAT_CHANGE, (packet: any) => {                               
                if (this.playerInfo.player.id == packet.characterId) {
                    this.playerInfo.characterStats[packet.stat].currentValue = packet.currentValue;
                    this.playerInfo.characterStats[packet.stat].maximumValue = packet.maximumValue;
                    AngularInterop.updateAngularScope();
                }
            });

            // Hook into our network events
            network.registerPacket(OpCode.SMSG_STORAGE_HERO_SEND, (packet) => {

                this.playerInfo.inventory = [];
                this.playerInfo.inventory.push.apply(this.playerInfo.inventory, packet.itemStorage);

                AngularInterop.updateAngularScope();

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
                        data.currentStep = value.currentStep;
                        data.questInfo = {};

                        // Push up our progress levels to accordingly
                        _.each(value.progress, (progressLevel, index) => {
                            data.questInfo.requirementProgress = [];
                            data.questInfo.requirementProgress.push({ progress: progressLevel });
                        });

               

                        this.playerInfo.quests.push(data);
                        AngularInterop.broadcastEvent('QuestsChanged');                                  
                        AngularInterop.updateAngularScope();
                    });

                });


            });


            // An update for quest progress stuff
            network.registerPacket(OpCode.SMSG_QUEST_PROGRESS_UPDATE, (packet) => {
                var updatedQuest = _.find(this.playerInfo.quests, (quest: any) => {
                    return quest.id == packet.questId;
                });

                // Update the progress indiciator; trigger a UI refresh
                updatedQuest.questInfo.requirementProgress[packet.requirementIndex].progress = packet.progress;

                AngularInterop.broadcastEvent('QuestsChanged');
                AngularInterop.updateAngularScope();

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




    }


}