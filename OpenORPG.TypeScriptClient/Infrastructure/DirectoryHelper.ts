module DirectoryHelper {

    // The base asset path
    var baseAssetPath = "assets/";

    export function getSpritePath() {
        return baseAssetPath + "sprites/";
    }

    export function getMusicPath() {
        return baseAssetPath + "audio/music/";
    }

    export function getAudioEffectPath() {
        return baseAssetPath + "audio/sounds/";
    }

    export function getItemsPath() {
        return baseAssetPath + "items/";
    }

} 