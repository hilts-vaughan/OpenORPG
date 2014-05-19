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

    export class CharacterWindow extends InterfaceWindow {

        equipmentBindings = [];


        bindEquipmentSlots() {
            this.equipmentBindings[EquipmentSlot.Weapon] = (".weaponslot");
            this.equipmentBindings[EquipmentSlot.Head] = (".headslot");
            this.equipmentBindings[EquipmentSlot.Hands] = (".handsslot");
            this.equipmentBindings[EquipmentSlot.Feet] = (".feetslot");
            this.equipmentBindings[EquipmentSlot.Body] = (".bodyslot");
            this.equipmentBindings[EquipmentSlot.Back] = (".backslot");
        }


        constructor() {

            super("assets/hud/character.html", "#characterdialog");

            this.bindEquipmentSlots();

            NetworkManager.getInstance().registerPacket(OpCode.SMSG_EQUIPMENT_UPDATE, (packet) => {

                // Update all the things
                var slot: EquipmentSlot = packet.slot;
                var equipment: any = packet.equipment;

                // Update the slot with the item you need
                var domSlot = this.equipmentBindings[slot];
                $(domSlot).empty();

                if (equipment != null) {

                    var item = $("<div class='equipitem'></div>");
                    var image = GraphicsUtil.itemIdToImagePath(equipment.id);
                    $(item).css('background-image', 'url(' + image + ')');

                    $(domSlot).append(item);
             

                }

            });
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

                var image = GraphicsUtil.itemIdToImagePath(gameItem.item.id);
                $(item).css('background-image', 'url(' + image + ')');

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