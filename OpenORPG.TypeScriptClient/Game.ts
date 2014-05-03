
module OpenORPG {

    
    export class Game {

        constructor() {

            // Init our game

            var width = $("#gameContainer").width();
            var height = $("#canvasholder").height();
            this.game = new Phaser.Game(1336, 700, Phaser.CANVAS, 'canvasholder', null, true, false);
            

            this.game.state.add("boot", new BootState(), true);
            this.game.state.start("boot");    
        }

        game: Phaser.Game;

        preload() {
   
        }

        create() {

        }

    }

}