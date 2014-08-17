module OpenORPG {

    export class DamageText extends Phaser.Text {

        constructor(entity: Entity, damage: number) {

            var style = FontFactory.getDamageFont();

            if (damage < 0) {
                damage *= -1;
                style.fill = "#00CC00";
            }


            // Setup the important things here
            super(entity.game, 0, 0, damage.toString(), style);

            this.anchor.set(0.5, 0.5);
            this.position.setTo(entity.width / 2, 0);
            entity.addChild(this);

            var effect = EffectFactory.floatAndFadeAway(entity.game, this);


            // When the effect is finished, scrap this
            effect.onComplete.add(() => {
                this.destroy();
            }, this);

        }

    }

} 