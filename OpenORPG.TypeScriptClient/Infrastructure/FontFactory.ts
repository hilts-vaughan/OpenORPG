module FontFactory {

    // Returns a basic, usable font for the game
    export function getBasicFont(): OpenORPG.FontDefinition {
        return null;
    }

    // Returns a font for a specific player
    export function getPlayerFont(): OpenORPG.FontDefinition {
        var font = new OpenORPG.FontDefinition("12px Georgia", "#ffff00", "center", "#000000", 3);
        return font;
    }

    export function getDamageFont(): OpenORPG.FontDefinition {
        return new OpenORPG.FontDefinition("22px Georgia", "##AA0114", "center", "#000000", 3);
    }

}