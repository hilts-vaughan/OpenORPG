module OpenORPG {
    export class BootState extends Phaser.State {

        create() {
            //TODO:  Do a change for disabilityChange here

            // Setup our connecting splash
            var connectingSplash = this.game.add.sprite(1024 / 2, 768 / 2, "connecting", 0);
            connectingSplash.anchor.setTo(0.5, 0.5);

            var port = 1234;
            var host = "localhost";

        }

        preload() {
            this.game.load.image("connecting", "assets/ui/connecting.png");
        }

    }
}