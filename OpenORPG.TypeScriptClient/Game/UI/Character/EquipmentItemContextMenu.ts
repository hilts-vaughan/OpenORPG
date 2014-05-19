module OpenORPG {

    export class EquipmentItemContextMenu {


        /*
         * The container elements are intialized here
         */
        constructor(container: HTMLElement) {

            var jquery: any = $;
            jquery(container).contextmenu(
                {


                    delegate: ".equipitem",
                    menu: [
                        {
                            title: "Remove",
                            uiIcon: "",
                            action: (event, ui) => {

                                if ($(ui.target).hasClass("itemtext")) {
                                    ui.target = $(ui.target).parent();
                                }

                                var id: number = parseInt($(ui.target).attr("slot"));
                                var request = PacketFactory.createUnEqupRequest(id);

                                NetworkManager.getInstance().sendPacket(request);
                            }
                        }

                    ]
                });


            // End of event construction here
        }






    }


} 