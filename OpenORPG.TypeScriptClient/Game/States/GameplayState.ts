
module OpenORPG {


    // The gameplay state manages 
    export class GameplayState extends Phaser.State {

        private zone: Zone = null;
        private currenTrack: Phaser.Sound;



        private characterHud: CharacterStatusWidget;
        private bottomBarWidget: BottombarWidget;
        private chatWidget: ChatWidget;
        private menuWidget: MenuTrayWidget;
        private questWindow: QuestWindow = new QuestWindow();

        // Keep track of character info
        private playerInfo: PlayerInfo = new PlayerInfo();
        private playerMonitor: PlayerInfoMontior;

        constructor() {
            super();

            Logger.trace("GameplayState - Creating object and setting up handlers");

            var that = this;

            var $body = angular.element(document.body);   // 1
            var $rootScope: any = $body.scope();
            $rootScope = $rootScope.$root;
            $rootScope.$apply(function () {               // 3
                $rootScope.playerInfo = that.playerInfo;
            });


            this.playerMonitor = new PlayerInfoMontior(this.playerInfo);

       

            this.characterHud = new CharacterStatusWidget($("#canvasholder"), this.playerInfo);
            this.bottomBarWidget = new BottombarWidget($("#canvasholder"));
            this.chatWidget = new ChatWidget($("#canvasholder"));
            this.menuWidget = new MenuTrayWidget($("#canvasholder"), this.playerInfo);


        }

        preload() {
            SpriteManager.loadSpriteImages(this.game);

            // Load up everything else
            var loader = this.game.load;

            loader.tilemap("map_1", "assets/Maps/1.json", null, Phaser.Tilemap.TILED_JSON);
            loader.tilemap("map_2", "assets/Maps/2.json", null, Phaser.Tilemap.TILED_JSON);

            loader.image("tilesheet", "assets/Maps/tilesheet_16.png");
            loader.image("target_icon", "assets/img/target_selector.png");

            // Load all our audio
            loader.audio("audio_music_town", [DirectoryHelper.getMusicPath() + "town.ogg"]);
            loader.audio("audio_effect_hit", [DirectoryHelper.getAudioEffectPath() + "hit1.ogg"]);

        }

        render() {
            if (this.zone != null)
                this.zone.render();
        }

        create() {
            // Start our physics systems
            this.game.physics.startSystem(Phaser.Physics.ARCADE);
            this.zone = new Zone(this.game, this.playerInfo);

            var network = NetworkManager.getInstance();
            network.registerPacket(OpCode.SMSG_ZONE_CHANGED, (packet: any) => {

                this.zone.clearZone();

                // Stop current music and all sound effects
                this.game.sound.stopAll();

                // Load new audio track in
                this.currenTrack = this.game.add.audio("audio_music_town", 0.5, true, true);
                this.currenTrack.play();

                this.zone.initLocalZone(packet.zoneId);


                for (var entityKey in packet.entities) {
                    var entity = packet.entities[entityKey];

                    // Create your objects here 
                    var worldEntity: Entity = this.zone.addNetworkEntityToZone(entity);

                    // Do camera following here
                    if (worldEntity.id == packet.heroId) {
                        this.game.camera.follow(worldEntity);
                        this.zone.movementSystem.attachEntity(worldEntity);
                        this.zone.combatSystem.attachTo(worldEntity);
                        this.playerInfo.player = worldEntity;

                        // Init character info
                        for (var key in entity.characterStats.stats) {
                            var statObject = entity.characterStats.stats[key];
                            this.playerInfo.characterStats[statObject.statType] = { currentValue: statObject.currentValue, maximumValue: statObject.maximumValue };
                        }



                        this.playerInfo.name = worldEntity.name;


                        var $body = angular.element(document.body);   // 1
                        var $rootScope: any = $body.scope();
                        $rootScope = $rootScope.$root;
                        $rootScope.$apply();


                    }

                }
            });



            network.registerPacket(OpCode.SMSG_SERVER_OFFER_QUEST, (packet: any) => {
                this.questWindow.presentQuest(packet.questId);
            });

        }

        update() {
            if (this.zone != null)
                this.zone.update();
        }


    }

}