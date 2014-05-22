
module OpenORPG {

    
    export class Game {
        
 

        constructor() {

            // Init our game

            var width = $("#gameContainer").width();
            var height = $("#canvasholder").height();
            this.game = new Phaser.Game(1336, 768, Phaser.CANVAS, 'canvasholder', null, true, false);
  


            this.game.state.add("boot", new BootState(), true);
            this.game.state.start("boot");    
        }

        game: Phaser.Game;

        preload() {
            this.game.canvas.id = "gamecanvas";
            var c : any = this.game.stage;
            c.canvas.id = "gamecanvas";

            $("#gameContainer").attr("tabindex", 1);
        }

        create() {
      
        }

    }

}