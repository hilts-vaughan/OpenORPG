module OpenORPG {

    export class Skill {

        get id(): number {
            return this.template.id;
        }

        get cooldownTime(): number {
            return this.template.cooldownTime;
        }

        get name(): string {
            return this.template.name;
        }

        get iconId(): number {
            return this.template.iconId;
        }

        public cooldown: number;


        /*
         * The internal template we're using for this skill
         */
        private template: any;

        constructor(skillTemplate: any) {
            this.template = skillTemplate;
            this.cooldown = skillTemplate.cooldown;
        }

        /*
         * Resets the given cooldown timer to the alloted value.
         */
        resetCooldown() {
            this.cooldown = this.cooldownTime;
            Logger.info("Cooldown for skill " + this.id + " has been reset to " + this.cooldown);
        }

    }

}