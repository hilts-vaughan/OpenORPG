var SpriteManager;
(function (SpriteManager) {
    function loadSpriteInfo(game) {
        game.load.json("spritepack", DirectoryHelper.getSpritePath() + "sprites.json");
    }
    SpriteManager.loadSpriteInfo = loadSpriteInfo;

    function loadSpriteDefintions(game) {
        var spritePackFromCache = game.cache.getJSON("spritepack");

        for (var key in spritePackFromCache) {
            // Get the value
            var value = spritePackFromCache[key];

            // Now, perform the actual load
            game.load.json("spritedef_" + key, DirectoryHelper.getSpritePath() + value + ".json");
        }
    }
    SpriteManager.loadSpriteDefintions = loadSpriteDefintions;

    function loadSpriteImages(game) {
        var spritePackFromCache = game.cache.getJSON("spritepack");

        for (var key in spritePackFromCache) {
            var spriteDef = game.cache.getJSON("spritedef_" + key);
            var value = spritePackFromCache[key];
            game.load.spritesheet(key, DirectoryHelper.getSpritePath() + value + ".png", spriteDef.width, spriteDef.height);
        }
    }
    SpriteManager.loadSpriteImages = loadSpriteImages;
})(SpriteManager || (SpriteManager = {}));
//# sourceMappingURL=SpriteManager.js.map
