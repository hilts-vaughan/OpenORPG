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
        toggleVisibility() {
            $(this.windowName).toggle("clip", {}, 300);
        }

        ready() {

        }

    }


    export class InventoryWindow extends InterfaceWindow {

        // Create our inventory window
        constructor() {
            super("assets/hud/inventory.html", "#inventorydialog");

            // Hook into our network events
            NetworkManager.getInstance().registerPacket(OpCode.SMSG_STORAGE_HERO_SEND, (packet) => {
                // Do something about the inventory update
                console.log("Inventory update recieved");
                console.log(packet);
                this.renderInventory(packet.itemStorage);
            });

        }

        ready() {



        }

        renderInventory(inventory: any) {

            $("#itemback").empty();

            for (var i = 0; i < inventory.capacity; i++) {
                var $slot = $("<div class='itemslot'/>").attr("slotId", i);
                $("#itemback").append($slot);
            }

            for (var slotId in inventory.storage) {
                var item = $("<div class='item'> <p class='itemtext'/>  </div>");

                var gameItem = inventory.storage[slotId];
                $('[slotId="' + slotId + '"]').append(item);
                item.children().first().text(gameItem.amount);

                // Set title
                $(item).attr("title", "Name: " + gameItem.item.name + " | Description: " + gameItem.item.description);



            }


            // Setup drag events
            $(".item").draggable({ revert: 'invalid' });

            $('.itemslot').droppable({
                accept: '.item',

                drop: function (ev, ui) {

        

                    var dropped = ui.draggable;
                    var droppedOn = $(this);

                    //
                    var sourceSlotId: number = parseInt($(dropped).parent().attr("slotId"));
                    var destSlotId: number = parseInt($(droppedOn).attr("slotId"));



                    $(dropped).detach().css({ top: 0, left: 0 }).appendTo(droppedOn);
                    
            
                    var packet = PacketFactory.createStorageMoveRequest(sourceSlotId, destSlotId, 0);
                    NetworkManager.getInstance().sendPacket(packet);

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

            // Attach a context menu
            var menu: InventoryContextMenu = new InventoryContextMenu($("#itemback").get(0));


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