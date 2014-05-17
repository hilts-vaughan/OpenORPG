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
                            alert("Dropping " + ui.target.text());
                        }
                    }

                ]
            });


            // End of event construction here
        }




    }


}