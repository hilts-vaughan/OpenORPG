module OpenORPG {

    /*
     *  An interface window inside of a game
     */
    export class InterfaceWindow {

        private windowName: string;

        constructor(windowFile: string, windowName: string) {

            $(windowName).dialog(
                {
                    autoOpen: true,
                    resizable: false,
                    modal: false,
                    width: 'auto',

                    open: function () { $(this).parent().css("padding", "0px"); $(this).parent().css("background", "transparent"); }



                });

            // Get jQuery to load the static content into the page
            $.get(windowFile, data => {
                $(windowName).html(data);
            });

            this.windowName = windowName;


        }


        // Close this interface window
        close() {
            $(this.windowName).dialog("close");
        }



    }


    export class InventoryWindow extends InterfaceWindow {

        // Create our inventory window
        constructor() {
            super("assets/hud/inventory.html", "#inventorydialog");
        }

    }




} 