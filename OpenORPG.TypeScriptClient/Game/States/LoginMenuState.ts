///<reference path="./AbstractState.ts" />
///<reference path="../UI/Panel.ts" />
///<reference path="../../OpenORPG/IController.ts" />

module OpenORPG {
    /* TODO: Refactor stuff when done writing more UI code */
    class LoginPanel extends UI.Panel {
        constructor(parent: JQuery) {
            super(parent, "assets/states/login.html");
        }
    }

    export class LoginMenuState extends AbstractState {
        private static _instance: LoginMenuState = null;

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

            if (Settings.getInstance().autoLoginSet && this.credentialsExist()) {
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

        credentialsExist() : boolean {
            return Settings.getInstance().savedUsername != null;
        }
    }

    class LoginController implements IController {
        private state: LoginMenuState;
        private scope: Scope;

        constructor(state: LoginMenuState) {
            this.state = state;
            this.scope = null;
        }

        public get name(): string {
            return "ControllerPanelLogin";
        }

        protected get settings(): Settings {
            if (this.scope == null) {
                return null;
            }

            return this.scope.settings;
        }

        public onSaveUsername(event: JQueryEventObject): void {
            this.settings.savePassword = this.settings.saveUsername && this.settings.savePassword;
        }

        public onSavePassword(event: JQueryEventObject): void {
            this.settings.saveUsername = this.settings.savePassword || this.settings.saveUsername;
        }

        public doLogin(): void {
            this.state.login();

            if (!this.settings.saveUsername) { this.settings.savedUsername = null; }
            if (!this.settings.savePassword) { this.settings.savedPassword = null; }

            this.settings.save();
        }

        public doRegister(): void {

        }

        public get angular(): ($scope: any, $rootScope: any) => any {
            var that = this;
            return ($scope: any, $rootScope: any) => {
                that.scope = $scope;
            };
        }
    }
}