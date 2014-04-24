var DirectoryHelper;
(function (DirectoryHelper) {
    // The base asset path
    var baseAssetPath = "/assets/";

    function getSpritePath() {
        return baseAssetPath + "sprites/";
    }
    DirectoryHelper.getSpritePath = getSpritePath;

    function getAudioPath() {
        return baseAssetPath + "audio/";
    }
    DirectoryHelper.getAudioPath = getAudioPath;
})(DirectoryHelper || (DirectoryHelper = {}));
//# sourceMappingURL=DirectoryHelper.js.map
