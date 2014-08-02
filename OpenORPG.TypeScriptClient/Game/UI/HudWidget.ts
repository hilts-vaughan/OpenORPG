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
                that.onLoaded();
            });

        }

        /**
         * This method is called when loaded
         */
        onLoaded() {

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


            // Listen for the stat change event so that we can refresh the HUD
            this.playerInfo = player;
            this.playerInfo.listenCharacterStatChange(() => {


                var hpContainer = this.container.find("#health-vital-bar .vital-bar-fill");
                var mpContainer = this.container.find("#mana-vital-bar .vital-bar-fill");

                hpContainer.text(LocaleManager.getInstance().getString("Hitpoints"));
                mpContainer.text(LocaleManager.getInstance().getString("Manapoints"));

                var hp = this.playerInfo.characterStats[StatTypes.Hitpoints];
                var mp = this.playerInfo.characterStats[StatTypes.Intelligence];

                var hpPercent = (hp.currentValue / hp.maximumValue) * 100;
                var mpPercent = (mp.currentValue / mp.maximumValue) * 100;

                    hpContainer.animate({
                        "width": hpPercent + "%"
                    }, 500);

                mpContainer.animate({
                    "width": mpPercent + "%"
                }, 500);


                this.container.find("#health-vital-bar .float-vital").text(hp.currentValue + "/" + hp.maximumValue);
                this.container.find("#mana-vital-bar .float-vital").text(mp.currentValue + "/" + mp.maximumValue);

                this.container.find(".label-left").text(this.playerInfo.name);


            }
                );


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
        private playerInfo : PlayerInfo;

        constructor(canvas: JQuery, playerInfo : PlayerInfo) {
            super(canvas, "assets/templates/widgets/menu_tray.html");
            this.playerInfo = playerInfo;
        }

        onLoaded() {
            this.inventoryWindow = new InventoryWindow();
            this.characterWindow = new CharacterWindow(this.playerInfo);

            var that = this;
            this.playerInfo.listenCharacterStatChange(() => {

                that.characterWindow.renderStats();

            });

            // A few events quickly to bind our menu items
            this.container.find(".menu-item-backpack").on("click", () => {
                that.inventoryWindow.toggleVisibility();
            });

            this.container.find(".menu-item-equip").on("click", () => {
                that.characterWindow.toggleVisibility();
            });

            this.container.find(".menu-item-skills").on("click", () => {
                var q: SkillWindow;
                q = new SkillWindow(this.playerInfo);
                q.toggleVisibility();
            });


        }

    }


    export class ChatWidget extends HudWidget {

        private chatManager: ChatManager;

        constructor(canvas: JQuery) {
            super(canvas, "assets/templates/widgets/chat.html");
        }

        onLoaded() {

            this.chatManager = new ChatManager();

            // Do some basic key bindings

            $(document).on('keypress', (event: JQueryEventObject) => {

                if (document.activeElement) {
                    var x: any = document.activeElement;
                    var id = x.id;

                    if (event.which == 13) {

                        if (id == "chatmessage") {
                            console.log('focus game');
                            $("#canvasholder").focus();
                        } else {
                            console.log('focus chat');
                            $("#chatmessage").focus();
                        }
                    }
                }


            });



        }

    }



}