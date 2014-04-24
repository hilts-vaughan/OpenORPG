module OpenORPG {
    
    // A zone trigger is used to fire off events when they are entered
    export class ZoneTrigger {

        private game: Phaser.Game;
        private x: number;
        private y: number;
        private width: number;
        private height: number;
        private callback: () => void;
        private zone: Zone;

        private triggered: boolean = false;

        constructor(game: Phaser.Game, x: number, y: number, width: number, height: number, callback: () => void, zone : Zone) {
            this.game = game;
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
            this.callback = callback;
            this.zone = zone;
        }

        public update(entity : Entity) {

            if (this.triggered)
                return;

            var rectangle = new Phaser.Rectangle(this.x, this.y, this.width, this.height);
            if(Phaser.Rectangle.intersects(rectangle, entity.body)) {
                this.triggered = true;
                this.callback();
            }

        }


    }

}