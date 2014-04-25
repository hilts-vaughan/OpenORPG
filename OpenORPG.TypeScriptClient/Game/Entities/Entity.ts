module OpenORPG {

    export class Entity extends Phaser.Sprite {

        // These properties are what are strongly typed
        public name: string;
        public id: number;

        public characterState: CharacterState;
        public direction: Direction;

        // Our name tag header if required
        private nameTagText: Phaser.Text = null;

        private skillAnimation: Phaser.Animation = null;


        constructor(game: Phaser.Game, x: number, y: number) {
            super(game, x, y);

            this.anchor.setTo(0, 0);

            // Do something with this entity
            this.game.physics.enable(this, Phaser.Physics.ARCADE, true);
            this.body.collideWorldBounds = true;

            // Disable smoothing on the sprite
            this.smoothed = false;



        }

        update() {

            if (this.nameTagText != null)
                this.nameTagText.position.setTo(this.x + this.texture.width / 2, this.y);

            var directionString = this.directionToString();

            // Use of a skill animate over-takes everything
            if (this.skillAnimation != null) {

                if (this.skillAnimation.isFinished)
                    this.skillAnimation = null;
                return;
            }

            switch (this.characterState) {
                case CharacterState.Idle:
                    this.playIdle(directionString);
                    break;
                case CharacterState.UsingSkill:
                    this.playReadyingSkill(directionString);
                    break;
                case CharacterState.Moving:
                    this.playWalk(directionString);
                    break;
            }
        }

        private playIdle(direction: string) {
            this.animations.play("idle_" + direction);
        }

        private playReadyingSkill(direction: string) {
            this.animations.play("atk_" + direction);
        }

        private playWalk(direction: string) {
            this.animations.play("walk_" + direction);
        }


        public playSkillAnimation() {
            var direction = this.directionToString();
            this.skillAnimation = this.animations.play("atk_" + direction, 12, false);
        }

        render() {
            this.game.debug.body(this);
        }


        private directionToString(): string {

            switch (this.direction) {
                case Direction.North:
                    return "up";
                case Direction.South:
                    return "down";
                case Direction.East:
                    return "right";
                case Direction.West:
                    return "left";
            }
        }

        public propertyChanged(name: string, value: any) {

            switch (name) {
                case "name":
                    this.updateName(value);
                    break;
                case "sprite":
                    this.updateSprite(value);
                    break;
            }


        }

        private updateName(name: string) {

            if (this.nameTagText == null) {
                this.nameTagText = new Phaser.Text(this.game, 0, 0, name, FontFactory.getPlayerFont());
                this.nameTagText.anchor.set(0.5, 0.5);
                this.game.world.add(this.nameTagText);
            }
            else {
                this.nameTagText.text = name;
            }
        }

        private updateSprite(sprite: string) {

            var textureId = "entity_sprite_" + sprite;
            var image: any = this.game.cache.getImage(textureId);
            this.loadTexture(textureId, 0);

            var spriteDefinition: any = this.game.cache.getJSON("spritedef_" + textureId);
            var rowFrames = image.width / spriteDefinition.width;

            for (var animKey in spriteDefinition.animations) {
                var anim = spriteDefinition.animations[animKey];
                var index = rowFrames * anim.row;
                var frames = []

                for (var i = 0; i < anim.length; i++) {
                    frames.push(index + i);
                }

                this.animations.add(animKey, frames, 4, true, true);
            }

            // Finished updating sprite
            this.animations.play("idle_down");

            // Setup the body for this sprite
            this.body.setSize(32, 32, this.texture.width / 2 - 16, this.texture.height - 32);
        }

        /* This is used for network transmission only, it can be dangerous if used
        without a second thought. For the most part, only let the networking code
        invoke this and do not use on any other methods that could be harmful.
        */
        public mergeWith(object: any) {
            // Extends the objects with jQuery
            $.extend(this, object);
        }


    }

}