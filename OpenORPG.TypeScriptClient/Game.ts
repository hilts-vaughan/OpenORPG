
module OpenORPG {

    
    export class Game {

        constructor() {

            // Init our game
            this.game = new Phaser.Game(1024, 768, Phaser.CANVAS, 'gameContainer', null, true, false);
            

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