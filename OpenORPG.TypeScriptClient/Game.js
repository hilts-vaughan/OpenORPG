/// <reference path="phaser.d.ts" />
var OpenORPG;
(function (OpenORPG) {
    var Game = (function () {
        function Game() {
            // Init our game
            this.game = new Phaser.Game(1024, 768, Phaser.AUTO, 'gameContainer', null, true, false);
        }
        Game.prototype.preload = function () {
            this.game.state.add("boot", new OpenORPG.BootState(), true);
        };

        Game.prototype.create = function () {
        };
        return Game;
    })();
    OpenORPG.Game = Game;
})(OpenORPG || (OpenORPG = {}));
//# sourceMappingURL=Game.js.map
