var __extends = this.__extends || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    __.prototype = b.prototype;
    d.prototype = new __();
};
var OpenORPG;
(function (OpenORPG) {
    var LoginMenuState = (function (_super) {
        __extends(LoginMenuState, _super);
        function LoginMenuState() {
            _super.apply(this, arguments);
        }
        LoginMenuState.prototype.create = function () {
            var _this = this;
            // Create our login button
            var button = this.game.add.button(1024 / 2, 768 / 2 + 120, "play_button", null, this, 1, 1, 2);
            button.anchor.setTo(0.5, 0.5);

            var text = this.game.add.text(1024 / 2, 768 / 2 - 170, "Select your hero", FontFactory.getPlayerFont());
            text.anchor.set(0.5, 0.5);

            var network = OpenORPG.NetworkManager.getInstance();

            network.registerPacket(1 /* SMSG_LOGIN_RESPONSE */, function (packet) {
                if (packet.status == 1) {
                    _this.game.state.add("heroselect", new OpenORPG.HeroSelectState());
                    _this.game.state.start("heroselect");
                }
            });

            if (Settings.autoLoginSet()) {
                var username = this.game.net.getQueryString("username");
                var password = this.game.net.getQueryString("password");

                var loginPacket = PacketFactory.createLoginPacket(username, password);
                network.sendPacket(loginPacket);
            }
        };

        LoginMenuState.prototype.preload = function () {
            var loader = this.game.load;

            // Load up the resources we need for here
            loader.image("scroll_bg", "assets/ui/scroll.png");
            loader.image("player_active", "assets/ui/player_active.png");
            loader.image("player_inactive", "assets/ui/player_inactive.png");
            loader.spritesheet("play_button", "assets/ui/play_button.png", 394, 154);

            SpriteManager.loadSpriteInfo(this.game);
        };
        return LoginMenuState;
    })(Phaser.State);
    OpenORPG.LoginMenuState = LoginMenuState;
})(OpenORPG || (OpenORPG = {}));
//# sourceMappingURL=LoginMenuState.js.map
