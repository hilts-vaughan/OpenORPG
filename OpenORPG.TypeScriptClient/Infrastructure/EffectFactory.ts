module EffectFactory {

    export function fadeEntityIn(entity: OpenORPG.Entity) {
        // Set the alpha to 0 to begin the fade in
        entity.alpha = 0;

        var properties =  {
            alpha: 1
        };
        return entity.game.add.tween(entity).to(properties, 750, Phaser.Easing.Quadratic.In, true);
    }

    // This effects causes an object to float and fade away
    export function floatAndFadeAway(game : Phaser.Game, gameObject: PIXI.Sprite) {
        var properties = {
            y: gameObject.y - 32,
            alpha: 0
        }

       return game.add.tween(gameObject).to(properties, 1000, Phaser.Easing.Linear.None, true);
    }

    export function pulseDamage(entity: OpenORPG.Entity) {
        var properties = {
            tint: 0x7E3517,
            alpha: 0.7
        };

        // Pulse the damage output
        return entity.game.add.tween(entity).to(properties, 250, Phaser.Easing.Linear.None, true, 0, 1 , true);
    }


} 