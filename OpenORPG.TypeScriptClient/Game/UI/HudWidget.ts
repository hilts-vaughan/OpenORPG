module OpenORPG {

    /**
     * A HUD widget is anything that is thrown on top of the main canvas container
     */
    export class HudWidget {

        canvas: JQuery;
        container : JQuery;
        
        constructor(canvas: JQuery, templatePath: string) {
            this.canvas = canvas;

 
            // We can instance our template path here to generate our DOM element           
            $.get(templatePath, data => {
                this.container = $(data);
                $(canvas).append(this.container);
                this.onLoaded();
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
        
        constructor(canvas : JQuery) {

            super(canvas, "assets/templates/widgets/character_status.html");
        }

    }

    export class BottombarWidget extends HudWidget {
        
        constructor(canvas: JQuery) {
            super(canvas, "assets/templates/widgets/bottom_bar.html");
        }

    }


    export class ChatWidget extends HudWidget {

        private chatManager : ChatManager;

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