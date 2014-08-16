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

        initZone() {

        }

        render() {

        }

        /*
         * This method is invoked when an entity has become added to the current zone for ease of
         * feedback for clients that are subscribing. 
         */
        onEntityAdded(entity: Entity) {
            
        }

        /*
         * This method is invoked when an entity has become removed from the current zone for ease of
         * feedback for clients that are subscribing.
         */
        onEntityRemoved(entity: Entity) {
            
        }

    }

    export class CombatSystem extends GameSystem {

        private meleeKey: Phaser.Key;
        private interactKey: Phaser.Key;

        private player: Entity;
        private targetEntity : Entity;
        private playerInfo: PlayerInfo;

        constructor(zone: Zone, player: Entity, playerInfo: PlayerInfo) {
            super(zone);

            // Setup our key presses and do sonly once
            this.meleeKey = zone.game.input.keyboard.addKey(Phaser.Keyboard.CONTROL);
            this.meleeKey.onDown.add(this.sendMeleeSkill, this);

            this.interactKey = zone.game.input.keyboard.addKey(Phaser.Keyboard.SHIFT);
            this.interactKey.onDown.add(this.sendInteraction, this);

            this.playerInfo = playerInfo;

            // Setup our network events
            var network = NetworkManager.getInstance();
            network.registerPacket(OpCode.SMSG_SKILL_USE_RESULT, (packet: any) => {
                // Play animation on the client
                var user = this.parent.entities[packet.userId];

                if (user != null) {
                    user.playSkillAnimation();

                    // reset the skill cooldown if required
                    if (user.id == this.player.id) {
                        var skillId = packet.skillId;
                        var skill = <Skill> _.find(this.playerInfo.characterSkills, (s: Skill) => {
                            return s.id == skillId;
                        });

                        if (skill)
                            skill.resetCooldown();
                        else
                            Logger.error("Attempted to reset cooldown for skill " + skillId + " but the player did not possess the skill.");

                    }
                }

                // Do stuff to the victim
                var victim = this.parent.entities[packet.targetId];

                // If this packet is delayed, we might get a null reference
                if (victim != null) {
                    this.applyToVictim(victim, packet.damage);
                } else {
                    Logger.warn("CombatSystem - Attempted to apply skill sequence to victim but no longer existed");
                }


            });


        }

        onEntityAdded(entity: Entity) {
            entity.events.onInputDown.add(this.handleSelection, this);
        }

        onEntityRemoved(entity: Entity) {
            entity.events.onInputDown.remove(this.handleSelection, this);
        }

        /*
         * This method is called when an entity has been selected.
         * The system will handle what will happen.
         */
        private handleSelection(entity : Entity) {
            this.selectTarget(entity);
        }

        private selectTarget(entity: Entity) {
            // Select our new target
            entity.performSelection();

            if (this.targetEntity != null)
                this.targetEntity.performDeselection();

            this.targetEntity = entity;

            var packet = PacketFactory.createTargetNotification(this.targetEntity.id);
            NetworkManager.getInstance().sendPacket(packet);

        }

        private applyToVictim(victim: Entity, damage: number) {
            var victimDamageText = new DamageText(victim, damage);
            EffectFactory.pulseDamage(victim);

            // Play hit effect
            var effect = this.parent.game.add.audio("audio_effect_hit", 0.3, false, true);
            effect.play();
        }

        public attachTo(player: Entity) {
            this.player = player;
        }


        private sendMeleeSkill() {
            var network = NetworkManager.getInstance();
            var packet = PacketFactory.createSkillUsePacket(1, -1);
            network.sendPacket(packet);
        }

        private sendInteraction() {
            var network = NetworkManager.getInstance();
            var packet = PacketFactory.createInteractionRequest();
            network.sendPacket(packet);
        }


        destroy() {
            this.parent.game.input.keyboard.removeKey(Phaser.Keyboard.CONTROL);
        }

        update() {

            this.playerInfo.characterSkills.forEach((skill: Skill) => {
                skill.cooldown = Math.max(0, skill.cooldown - this.parent.game.time.elapsed / 1000);
            });

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


        }

        initZone() {
            // Setup our zone trigger if it is at all possible
            this.topZoneTrigger = new ZoneTrigger(this.parent.game, 0, 0, this.parent.tileMap.widthInPixels, this.parent.tileMap.tileHeight, this.topCallback.bind(this), this.parent);
            this.bottomZoneTrigger = new ZoneTrigger(this.parent.game, 0, this.parent.tileMap.heightInPixels - 32, this.parent.tileMap.widthInPixels, this.parent.tileMap.tileHeight, this.bottomCallback.bind(this), this.parent);
            this.leftZoneTrigger = new ZoneTrigger(this.parent.game, 0, 0, this.parent.tileMap.tileWidth, this.parent.tileMap.heightInPixels, this.leftCallback.bind(this), this.parent);
            this.rightZoneTrigger = new ZoneTrigger(this.parent.game, this.parent.tileMap.widthInPixels - 32, 0, this.parent.tileMap.tileWidth, this.parent.tileMap.heightInPixels, this.rightCallback.bind(this), this.parent);
        }

        render() {
            if (this.player != null) {
                //this.parent.game.debug.body(this.player, "yellow", true);
            }
        }

        private topCallback() {
            // We make sure to force a movement update before all of these
            MovementSystem.current.generateMovementTicket(true);
            this.sendZoneChangeRequest(Direction.North);
        }

        private bottomCallback() {
            // We make sure to force a movement update before all of these
            MovementSystem.current.generateMovementTicket(true);

            var network = NetworkManager.getInstance();
            this.sendZoneChangeRequest(Direction.South);
        }

        private rightCallback() {
            // We make sure to force a movement update before all of these
            MovementSystem.current.generateMovementTicket(true);
            this.sendZoneChangeRequest(Direction.East);
        }

        private leftCallback() {
            // We make sure to force a movement update before all of these
            MovementSystem.current.generateMovementTicket(true);
            this.sendZoneChangeRequest(Direction.West);
        }

       private sendZoneChangeRequest(direction: Direction) {
           var network = NetworkManager.getInstance();
           network.sendPacket(PacketFactory.createZoneRequestChange(direction));
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

                if (velocity.isZero()) {
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

                    this.player.playWalk(this.player.directionToString());

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

            if (entity) {

                // Set a direction
                entity.direction = packet.direction;

                var properties =
                {
                    x: position.x,
                    y: position.y
                }

                // Start the tween immediately
                this.parent.game.add.tween(entity).to(properties, MovementSystem.MOVEMENT_TICKET_FREQUENCY + 50, Phaser.Easing.Linear.None, true);
            } else {
                Logger.warn("MovementSystem - Attempted to move and tween a non-existant entity. It probably moved.");
            }


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