
module OpenORPG {

    
    export class Game {

        constructor() {

            // Init our game

            var width = $("#gameContainer").width();
            var height = $("#gameContainer").height();
            this.game = new Phaser.Game(1336, 786, Phaser.CANVAS, 'gameContainer', null, true, false);
            

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