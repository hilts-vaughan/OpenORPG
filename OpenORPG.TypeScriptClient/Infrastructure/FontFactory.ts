module FontFactory {

    // Returns a basic, usable font for the game
    export function getBasicFont(): OpenORPG.FontDefinition {
        return null;
    }

    // Returns a font for a specific player
    export function getPlayerFont(): OpenORPG.FontDefinition {
        var font = new OpenORPG.FontDefinition("14px AdvoCut", "#ffff00", "center", "#000000", 3);
        return font;
    }

    export function getDamageFont(): OpenORPG.FontDefinition {
        return new OpenORPG.FontDefinition("26px AdvoCut", "#FFFFFF", "center", "#000000", 4);
    }

}