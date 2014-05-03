module OpenORPG {
    /// <reference path="Infrastructure/World/GameSystem.ts" />

    export class GameSystem {

        public parent: Zone;


        constructor(parent: Zone) {
            this.parent = parent;
        }

        // Use this to update as required
        update() {

        }

        destroy() {

        }

        render() {

        }


    }

    export class CombatSystem extends GameSystem {

        private meleeKey: Phaser.Key;
        private player: Entity;

        constructor(zone: Zone, player: Entity) {
            super(zone);

            // Setup our key presses
            this.meleeKey = zone.game.input.keyboard.addKey(Phaser.Keyboard.CONTROL);
            this.meleeKey.onDown.add(this.sendMeleeSkill, this);

            // Setup our network events
            var network = NetworkManager.getInstance();
            network.registerPacket(OpCode.SMSG_SKILL_USE_RESULT, (packet: any) => {
                // Play animation on the client
                var user = this.parent.entities[packet.userId];
                user.playSkillAnimation();

                // Do stuff to the victim
                var victim = this.parent.entities[packet.targetId]
                var victimDamageText = new DamageText(victim, packet.damage);
                EffectFactory.pulseDamage(victim);

                // Play hit effect
                var effect = this.parent.game.add.audio("audio_effect_hit", 0.3, false, true);
                effect.play();

            });


        }

        public attachTo(player: Entity) {
            this.player = player;
        }


        private sendMeleeSkill() {
            var network = NetworkManager.getInstance();
            var packet = PacketFactory.createSkillUsePacket(1, -1);
            network.sendPacket(packet);
        }

        destroy() {
            this.parent.game.input.keyboard.removeKey(Phaser.Keyboard.CONTROL);
        }

        update() {

            // Poll for our input here

        }


    }


    export class MovementSystem extends GameSystem {

        private static MOVEMENT_TICKET_FREQUENCY: number = 500;
        private static current: MovementSystem = null;

        private timerToken: number;
        private player: Entity;

        // Our zone changing triggers
        private topZoneTrigger: ZoneTrigger;
        private bottomZoneTrigger: ZoneTrigger;
        private leftZoneTrigger: ZoneTrigger;
        private rightZoneTrigger: ZoneTrigger;


        constructor(parent: Zone, player: Entity) {
            super(parent);

            this.player = player;
            MovementSystem.current = this;

            var network = NetworkManager.getInstance();
            network.registerPacket(OpCode.SMSG_ENTITY_MOVE, (packet: any) => {
                this.handleEntityMove(packet);
            });

            this.timerToken = setInterval(this.generateMovementTicket, MovementSystem.MOVEMENT_TICKET_FREQUENCY);

            // Setup our zone trigger if it is at all possible
            this.topZoneTrigger = new ZoneTrigger(this.parent.game, 0, 0, this.parent.tileMap.widthInPixels, this.parent.tileMap.tileHeight, this.topCallback, this.parent);
            this.bottomZoneTrigger = new ZoneTrigger(this.parent.game, 0, this.parent.tileMap.heightInPixels - 32, this.parent.tileMap.widthInPixels, this.parent.tileMap.tileHeight, this.bottomCallback, this.parent);
            this.leftZoneTrigger = new ZoneTrigger(this.parent.game, 0, 0, this.parent.tileMap.tileWidth, this.parent.tileMap.heightInPixels, this.leftCallback, this.parent);
            this.rightZoneTrigger = new ZoneTrigger(this.parent.game, this.parent.tileMap.widthInPixels - 32, 0, this.parent.tileMap.tileWidth, this.parent.tileMap.heightInPixels, this.rightCallback, this.parent);

        }

        render() {
            if (this.player != null) {
                //this.parent.game.debug.body(this.player, "yellow", true);
            }
        }

        private topCallback() {
            // We make sure to force a movement update before all of these
            MovementSystem.current.generateMovementTicket(true);

            var network = NetworkManager.getInstance();
            network.sendPacket(PacketFactory.createZoneRequestChange(Direction.North));
        }

        private bottomCallback() {
            // We make sure to force a movement update before all of these
            MovementSystem.current.generateMovementTicket(true);

            var network = NetworkManager.getInstance();
            network.sendPacket(PacketFactory.createZoneRequestChange(Direction.South));
        }

        private rightCallback() {
            // We make sure to force a movement update before all of these
            MovementSystem.current.generateMovementTicket(true);

            var network = NetworkManager.getInstance();
            network.sendPacket(PacketFactory.createZoneRequestChange(Direction.East));
        }

        private leftCallback() {
            // We make sure to force a movement update before all of these
            MovementSystem.current.generateMovementTicket(true);

            var network = NetworkManager.getInstance();
            network.sendPacket(PacketFactory.createZoneRequestChange(Direction.West));
        }


        update() {
            // If we're not viewing anything, quit
            if (this.player == null)
                return;

            // This chunk of code is used to control the player physics body around the map
            if (this.player.characterState == CharacterState.Moving || this.player.characterState == CharacterState.Idle) {
                var speed: number = 120;
                var velocity: Phaser.Point = new Phaser.Point(0, 0);

                if (this.parent.game.input.keyboard.isDown(Phaser.Keyboard.LEFT)) {
                    velocity.add(-120, 0);
                }
                if (this.parent.game.input.keyboard.isDown(Phaser.Keyboard.RIGHT)) {
                    velocity.add(120, 0);
                }
                if (this.parent.game.input.keyboard.isDown(Phaser.Keyboard.UP)) {
                    velocity.add(0, -120);
                }

                if (this.parent.game.input.keyboard.isDown(Phaser.Keyboard.DOWN)) {
                    velocity.add(0, 120);
                }

                if(velocity.isZero()) {
                    if (!this.player.body.velocity.isZero())
                        this.generateMovementTicket(true);
                    this.player.body.velocity.setTo(0, 0);
                }


                if (!velocity.isZero()) {
                    this.player.body.velocity.setTo(velocity.x, velocity.y);

                    if (velocity.x < 0)
                        this.player.direction = Direction.West;
                    else if (velocity.x > 0)
                        this.player.direction = Direction.East;

                    if (velocity.y < 0)
                        this.player.direction = Direction.North;
                    else if (velocity.y > 0)
                        this.player.direction = Direction.South;

                }

            } // end movement controller

            // We can check our zone triggers here now, these can fire whenever
            this.topZoneTrigger.update(this.player);
            this.bottomZoneTrigger.update(this.player);
            this.leftZoneTrigger.update(this.player);
            this.rightZoneTrigger.update(this.player);


        }

        destroy() {
            this.player = null;

            // Stop and destroy the timer
            clearInterval(this.timerToken);
        }

        public attachEntity(entity: Entity) {
            this.player = entity;
        }

        private handleEntityMove(packet: any) {
            var id: number = packet.id;
            var position: any = packet.position;
            var entity: Entity = this.parent.entities[id];

            // Set a direction
            entity.direction = packet.direction;

            var properties =
                {
                    x: position.x,
                    y: position.y
                }

            // Start the tween immediately
            this.parent.game.add.tween(entity).to(properties, MovementSystem.MOVEMENT_TICKET_FREQUENCY + 50, Phaser.Easing.Linear.None, true);


        }

        private generateMovementTicket(override: boolean = false) {
            if (MovementSystem.current.player != null) {
                if (!MovementSystem.current.player.body.velocity.isZero()) {
                    var packet = PacketFactory.createMovementPacket(MovementSystem.current.player.x, MovementSystem.current.player.y, override, MovementSystem.current.player.direction);
                    NetworkManager.getInstance().sendPacket(packet);
                }
            }
        }


    }

} 