var FontFactory;
(function (FontFactory) {
    // Returns a basic, usable font for the game
    function getBasicFont() {
        return null;
    }
    FontFactory.getBasicFont = getBasicFont;

    // Returns a font for a specific player
    function getPlayerFont() {
        var font = new OpenORPG.FontDefinition("12px Georgia", "#ffff00", "center");
        return font;
    }
    FontFactory.getPlayerFont = getPlayerFont;
})(FontFactory || (FontFactory = {}));
//# sourceMappingURL=FontFactory.js.map
