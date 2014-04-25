module OpenORPG {
    // The gameplay state manages 
    export class GameplayState extends Phaser.State {

        private zone: Zone = null;
        private currenTrack : Phaser.Sound;

        constructor() {
            super();

        }

        preload() {
            SpriteManager.loadSpriteImages(this.game);

            // Load up everything else
            var loader = this.game.load;

            loader.tilemap("map_1", "assets/Maps/1.json", null, Phaser.Tilemap.TILED_JSON);
            loader.tilemap("map_2", "assets/Maps/2.json", null, Phaser.Tilemap.TILED_JSON);

            loader.image("tilesheet", "assets/Maps/tilesheet_16.png");

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

            var network = NetworkManager.getInstance();
            network.registerPacket(OpCode.SMSG_ZONE_CHANGED, (packet: any) => {

                if (this.zone != null)
                    this.zone.clearZone();

                // Stop current music and all sound effects
                this.game.sound.stopAll();

                // Load new audio track in
                this.currenTrack = this.game.add.audio("audio_music_town", 0.5, true, true);
                this.currenTrack.play();


                this.zone = new Zone(this.game, packet.zoneId);

                for (var entityKey in packet.entities) {
                    var entity = packet.entities[entityKey];

                    // Create your objects here 
                    var worldEntity: Entity = this.zone.addNetworkEntityToZone(entity);

                    // Do camera following here
                    if (worldEntity.id == packet.heroId) {
                        this.game.camera.follow(worldEntity);
                        this.zone.movementSystem.attachEntity(worldEntity);
                    }

                }
            });
        }

        update() {
            if (this.zone != null)
                this.zone.update();
        }


    }

}