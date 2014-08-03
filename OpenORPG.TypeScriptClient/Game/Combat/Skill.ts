module OpenORPG {
    
    export class Skill {
        
        public skillId: number;
        public cooldownTime: number;
        public cooldownLeft : number;
        public template : Object;

        constructor(skill : any) {
            
            // Extract out what we need from our skills
            this.cooldownTime = skill.cooldownTime;
            this.cooldownLeft = skill.cooldown;


        }


    }

}