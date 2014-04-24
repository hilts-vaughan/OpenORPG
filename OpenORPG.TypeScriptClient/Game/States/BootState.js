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
            var _this = this;
            // Setup our connecting splash
            var connectingSplash = this.game.add.sprite(1024 / 2, 768 / 2, "connecting", 0);
            connectingSplash.anchor.setTo(0.5, 0.5);

            // Connect
            var network = OpenORPG.NetworkManager.getInstance();

            network.onConnectionCallback = function () {
                _this.game.state.add("mainmenu", new OpenORPG.LoginMenuState());
                _this.game.state.start("mainmenu");
            };

            network.onConnectionErrorCallback = function () {
                _this.game.state.start("errorstate");
            };

            // Do a connection
            network.connect();
        };

        BootState.prototype.preload = function () {
            //TODO: Do some of the sprite loading we might want to do here
            this.game.load.image("connecting", "assets/ui/connecting.png");
        };
        return BootState;
    })(Phaser.State);
    OpenORPG.BootState = BootState;
})(OpenORPG || (OpenORPG = {}));
//# sourceMappingURL=BootState.js.map
