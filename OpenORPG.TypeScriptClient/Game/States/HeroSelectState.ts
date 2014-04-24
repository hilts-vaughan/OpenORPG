module OpenORPG {
    export class HeroSelectState extends Phaser.State {

        preload() {
            // Prepare sprites
            SpriteManager.loadSpriteDefintions(this.game);
        }

        create() {

            // Select our first hero
            var packet = PacketFactory.createHeroSelectPacket(1);

            // Bind a network event

            var network = NetworkManager.getInstance();
            network.registerPacket(OpCode.SMSG_HERO_SELECT_RESPONSE, (packet) => {
                this.game.state.add("game", new GameplayState());
                this.game.state.start("game");
            });

            // Send our packet as required
            network.sendPacket(packet);

        }

        constructor() {
            super();
        }


    }

} 