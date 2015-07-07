/// <reference path="./AbstractState.ts" />
/// <reference path="../UI/Panel.ts" />

module OpenORPG {
    export class LoginMenuState extends AbstractState {
        private static _instance: LoginMenuState;

        public static get instance(): LoginMenuState {
            return this._instance;
        }

        public loginPanel: LoginPanel;

        constructor() {
            super();

            Interface.game.controller(new LoginController(this));

            LoginMenuState._instance = this;

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
                this.login();
            }
        }

        public login(): void {
            /*//TODO: Get query parameters working
            var options = this.game.net.getQueryString("username");*/
            var loginPacket = PacketFactory.createLoginPacket(Settings.getInstance().savedUsername, Settings.getInstance().savedPassword);

            NetworkManager.getInstance().sendPacket(loginPacket);
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
            this.loginPanel.hide();
        }
    }

    class LoginController implements IController {
        private state: LoginMenuState;

        constructor(state: LoginMenuState) {
            this.state = state;
        }

        public get name(): string {
            return "ControllerPanelLogin";
        }

        public get angular(): ($scope: any, $rootScope: any) => any {
            var state = this.state;
            return ($scope: any, $rootScope: any) => {
                $scope.rememberUsername = function (event: JQueryEventObject) {
                    state.loginPanel.refreshCheckboxes(true, false);
                };
                /* IDEA: Create controller object model with root interface to be added to each game state */
                $scope.rememberPassword = function (event: JQueryEventObject) {
                    state.loginPanel.refreshCheckboxes(false, true);
                };

                $scope.login = function () {
                    console.log("test");
                    state.loginPanel.updateSettings();
                    state.login();
                };

                $scope.register = function () {

                };
            };
        }
    }

    /* TODO: Refactor stuff when done writing more UI code */
    class LoginPanel extends UI.Panel {
        private _rememberUser: UI.Checkbox;
        private _rememberPass: UI.Checkbox;
        private _username: UI.Element;
        private _password: UI.Element;

        constructor(parent: JQuery) {
            super(parent, "assets/states/login.html");
        }

        public onLoad(event: JQueryEventObject): void {
            super.onLoad(event);

            var that = this;
            
            this._rememberUser = new UI.Checkbox(this.element, "#remember-user");
            this._rememberPass = new UI.Checkbox(this.element, "#remember-pass");

            this._username = new UI.Element(this.element, "#login-username");
            if (Settings.getInstance().saveUsername) {
                this._username.element.val(Settings.getInstance().savedUsername);
            }

            this._password = new UI.Element(this.element, "#login-password");
            if (Settings.getInstance().savePassword) {
                this._password.element.val(Settings.getInstance().savedPassword);
            }

            this.refreshCheckboxes();
        }

        public updateSettings(): void {
            Settings.getInstance().savedUsername = null;
            Settings.getInstance().savedPassword = null;

            if (Settings.getInstance().saveUsername) {
                Settings.getInstance().savedUsername = this._username.element.val();
            }

            if (Settings.getInstance().savePassword) {
                Settings.getInstance().savedPassword = this._password.element.val();
            }

            Settings.getInstance().save();
        }

        public refreshCheckboxes(toggleUser: boolean = false, togglePass: boolean = false) {
            if (toggleUser) {
                Settings.getInstance().saveUsername = !Settings.getInstance().saveUsername;

                /* This makes it so toggling the username saving off while
                password saving is on will disable password saving as well. */
                Settings.getInstance().savePassword = Settings.getInstance().saveUsername && Settings.getInstance().savePassword;
            }

            if (togglePass) {
                Settings.getInstance().savePassword = !Settings.getInstance().savePassword;
            }

            /* This makes it so toggling the password saving on while
            username saving is off will enable user saving as well. */
            Settings.getInstance().saveUsername = Settings.getInstance().saveUsername || Settings.getInstance().savePassword;

            Settings.getInstance().save();

            this._rememberUser.checked = Settings.getInstance().saveUsername;
            this._rememberPass.checked = Settings.getInstance().savePassword;
        }
    }
}