var __extends = this.__extends || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    __.prototype = b.prototype;
    d.prototype = new __();
};
var OpenORPG;
(function (OpenORPG) {
    var GameplayState = (function (_super) {
        __extends(GameplayState, _super);
        function GameplayState() {
            _super.apply(this, arguments);
        }
        GameplayState.prototype.preload = function () {
            SpriteManager.loadSpriteImages(this.game);
            // Load up everything else
        };

        GameplayState.prototype.create = function () {
        };
        return GameplayState;
    })(Phaser.State);
    OpenORPG.GameplayState = GameplayState;
})(OpenORPG || (OpenORPG = {}));
//# sourceMappingURL=GameplayState.js.map
