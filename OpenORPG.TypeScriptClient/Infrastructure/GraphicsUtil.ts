module GraphicsUtil {


    export function itemIdToImagePath(itemId: number) {
        return DirectoryHelper.getItemsPath() + itemId + ".png";
    }

} 