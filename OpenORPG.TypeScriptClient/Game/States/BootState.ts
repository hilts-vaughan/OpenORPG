module OpenORPG {
    export class BootState extends AbstractState {
        create() {
            Logger.info("Booting the game...");

            this.game.stage.disableVisibilityChange = true;
            this.game.scale.setGameSize(window.innerWidth, window.innerHeight);
            
            /* Setup the connecting splash screen */
            var connectingSplash = this.game.add.sprite(1024 / 2, 768 / 2, "connecting", 0);
            connectingSplash.anchor.setTo(0.5, 0.5);
            
            /* Set up the network object. */
            var network = NetworkManager.getInstance();

            network.onConnectionCallback = () => {
                this.game.state.add("mainmenu", new LoginMenuState());
                this.game.state.start("mainmenu");
            }

            network.onConnectionErrorCallback = () => {
                this.game.state.start("errorstate");
            }

            /* Connect to the server */
            Logger.info("Connecting to the server...");
            network.connect();
        }

        preload() {
            //TODO: Do some of the sprite loading we might want to do here
            this.game.load.image("connecting", "assets/ui/connecting.png");
        }
    }
}