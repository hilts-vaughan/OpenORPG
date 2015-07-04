///<reference path="../UI/Panel.ts" />

module OpenORPG {
    export class LoginMenuState extends AbstractState {
        private static _instance: LoginMenuState;

        public static get instance(): LoginMenuState {
            return this._instance;
        }

        private loginPanelWidget: LoginPanelWidget;
        public loginPanel: LoginPanel;

        constructor() {
            super();

            LoginMenuState._instance = this;
            /*var that = this;
            Angular.Game.controller('ControllerPanelLogin', [
                '$scope', '$rootScope', function ($scope, $rootScope) {
                    $scope.settings = $.extend({}, Settings.getInstance());
                    console.log("sdfuisdfuij");
                    $scope.rememberUsername = function () {
                        that.loginPanel.refreshCheckboxes(true, false);
                    };

                    $scope.rememberPassword = function () {
                        that.loginPanel.refreshCheckboxes(false, true);
                    };

                    $scope.login = function () {
                        $.extend(Settings.getInstance(), $scope.settings);
                        Settings.getInstance().flush();
                        Settings.getInstance().save();
                    };

                    $scope.register = function () {

                    };
                }
            ]);*/

            //this.loginPanelWidget = new LoginPanelWidget($("#canvasholder"));
            this.loginPanel = new LoginPanel($("#canvasholder"));
        }

        create() {
            var network = NetworkManager.getInstance();

            network.registerPacket(OpCode.SMSG_LOGIN_RESPONSE, (packet: any) => {
                if (packet.status == 1) {
                    this.game.state.add("heroselect", new HeroSelectState());
                    this.game.state.start("heroselect");
                }
            });

            if (Settings.getInstance().autoLoginSet) {
                //TODO: Get query parameters working
                var options = this.game.net.getQueryString("username");
                var loginPacket = PacketFactory.createLoginPacket(options["user"], options["password"]);

                network.sendPacket(loginPacket);
            }
        }

        preload() {
            var loader = this.game.load;

            // Load up the resources we need for here
            loader.image("scroll_bg", "assets/ui/scroll.png");
            loader.image("player_active", "assets/ui/player_active.png");
            loader.image("player_inactive", "assets/ui/player_inactive.png");
            loader.spritesheet("play_button", "assets/ui/play_button.png", 394, 154);

            SpriteManager.loadSpriteInfo(this.game);
        }

        shutdown() {
            //this.loginPanelWidget.hide();
        }
    }

    /* TODO: Refactor stuff when done writing more UI code */
    class LoginPanel extends UI.Panel {
        private _rememberUser: UI.Checkbox;
        private _rememberPass: UI.Checkbox;

        constructor(parent: JQuery) {
            super(parent, "assets/templates/widgets/login_panel.html");
        }

        public onLoad(event: JQueryEventObject): void {
            super.onLoad(event);

            var that = this;
            
            this._rememberUser = new UI.Checkbox(this.element, "#remember-user");
            this._rememberPass = new UI.Checkbox(this.element, "#remember-pass");

            this.refreshCheckboxes();
        }

        public refreshCheckboxes(toggleUser: boolean = false, togglePass: boolean = false) {
            if (toggleUser) {
                Settings.getInstance().saveUsername = !Settings.getInstance().saveUsername;
            }

            if (togglePass) {
                Settings.getInstance().savePassword = !Settings.getInstance().savePassword;
            }

            /* Since I couldn't get it working consistently, opting
             * to save the password does nothing unless you've already
             * opted to save the username, instead of the intended
             * functionality where opting to save the password would
             * also flip on saving the username. */
            Settings.getInstance().savePassword = Settings.getInstance().saveUsername && Settings.getInstance().savePassword;

            Settings.getInstance().flush();
            Settings.getInstance().save();

            this._rememberUser.checked = Settings.getInstance().saveUsername;
            this._rememberPass.checked = Settings.getInstance().savePassword;
        }
    }
}