﻿module OpenORPG {

    /*
     * A simple container class that has the job of simply containing amounts of information.
     * A simple flyweight class that can be passed around to tie in various pieces of info and
     * prevent duplication. Contains information like 
     * 
     */
    export class PlayerInfo {

        public name : string;

        // A small interface to character stats
        public characterStats: Array<CharacterStat> = new Array<CharacterStat>();
        private characterStatsCallbacks: Array<Function> = new Array<Function>();


        public inventory: Array<Item> = new Array<Item>();
        private inventoryCallbacks = [];

        constructor() {

        }


        listenCharacterStatChange(callback: Function) {
            this.characterStatsCallbacks.push(callback);
        }


        onCharacterStatChange() {
            this.characterStatsCallbacks.forEach(this.callCallback);
        }




        callCallback(element: Function) {
            element();
        }




    }
}