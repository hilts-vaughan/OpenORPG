module SpriteManager {

    export function loadSpriteInfo(game: Phaser.Game) {
        game.load.json("spritepack", DirectoryHelper.getSpritePath() + "sprites.json");
    }

    export function loadSpriteDefintions(game: Phaser.Game) {
        var spritePackFromCache = game.cache.getJSON("spritepack");

        for (var key in spritePackFromCache) {
            // Get the value
            var value: string = spritePackFromCache[key];

            // Now, perform the actual load
            game.load.json("spritedef_" + key, DirectoryHelper.getSpritePath() + value + ".json");
        }
    }

    export function loadSpriteImages(game: Phaser.Game) {
        var spritePackFromCache = game.cache.getJSON("spritepack");

        for (var key in spritePackFromCache) {
            var spriteDef : any = game.cache.getJSON("spritedef_" + key);
            var value: string = spritePackFromCache[key];
            game.load.spritesheet(key, DirectoryHelper.getSpritePath() + value + ".png", spriteDef.width, spriteDef.height);
        }
    }


}