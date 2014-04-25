module OpenORPG {
    export class LoginMenuState extends Phaser.State {

        create() {

            // Create our login button
            var button = this.game.add.button(1024 / 2, 768 / 2 + 120, "play_button", null, this, 1, 1, 2);
            button.anchor.setTo(0.5, 0.5);

            var text = this.game.add.text(1024 / 2, 768 / 2 - 170, "Select your hero", FontFactory.getPlayerFont());
            text.anchor.set(0.5, 0.5);

            var network = NetworkManager.getInstance();

            network.registerPacket(OpCode.SMSG_LOGIN_RESPONSE, (packet: any) => {
                if (packet.status == 1) {
                    this.game.state.add("heroselect", new HeroSelectState());
                    this.game.state.start("heroselect");
                }

            });

            if (Settings.autoLoginSet()) {
                var options = this.game.net.getQueryString("username");
  

                //TODO: Get query parameters working
                var loginPacket = PacketFactory.createLoginPacket(options["user"], options["password"]);
                network.sendPacket(loginPacket);
            }


        }

        preload() {
            var loader = this.game.load;

            // Load up the resources we need for here
            loader.image("scroll_bg", "assets/ui/scroll.png");
            loader.image("player_active", "assets/ui/player_active.png");
            loader.image("player_inactive", "assets/ui/player_inactive.png");
            loader.spritesheet("play_button", "assets/ui/play_button.png", 394, 154);

            SpriteManager.loadSpriteInfo(this.game);
        }


    }

}