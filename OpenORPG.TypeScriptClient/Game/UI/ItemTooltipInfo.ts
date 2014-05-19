module OpenORPG {

    /*
     * This class is responsible for displaying tooltip info for items.
     * It will display some general information
     */
    export class ItemTooltipInfo {

        data: string;
        element: HTMLElement;
        gameItem: any;

        /*
         * Contstructs a new tooltip   
         */
        constructor(element: HTMLElement, gameItem: any) {

            this.element = element;
            this.gameItem = gameItem;

            $.get("assets/hud/itemtooltip.html", data => {
                this.data = data;
                this.ready();
            });

        }

        ready() {


            $(this.element).tooltip({

                position: {
                    my: "center top",
                    at: "center bottom",
                    using: function (position, feedback) {
                        $(this).css(position);
                        $("<div>")
                            .addClass("arrow")
                            .addClass(feedback.vertical)
                            .addClass(feedback.horizontal)
                            .appendTo(this);
                    }
                },


                items: '.item, .equipitem',
                content: () => {
                    var context = $(this.data);

                    var image = GraphicsUtil.itemIdToImagePath(this.gameItem.id);
                    $(context).find("#IconHolder").css('background-image', 'url(' + image + ')');
                    $(context).find("#ItemName").text(this.gameItem.name);
                    $(context).find(".ItemDesc").text(this.gameItem.description);
                    $(context).find("#ItemTypex").text(this.gameItem.type);

                    //ItemName


                    return context[2].outerHTML;
                }


            });

            // Do some markup stuff here now
            // IconHolder


        }



    }

}