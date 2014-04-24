var __extends = this.__extends || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    __.prototype = b.prototype;
    d.prototype = new __();
};
var OpenORPG;
(function (OpenORPG) {
    var HeroSelectState = (function (_super) {
        __extends(HeroSelectState, _super);
        function HeroSelectState() {
            _super.call(this);
        }
        HeroSelectState.prototype.preload = function () {
            // Prepare sprites
            SpriteManager.loadSpriteDefintions(this.game);
        };

        HeroSelectState.prototype.create = function () {
            var _this = this;
            // Select our first hero
            var packet = PacketFactory.createHeroSelectPacket(1);

            // Bind a network event
            var network = OpenORPG.NetworkManager.getInstance();
            network.registerPacket(6 /* SMSG_HERO_SELECT_RESPONSE */, function (packet) {
                _this.game.state.add("game", new OpenORPG.GameplayState());
                _this.game.state.start("game");
            });

            // Send our packet as required
            network.sendPacket(packet);
        };
        return HeroSelectState;
    })(Phaser.State);
    OpenORPG.HeroSelectState = HeroSelectState;
})(OpenORPG || (OpenORPG = {}));
//# sourceMappingURL=HeroSelectState.js.map
