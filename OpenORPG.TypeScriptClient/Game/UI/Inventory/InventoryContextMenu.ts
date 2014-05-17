module OpenORPG {

    export class InventoryContextMenu {


        /*
         * The container elements are intialized here
         */
        constructor(container: HTMLElement) {

            var jquery: any = $;
            jquery(container).contextmenu(
                {
            

                delegate: ".item",
                menu: [
                    {
                        title: "Use",
                        uiIcon: "",
                        action: (event, ui) => {
                            alert("Using " + ui.target.text());
                        }
                    },

                    {
                        title: "Drop",
                        uiIcon: "",
                        action: (event, ui) => {

                            if ($(ui.target).hasClass("itemtext")) {
                                ui.target = $(ui.target).parent();
                            }

                            var id: number = parseInt($(ui.target).parent().attr("slotId"));
                            var request = PacketFactory.createStorageDropRequest(id, 1);

                            NetworkManager.getInstance().sendPacket(request);
                        }
                    }

                ]
            });


            // End of event construction here
        }






    }


}