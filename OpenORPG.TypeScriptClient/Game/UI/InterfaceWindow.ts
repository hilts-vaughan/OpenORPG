module OpenORPG {

    /*
     *  An interface window inside of a game
     */
    export class InterfaceWindow {

        public windowName: string;

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
                this.ready();
            });

            this.windowName = windowName;


        }


        // Close this interface window
        close() {
            $(this.windowName).dialog("close");
        }

        ready() {

        }

    }


    export class InventoryWindow extends InterfaceWindow {

        // Create our inventory window
        constructor() {
            super("assets/hud/inventory.html", "#inventorydialog");


        }

        ready() {

            // Setup drag events
            $(".item").draggable({ revert: 'invalid' });

            $('.itemslot').droppable({
                accept: '.item',

                drop: function (ev, ui) {
                    var dropped = ui.draggable;
                    var droppedOn = $(this);
                    $(dropped).detach().css({ top: 0, left: 0 }).appendTo(droppedOn);
                }

            });

            $(this.windowName).tooltip({
                position: {
                    my: "center bottom-20",
                    at: "center top",
                    using: function (position, feedback) {
                        $(this).css(position);
                        $("<div>")
                            .addClass("arrow")
                            .addClass(feedback.vertical)
                            .addClass(feedback.horizontal)
                            .addClass("inventorytooltip")
                            .appendTo(this);
                    }
                }
            });



        }

        itemInSpot(drag_item, spot) {
            var item = $('<div />'); // create new img element
            item.attr({ // copy attributes
                src: drag_item.attr('src'),
            }).attr('class', drag_item.attr('class')).appendTo(spot).draggable({ revert: 'invalid' }); // add to spot + make draggable
            drag_item.remove(); // remove the old object
        }


    }




} 