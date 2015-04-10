
module OpenORPG {

 
    
    export class Game {
        
 

        constructor() {

            // Init our game

            var width = $("#gameContainer").width();
            var height = $("#canvasholder").height();
            this.game = new Phaser.Game(1600, 900, Phaser.CANVAS, 'canvasholder', this, true, false);
              
            



            this.game.state.add("boot", new BootState(), false);
            
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
        }

    }

}