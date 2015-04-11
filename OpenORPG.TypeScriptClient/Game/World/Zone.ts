module OpenORPG {
    export class Zone {

        public game: Phaser.Game;
        private _mapId: number;
        private playerInfo : PlayerInfo;

        public tileMap: Phaser.Tilemap;

        private static current: Zone;

        // Internal lists for usage later
        private _toRemove: any = [];
        private _toAdd: any = [];
        private _toUpdate: any = [];

        private entityLayer: Phaser.TilemapLayer;
        private entityGroup: Phaser.Group;
        private bucket: any;

        // An array of entities to use
        public entities: Array<Entity> = new Array<Entity>();
        public systems: Array<GameSystem> = new Array<GameSystem>();

        public movementSystem: MovementSystem;
        public combatSystem : CombatSystem;

        constructor(game: Phaser.Game, playerInfo : PlayerInfo) {
            this.game = game;
            this.playerInfo = playerInfo;        

            Zone.current = this;


            this.setupNetworkHandlers();



        }

        public initLocalZone(mapId: number) {

            // Setup tilemap
            var game = this.game;
            this._mapId = mapId;

            this.tileMap = game.add.tilemap("map_" + mapId);
            this.tileMap.addTilesetImage("tilesheet");

            // Size and prepare
            var self: Zone = this;
            this.bucket = [];

            for (var layerKey in this.tileMap.layers) {
                var layer: any = this.tileMap.layers[layerKey];
                var worldLayer = this.tileMap.createLayer(layer.name);
                worldLayer.resizeWorld();
                this.bucket.push(worldLayer);

                // Check if this is the entity layer
                if (worldLayer.layer["name"] == "Entities") {
                    this.entityLayer = worldLayer;
                    this.entityGroup = new Phaser.Group(this.game);
                    this.game.world.addAt(this.entityGroup, worldLayer.index + 1);
                }

            }

            this.generateCollisionMap();

            // Create systems
            if (this.movementSystem == null) {

                // Create our systems as we need them
                this.movementSystem = new MovementSystem(this, null);
                this.combatSystem = new CombatSystem(this, null, this.playerInfo);
                this.systems.push(this.movementSystem);
                this.systems.push(this.combatSystem);
            }


            this.systems.forEach((system : GameSystem) => {
                system.initZone();
            });

        }

    

        public addNetworkEntityToZone(entity: any): Entity {
            var worldEntity = new Entity(this.game, 0, 0);
            worldEntity.mergeWith(entity);
            worldEntity.initAsNetworkable();

            this.entities[worldEntity.id] = worldEntity;
            this.entityGroup.addChild(worldEntity);


            // Fire off property changes
            for (var key in entity) {
                var value = entity[key];
                worldEntity.propertyChanged(key, value);
            }

            this._toAdd.push(worldEntity.id);

            // Allow adding hooks where needed
            this.systems.forEach((system: GameSystem) => system.onEntityAdded(worldEntity));

            return worldEntity;
        }


        public clearZone() {

            for (var entityKey in this.entities) {
                var entity = this.entities[entityKey];
                entity.destroy();

                entity.destroyNamePlate();

                // remove from list
                delete this.entities[entityKey];
            }

            // Create
            this.entities = new Array<Entity>();

            for (var bucketKey in this.bucket) {
                var layer = this.bucket[bucketKey];
                layer.destroy();
            }

            // Destroy our tilemap
            if (this.tileMap != null)
                this.tileMap.destroy();

            if (this.tileMap != null)
                this.entityGroup.destroy(true);




        }

        private generateCollisionMap() {
            var props: any = this.tileMap.tilesets[0];
            var tileProps = props.tileProperties;

            for (var propKey in tileProps) {
                var propValue: Object = tileProps[propKey];

                if (propValue.hasOwnProperty("c")) {
                    // If collision flag is set, then we'll push the index
                    for (var i = 0; i < this.tileMap.layers.length; i++) {
                        this.tileMap.setCollision([parseInt(propKey) + 1], true, i);
                    }
                }

            }



        }


        render() {
            for (var system in this.systems) {
                this.systems[system].render();
            }

            for (var entityKey in this.entities) {
                var entity = this.entities[entityKey];  
                entity.render();
            }

        }

        public update() {

            for (var toRemove in this._toRemove) {

                var value = this._toRemove[toRemove];
                var entity = this.entities[value];

                Logger.debug("Entity was removed from the current zone");
                Logger.debug(entity);

                // Allow any unhooking that needs to be done first
                this.systems.forEach((system: GameSystem) => system.onEntityRemoved(entity));

                entity.destroy();
                entity.destroyNamePlate();
                delete this.entities[value];
            }

            for (var toAdd in this._toAdd) {
                var valueA = this._toAdd[toAdd];

                var entityA = Zone.current.addNetworkEntityToZone(valueA);
                Logger.debug("Entity was added to the current zone");
                Logger.debug(entityA);



                // Apply the fade effect
                EffectFactory.fadeEntityIn(entityA);

            }

            for (var toUpdate in this._toUpdate) {
                var valueB = this._toUpdate[toUpdate];

                var entityB: any = Zone.current.entities[valueB.entityId];
                entityB.mergeWith(valueB.properties);

                for (var key in valueB.properties) {
                    var v = valueB.properties[key];
                    entityB.propertyChanged(key, v);
                }


            }


            for (var entityKey in this.entities) {
                var entity = this.entities[entityKey];  
                // entity.update();

                for (var layer in this.bucket)
                    this.game.physics.arcade.collide(entity, this.bucket[layer]);
            }

            // Update list of removal
            this._toRemove = [];
            this._toAdd = [];
            this._toUpdate = [];

            // Iterate our systems
            for (var system in this.systems) {
                this.systems[system].update();
            }

            // Re sort our entities
            if (this.entityGroup != null)
                this.entityGroup.sort('y', Phaser.Group.SORT_ASCENDING);

        }

        private setupNetworkHandlers() {
            var network = NetworkManager.getInstance();

            network.registerPacket(OpCode.SMSG_MOB_CREATE, (packet: any) => {


                Zone.current._toAdd.push(packet.mobile);
            });

            network.registerPacket(OpCode.SMSG_MOB_DESTROY, (packet: any) => {
                Zone.current._toRemove.push(packet.id);
            });

            network.registerPacket(OpCode.SMSG_ENTITY_PROPERTY_CHANGE, (packet: any) => {
                Zone.current._toUpdate.push(packet);
            });

        }



    }
} 