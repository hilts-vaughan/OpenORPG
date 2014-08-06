module OpenORPG {

    export class PlayerInfoMontior {

        private playerInfo: PlayerInfo;

        // Setup some network events here and be done with it
        constructor(playerInfo: PlayerInfo) {
            this.playerInfo = playerInfo;
            var that = this;
            var network = NetworkManager.getInstance();

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
            });

            network.registerPacket(OpCode.SMSG_QUEST_SEND_LIST, (packet) => {

                this.playerInfo.quests = [];

                _.each(packet.quests, (value : any) => {

                    $.getJSON("assets/gamesfiles/quests/" + value.questId + ".json", (data) => {
                        this.playerInfo.quests.push(data);
                        this.updateAngularScope();
                    });

                });


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