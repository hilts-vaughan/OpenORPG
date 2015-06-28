module OpenORPG {
    export class LoginMenuState extends AbstractState {
        private loginPanelWidget: LoginPanelWidget;

        constructor() {
            super();
            
            this.loginPanelWidget = new LoginPanelWidget($("#canvasholder"));
        }

        create() {
            var network = NetworkManager.getInstance();

            network.registerPacket(OpCode.SMSG_LOGIN_RESPONSE, (packet: any) => {
                if (packet.status == 1) {
                    this.game.state.add("heroselect", new HeroSelectState());
                    this.game.state.start("heroselect");
                }

            });

            if (Settings.getInstance().autoLoginSet) {
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

        shutdown() {
            this.loginPanelWidget.hide();
        }
    }
}