module OpenORPG {

    /*
     * A representation of a skill on the client side. Contains properties that the client might need to render and view
     * information about.
     */
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

        get description(): string {
            return this.template.description;
        }


        get type(): string {
            return this.template.type;
        }

        get castTime(): number {
            return this.template.castTime;
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
         * Resets the given cooldown timer to the alloted value on the internal representation.
         * This should only be called once the client has acknowledge a server side order. Otherwise,
         * the server will reject the skill request.
         */
        resetCooldown() {
            this.cooldown = this.cooldownTime;
            Logger.info("Cooldown for skill " + this.id + " has been reset to " + this.cooldown);
        }

    }

}