module OpenORPG {

    export class DamageText extends Phaser.Text {
        constructor(entity: Entity, damage: number) {
            var style = FontFactory.getDamageFont();
            if (damage < 0) {
                damage *= -1;
                style.fill = "#00CC00";
            }           
            // Be sure to call Phaser constructor properly                        
            super(entity.game, 0, 0, damage.toString(), style);

            this.anchor.set(0.5, 0.5);
            this.position.setTo(entity.width/2, -10);
            entity.addChild(this);

            var effect = EffectFactory.floatAndFadeAway(entity.game, this);

            // When the effect is finished, scrap this
            effect.onComplete.add(() => {
                this.destroy();
            }, this);
        }
    }
} 