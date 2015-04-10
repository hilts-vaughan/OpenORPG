/**
 * Provides a namespace
 */
module OpenORPG {

    class InterpolatorState {

        public _x: number;
        public _y: number;
        public _dir: Direction;

        constructor(x: number, y: number, dir: Direction) {
            this._x = x;
            this._y = y;
            this._dir = dir;
        }

    }

    export class EntityInterpolator {

        private _states: InterpolatorState[] = new Array<InterpolatorState>();

        /**
         * Provides a value of backwards time
         */
        private _backTime: number = 100;

        /**
         * A maximum amount of packets that can be queued up before being forced to releases
         */
        private _maxDelay: number = 200;

        private _threshold: number = 1;

        private _entity: Entity;

        private _currentMovementTween: Phaser.Tween;

        constructor(entity: Entity) {
            this._entity = entity;
        }

        /**
         * Given some data from the network, adds it to the state to be interpolated with
         */
        public addData(x: number, y: number, dir: Direction) {

            // Add the state the data we have available to us
            var state: InterpolatorState = new InterpolatorState(x, y, dir);
            this._states.push(state);

            if (this._states.length > this._maxDelay) {
                this.forceFlush();
            }

            this.update();
        }

        public render() {

            // Render the shapes as required
            this._states.forEach( (state : InterpolatorState) => {
                this._entity.game.debug.geom(new Phaser.Circle(state._x, state._y, 10));
            });

        }

        public update() {

            // If we're done interpolation, look for more work
            if (this._currentMovementTween == null || !this._currentMovementTween.isRunning) {

                // If there's work available..
                if (this._states.length > 0) {

                    if (this._currentMovementTween != null) {
                        this._currentMovementTween.stop();
                        this._currentMovementTween = null;
                    }

                    // Get the state to use
                    var state: InterpolatorState;
                    state = this._states.shift();

                    var tweenData = {
                        x: state._x,
                        y: state._y
                    }



                    var point: Phaser.Point = new Phaser.Point(tweenData.x, tweenData.y);
                    var point2: Phaser.Point = new Phaser.Point(this._entity.x, this._entity.y);

                    this._entity.direction = state._dir;

                    // If the distance is small, a teleport is in order and we continue on. This prevents small little hiccups
                    if (Phaser.Point.distance(point, point2) < this._threshold) {
                        this._entity.x = tweenData.x;
                        this._entity.y = tweenData.y;
                        Logger.debug("Left early...");
                        return;
                    }

                    Logger.debug("Starting to kick off an interpolation...");

                    // Kick off a new tween for the data
                    this._currentMovementTween = this._entity.game.add.tween(this._entity).to(tweenData, this._backTime, Phaser.Easing.Linear.None, true);

                    // Add tween completion callback
                    this._currentMovementTween.onComplete.addOnce(() => {
                        this._currentMovementTween.stop();
                        this._currentMovementTween = null;
                        this.update();
                    });

                } else {

                    if (this._currentMovementTween != null) {
                        this._currentMovementTween.stop();
                    }

                    this._currentMovementTween = null;
                }


            }
        }

        public isIdle(): boolean {
            if (this._currentMovementTween == null)
                return true;
            return false;
        }

        private forceFlush() {
            debugger;

            if (this._currentMovementTween != null) {
                this._currentMovementTween.stop();
                this._currentMovementTween = null;
            }

            var lastState: InterpolatorState = this._states[this._states.length - 1];

            this._entity.x = lastState._x;
            this._entity.y = lastState._y;
            this._entity.direction = lastState._dir;

            Logger.warn("A force flush was performed; looks like the network is falling behind.");

            // Remove all elements
            while (this._states.length) {
                this._states.pop();
            }

        }

    }

} 