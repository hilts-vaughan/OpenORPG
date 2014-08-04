module GraphicsUtil {


    export function itemIdToImagePath(itemId: number) {
        return DirectoryHelper.getItemsPath() + itemId + ".png";
    }

    export function getIconCssFromId(iconId: number) : string {
        var width = 1156;         
        var iconSize = 34;
        var widthIcons = width / iconSize;

        var x: number = (iconId % widthIcons) * iconSize;
        var y: number = Math.floor(iconId / widthIcons) *iconSize;

        var s: string = "url(assets/iconsheet.png) -{0}px -{1}px";
        s = s.replace("{0}", x.toString());
        s = s.replace("{1}", y.toString());

        return s;
    }

} 