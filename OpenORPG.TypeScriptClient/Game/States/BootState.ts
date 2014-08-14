module OpenORPG {
    export class BootState extends Phaser.State {


        create() {

            this.game.stage.disableVisibilityChange = true;
            
            // Setup our connecting splash
            var connectingSplash = this.game.add.sprite(1024 / 2, 768 / 2, "connecting", 0);
            connectingSplash.anchor.setTo(0.5, 0.5);

            // Connect
            var network = NetworkManager.getInstance();

            Logger.info("Booting the game...");

            network.onConnectionCallback = () => {
                this.game.state.add("mainmenu", new LoginMenuState());
                this.game.state.start("mainmenu");
            }

            network.onConnectionErrorCallback = () => {
                this.game.state.start("errorstate");
            }

            // Do a connection
            network.connect();

        }

        preload() {
            //TODO: Do some of the sprite loading we might want to do here
            this.game.load.image("connecting", "assets/ui/connecting.png");
        }

    }

}