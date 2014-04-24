module DirectoryHelper {

    // The base asset path
    var baseAssetPath = "assets/";

    export function getSpritePath() {
        return baseAssetPath + "sprites/";
    }

    export function getAudioPath() {
        return baseAssetPath + "audio/";
    }
} 