module OpenORPG {
    /**
     * A HUD widget is anything that is thrown on top of the main canvas container
     */
    export class HudWidget {
        canvas: JQuery;
        container: JQuery;

        constructor(canvas: JQuery, templatePath: string) {
            this.canvas = canvas;
            var that = this;

            // We can instance our template path here to generate our DOM element           
            $.get(templatePath, data => {
                that.container = $(data);
                $(canvas).append(that.container);

                angular.element(document).injector().invoke($compile => {
                    var container = that.container;
                    var scope = angular.element(container).scope();
                    $compile(container)(scope);
                    scope.$apply();
                });
                that.onLoaded();
            });
        }

        /**
         * This method is called when loaded
         */
        onLoaded() {
        }

        show() {
            this.container.show();
        }

        hide() {
            this.container.hide();
        }
    }
    /*
     * A small widget that is bound to the top left of the window space that displays information regarding character status.
     */
    export class CharacterStatusWidget extends HudWidget {
        private playerInfo: PlayerInfo;

        onLoaded() {
        }

        constructor(canvas: JQuery, player: PlayerInfo) {
            super(canvas, "assets/templates/widgets/character_status.html");
        }
    }
    export class BottombarWidget extends HudWidget {
        constructor(canvas: JQuery) {
            super(canvas, "assets/templates/widgets/bottom_bar.html");
        }
    }

    export class MenuTrayWidget extends HudWidget {
        private inventoryWindow: InventoryWindow;
        private characterWindow: CharacterWindow;
        private questListWindow: QuestListWindow;
        private settingsWindow : SettingsWindow;
        private playerInfo: PlayerInfo;

        constructor(canvas: JQuery, playerInfo: PlayerInfo) {
            super(canvas, "assets/templates/widgets/menu_tray.html");
            this.playerInfo = playerInfo;
        }

        onLoaded() {
            this.inventoryWindow = new InventoryWindow();
            this.characterWindow = new CharacterWindow(this.playerInfo);
            this.questListWindow = new QuestListWindow();
            this.settingsWindow = new SettingsWindow();
            var that = this;

            // A few events quickly to bind our menu items
            this.container.find(".menu-item-backpack").on("click", () => {
                that.inventoryWindow.toggleVisibility();
            });

            this.container.find(".menu-item-equip").on("click", () => {
                that.characterWindow.toggleVisibility();
            });

            this.container.find(".menu-item-achievements").on("click", () => {
                that.questListWindow.toggleVisibility();
            });

            this.container.find(".menu-item-settings").on("click", () => {
                that.settingsWindow.toggleVisibility();
            });

            this.container.find(".menu-item-skills").on("click", () => {
                var q: SkillWindow;
                q = new SkillWindow(this.playerInfo);
                q.toggleVisibility();
            });
        }
    }

    export class LoginPanelWidget extends HudWidget {
        constructor(canvas: JQuery) {
            super(canvas, "assets/templates/widgets/login_panel.html");
        }

        onLoaded() {
            var that = this;

            var tat = new UI.Element(null, "#login-panel");

            $("#remember-user").on("click", function (eventObject: JQueryEventObject, ...args: any[]) {
                that.refreshCheckboxes(true, false);
            });

            $("#remember-pass").on("click", function (eventObject: JQueryEventObject, ...args: any[]) {
                that.refreshCheckboxes(false, true);
            });

            this.refreshCheckboxes();
        }

        private refreshCheckboxes(toggleUser: boolean = false, togglePass: boolean = false) {
            if (toggleUser) {
                Settings.getInstance().saveUsername = !Settings.getInstance().saveUsername;
            }

            if (togglePass) {
                Settings.getInstance().savePassword = !Settings.getInstance().savePassword;
            }

            Settings.getInstance().savePassword = Settings.getInstance().saveUsername && Settings.getInstance().savePassword;

            Settings.getInstance().flush();
            Settings.getInstance().save();

            var rememberUser = $("#remember-user");
            if (Settings.getInstance().saveUsername) {
                console.log("yes user");
                rememberUser.attr("checked", "true");
            } else {
                console.log("not user");
                rememberUser.removeAttr("checked");
            }

            var rememberPass = $("#remember-pass");
            if (Settings.getInstance().savePassword) {
                console.log("yes pass");
                rememberPass.attr("checked", "true");
            } else {
                console.log("not pass");
                rememberPass.removeAttr("checked");
            }
        }
    }

    export class ChatWidget extends HudWidget {
        constructor(canvas: JQuery) {
            super(canvas, "assets/templates/widgets/chat.html");
        }

        onLoaded() {
            // Do some basic key bindings
            $(document).on('keypress', (event: JQueryEventObject) => {
                if (document.activeElement) {
                    var x: any = document.activeElement;
                    var id = x.id;

                    if (event.which == 13) {
                        if (id == "chatmessage") {
                            Logger.trace("ChatWidget - Focused game");
                            $("#canvasholder").focus();
                        } else {
                            Logger.trace("ChatWidget - Focused chat");
                            $("#chatmessage").focus();
                        }
                    }
                }
            });
        }
    }
}