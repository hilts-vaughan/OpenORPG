module OpenORPG {

    /**
     * Stores a single state of interpolated state from an entity movement from the remote client
     * and or server. 
     */
    class InterpolatorState {
        public _x: number;
        public _y: number;
        public _dir: Direction;
        private _timestamp : number;
           
        /**
         * Returns the timestamp as of the UNIX epoch from when this state was created.          
         */
        public get timestamp() : number {
            return this._timestamp;
        }
                
        constructor(x: number, y: number, dir: Direction) {
            this._x = x;
            this._y = y;
            this._dir = dir;
            this._timestamp = new Date().getTime();
        }
    }
    
    /**
     * Provides services for interpolating a remote entity through various states fed into it.
     * The interpolator will automatically keep up with the states fed with it, it is only neccessary
     * to ensure update is called on it and the neccessary properties will be updated.
     */
    export class EntityInterpolator {

        private _states: InterpolatorState[] = new Array<InterpolatorState>();

        /**
         * Represents the amount of time a InterpolatorState will take to be tweened to by default.
         * This value is only the standard -- interpolator states may be consumed faster when running behind.
         */
        private _backTime: number = 100;

        /**
         * A maximum amount of packets that can be queued up before being forced to releases
           This is roughly 1 * backTime ms behind. 
        
           When set to 10, this is a full second behind (which is quite a lot) 
        */
        private _maxStateBacklog: number = 20;

        /**
         * If the distance between the current property value and the next InterpolatorState value is less than this value,
         * then no tween is performed and we snap to this value. This prevents cases where small changes cause jitter.
         */       
        private _cutoffThreshold: number = 1;


        private _entity: Entity;
        private _currentMovementTween: Phaser.Tween;

        constructor(entity: Entity) {
            this._entity = entity;
        }

        
        /**
         * Adds state data to this new EntityInterpolator from a remote network feed. 
         */
        public addData(x: number, y: number, dir: Direction) {

            // Add the state the data we have available to us
            var state: InterpolatorState = new InterpolatorState(x, y, dir);
            this._states.push(state);

            if (this._states.length > this._maxStateBacklog) {
                this.forceFlush();
            }

            this.update();
        }

        // This is only used in debug mode
        public render() {
            // Render the shapes as required
            this._states.forEach((state: InterpolatorState) => {
                this._entity.game.debug.geom(new Phaser.Circle(state._x, state._y, 10));
            });
        }

        public update() {
            if (this._entity.game == null) {
                Logger.warn("An interpolator from a dead entity was left behind. Ignoring...");
                return;
            }

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

                    var distance: number = Phaser.Point.distance(point, point2);

                    // If the distance is small, a teleport is in order and we continue on. This prevents small little hiccups
                    if ( distance < this._cutoffThreshold) {
                        this._entity.x = tweenData.x;
                        this._entity.y = tweenData.y;                        
                        return;
                    }

                    // Bring into the view faster if we're lagging behind severely
                    if (distance > 64) {
                        this._entity.x = tweenData.x;
                        this._entity.y = tweenData.y;
                        return;
                    }

                    // Kick off a new tween for the data
                    this._currentMovementTween = this._entity.game.add.tween(this._entity).to(tweenData, this.getInterpolationInterval(), Phaser.Easing.Linear.None, true);

                    // Add tween completion callback
                    this._currentMovementTween.onComplete.addOnce(() => {
                        if (this._currentMovementTween != null) {
                            this._currentMovementTween.stop();
                            this._currentMovementTween = null;                            
                        }

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

        /**
         * Based on the current backlog, returns a modified time to tween to speed up state consumption.
         * This is generally done to outpace the server sending rate.
         */
        private getInterpolationInterval() {
            if (this._states.length < 5)
                return this._backTime;
            if (this._states.length < 6)
                return this._backTime - 20;
            if (this._states.length < 10)
                return this._backTime - 30;
            if (this._states.length < 12)
                return this._backTime - 40;
            return this._backTime;
        }

        public isIdle(): boolean {
            if (this._currentMovementTween == null)
                return true;
            return false;
        }

        /**
         * Forces a flush of all states. All propertie will be immediately set to the very last state
         * available and the backlog will be cleared.
         */
        private forceFlush() {
            if (this._currentMovementTween != null) {
                this._currentMovementTween.stop();
                this._currentMovementTween = null;
            }

            var lastState: InterpolatorState = this._states[this._states.length - 1];

            this._entity.x = lastState._x;
            this._entity.y = lastState._y;
            this._entity.direction = lastState._dir;

            Logger.warn("A force flush was performed; looks like the network is falling behind.");           
            
            while (this._states.length) {                
                var state : InterpolatorState = this._states.shift();
            }
        }

        /**
         * Resets the interpolator and all state assosciated with it. 
         */
        reset() {
            if (this._currentMovementTween != null) {
                this._currentMovementTween.stop();
                this._currentMovementTween = null;
            }

            // Remove all elements
            while (this._states.length) {
                this._states.pop();
            }
        }

    }

} 