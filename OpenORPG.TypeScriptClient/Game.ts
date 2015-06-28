module OpenORPG {
    export class Game {
        constructor() {
            var windowWidth = window.innerWidth;
            var windowHeight = window.innerHeight;

            var width = windowWidth;//Math.round(Math.min(windowWidth, windowHeight * 16.0 / 9.0));
            var height = windowHeight;//Math.round(Math.min(windowHeight, width * 9.0 / 16.0));

            //var canvasHolder = document.getElementById("canvasholder");
            this.game = new Phaser.Game(width, height, this.pickRenderer(), 'canvasholder', this, true, false);
            console.log(window.onresize);
            window.onresize = function (evt: UIEvent) {
                this.game.camera.setSize(window.innerWidth, window.innerHeight);
                this.game.scale.setGameSize(window.innerWidth, window.innerHeight);
                this.game.scale.refresh();
                console.log(this.game.scale);
                console.log(this.game.camera);
            }.bind(this);
            
            this.game.state.add("boot", new BootState(), false);
        }

        pickRenderer(): number {
            if (Settings.getInstance().debugForceWebGl)
                return Phaser.WEBGL;

            return Phaser.CANVAS;
        }

        aspectRatio() {
            return (window.innerWidth / window.innerHeight);
        }

        game: Phaser.Game;

        preload() {

        }

        create() {
            // Setup the game manager here, respond to changes in settings across the global phaser network
            var settings: Settings = Settings.getInstance();
            settings.onChange(this.updateSettings, this);
            settings.flush();

            this.game.state.start("boot");
        }

        updateSettings() {
            var settings: Settings = Settings.getInstance();
            this.game.sound.mute = !settings.playBGM;

            // Phaser uses a range between 0 and 1 for audio, we need to scale it down
            this.game.sound.volume = settings.volume / 100;
        }
    }
}