var __extends = this.__extends || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    __.prototype = b.prototype;
    d.prototype = new __();
};
var OpenORPG;
(function (OpenORPG) {
    var BootState = (function (_super) {
        __extends(BootState, _super);
        function BootState() {
            _super.apply(this, arguments);
        }
        BootState.prototype.create = function () {
            //TODO:  Do a change for disabilityChange here
            // Setup our connecting splash
            var connectingSplash = this.game.add.sprite(1024 / 2, 768 / 2, "connecting", 0);
            connectingSplash.anchor.setTo(0.5, 0.5);

            var port = 1234;
            var host = "localhost";
        };

        BootState.prototype.preload = function () {
            this.game.load.image("connecting", "assets/ui/connecting.png");
        };
        return BootState;
    })(Phaser.State);
    OpenORPG.BootState = BootState;
})(OpenORPG || (OpenORPG = {}));
//# sourceMappingURL=MyState.js.map
