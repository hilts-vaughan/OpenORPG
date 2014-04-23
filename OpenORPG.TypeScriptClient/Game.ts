/// <reference path="phaser.d.ts" />

module OpenORPG {

    export class Game {

        constructor() {
            // Init our game
            this.game = new Phaser.Game(1024, 768, Phaser.AUTO, 'gameContainer', null, true, true);
        }

        game: Phaser.Game;

        preload() {
            this.game.state.add("boot", new BootState(), true);
        }

        create() {

        }

    }

}