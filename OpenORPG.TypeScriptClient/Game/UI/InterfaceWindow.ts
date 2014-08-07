module OpenORPG {

    /*
     *  An interface window inside of a game
     */
    export class InterfaceWindow {

        public windowName: string;
        public $window: Object;

        constructor(windowFile: string, windowName: string) {

            this.$window = $(windowName).dialog(
                {
                    autoOpen: true,
                    resizable: false,
                    modal: false,
                    width: 'auto',
                    open: function () { $(this).parent().css("padding", "0px"); $(this).parent().css("background", "transparent"); }



                });



            var that = this;
            // Get jQuery to load the static content into the page
            $.get(windowFile, data => {
                $(windowName).html(data);
                this.ready();
                that.toggleVisibility();

                angular.element(document).injector().invoke(function ($compile) {
                    var container = $(that.windowName);
                    var scope = angular.element(container).scope();
                    $compile(container)(scope);
                    scope.$apply();
                });



            });

            this.windowName = windowName;


        }


        // Close this interface window
        toggleVisibility() {
            ($(this.$window).dialog("isOpen") == false) ? $(this.$window).dialog("open") : $(this.$window).dialog("close");

        }

        close() {
            $(this.$window).dialog("close");
        }


        open() {
          $(this.$window).dialog("open");
        }



        ready() {




        }

    }

    export class QuestListWindow extends InterfaceWindow {

        constructor() {
            super("assets/hud/quest_list.html", "#quest-list-dialog");
        }

    }

    /*
     * A window that is used for displaying quest related stuff
     */
    export class QuestWindow extends InterfaceWindow {
        private id: number;

        constructor() {
            super("assets/hud/quest.html", "#quest-dialog");

        }

        presentQuest(questId: number) {
            this.id = questId;
            this.render();
            this.open();
        }

        ready() {     
        }

        /*
         * We stick with jQuery here for legacy reasons. No need to change stuff that isn't broken.
         */
        render() {
            $(this.windowName).prev().hide();

            // Load the quest info and get ready
            $.getJSON("assets/gamesfiles/quests/" + this.id + ".json", (data) => {

                $(this.windowName).find("#description").text(data.description);
                $(this.windowName).find(".quest-header").text(data.name);

            });

            var that = this;

            $(this.windowName).find("#accept-button").click(() => {
                var packet = PacketFactory.createQuestAcceptRequest(that.id);
                NetworkManager.getInstance().sendPacket(packet);
                that.close();
            });

            $(this.windowName).find("#decline-button").click(() => {
                that.close();
            });

        }


    }

    export class SkillWindow extends InterfaceWindow {

        private playerInfo: PlayerInfo;

        constructor(playerInfo: PlayerInfo) {

            super("assets/hud/skills.html", "#skill-dialog");
            this.playerInfo = playerInfo;

        }


        ready() {

        }

    }

    export class CharacterWindow extends InterfaceWindow {

        equipmentBindings = [];

        playerInfo: PlayerInfo;


        bindEquipmentSlots() {
            this.equipmentBindings[EquipmentSlot.Weapon] = (".weaponslot");
            this.equipmentBindings[EquipmentSlot.Head] = (".headslot");
            this.equipmentBindings[EquipmentSlot.Hands] = (".handsslot");
            this.equipmentBindings[EquipmentSlot.Feet] = (".feetslot");
            this.equipmentBindings[EquipmentSlot.Body] = (".bodyslot");
            this.equipmentBindings[EquipmentSlot.Back] = (".backslot");
        }

        /*
         * This function is reponsible for rendering character statistics onto the form.
         */
        renderStats() {


            // Render info
            var selector = $(this.windowName).find("#statspanel").selector;

            $(selector).find('#charactername').text(this.playerInfo.name);

            // Remove old stat stuff
            $(".statrow").remove();


            var names: string[] = [];
            for (var n in StatTypes) {
                if (typeof StatTypes[n] === 'number') names.push(n);
            }


            for (var key in this.playerInfo.characterStats) {
                var value = this.playerInfo.characterStats[key];

                var content = $('<div class="statrow"></div>');
                if (value.maximumValue > 0)
                    $(content).html(names[key] + ': <b class="statnumber">' + value.currentValue + '/' + value.maximumValue + '</b>');
                else
                    $(content).html(names[key] + ': <b class="statnumber">' + value.currentValue + '</b>');

                $(selector).append(content);

            }





        }

        constructor(playerInfo: PlayerInfo) {

            super("assets/hud/character.html", "#characterdialog");

            this.bindEquipmentSlots();
            this.playerInfo = playerInfo;

            // Setup a binding to change on character state change
            //this.playerInfo.listenCharacterStatChange($.proxy(this.renderStats, this));



            NetworkManager.getInstance().registerPacket(OpCode.SMSG_EQUIPMENT_UPDATE, (packet) => {

                // Update all the things
                var slot: EquipmentSlot = packet.slot;
                var equipment: any = packet.equipment;

                // Update the slot with the item you need
                var domSlot = this.equipmentBindings[slot];
                $(domSlot).empty();

                if (equipment != null) {

                    var item = $("<div class='equipitem'></div>");
                    var image = GraphicsUtil.getIconCssFromId(equipment.iconId);
                    $(item).css('background', image);

                    $(domSlot).append(item);
                    $(item).attr("slot", slot);

                    var menu = new EquipmentItemContextMenu(item.parent()[0]);

                    var tooltip: ItemTooltipInfo = new ItemTooltipInfo($(domSlot)[0], equipment);

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
                var item = $("<div class='item'> <div class='itemtext'/>  </div>");

                var gameItem = inventory.storage[slotId];
                $('[slotId="' + slotId + '"]').append(item);

                var image = GraphicsUtil.getIconCssFromId(gameItem.item.iconId);
                $(item).css('background', image);

                item.children().first().text(gameItem.amount);

                // Set title
                // $(item).attr("title", "Name: " + gameItem.item.name + " | Description: " + gameItem.item.description);

                var tooltip: ItemTooltipInfo = new ItemTooltipInfo($('[slotId="' + slotId + '"]')[0], gameItem.item);

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