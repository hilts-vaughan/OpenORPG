/// <reference path="../../phaser.d.ts" />

module OpenORPG {
    export class AbstractState extends Phaser.State {
        get centerX(): number {
            return this.game.scale.bounds.centerX;
        }

        get centerY(): number {
            return this.game.scale.bounds.centerY;
        }


    }
}