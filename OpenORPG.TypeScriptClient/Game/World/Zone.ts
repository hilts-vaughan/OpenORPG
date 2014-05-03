module OpenORPG {
    export class Zone {

        public game: Phaser.Game;
        private _mapId: number;
        public tileMap: Phaser.Tilemap;

        private static current: Zone;

        // Internal lists for usage later
        private _toRemove: any = [];
        private _toAdd: any = [];

        private entityLayer: Phaser.TilemapLayer;
        private entityGroup: Phaser.Group;
        private bucket: any;

        // An array of entities to use
        public entities: Array<Entity> = new Array<Entity>();
        public systems: Array<GameSystem> = new Array<GameSystem>();

        public movementSystem: MovementSystem;

        constructor(game: Phaser.Game, mapId: number) {
            this.game = game;
            this._mapId = mapId;

            Zone.current = this;

            // Setup tilemap
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


            // Create our systems as we need them
            this.movementSystem = new MovementSystem(this, null);
            var combatSystem = new CombatSystem(this, null);
            this.systems.push(this.movementSystem);
            this.systems.push(combatSystem);

            this.setupNetworkHandlers();

    

        }

        public addNetworkEntityToZone(entity: any): Entity {
            var worldEntity = new Entity(this.game, 0, 0);
            worldEntity.mergeWith(entity);

            this.entities[worldEntity.id] = worldEntity;
            this.entityGroup.addChild(worldEntity);


            // Fire off property changes
            for (var key in entity) {
                var value = entity[key];
                worldEntity.propertyChanged(key, value);
            }

            this._toAdd.push(worldEntity.id);

            return worldEntity;
        }


        public clearZone() {

            for (var entityKey in this.entities) {
                var entity = this.entities[entityKey];
                entity.destroy();

                // remove from list
                delete this.entities[entityKey];
            }

            for (var bucketKey in this.bucket) {
                var layer = this.bucket[bucketKey];
                layer.destroy();
            }

            // Destroy our tilemap
            this.tileMap.destroy();
            this.entityGroup.destroy(true);


            for (var systemKey in this.systems) {
                var system = this.systems[systemKey];
                system.destroy();
            }

         


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
        }

        public update() {
            for (var toRemove in this._toRemove) {

                var value = this._toRemove[toRemove];
                var entity = this.entities[value];

                entity.destroy();
                entity.destroyNamePlate();
                delete this.entities[toRemove];
            }



            for (var entityKey in this.entities) {
                var entity = this.entities[entityKey]
                entity.update();

                for (var layer in this.bucket)
                    this.game.physics.arcade.collide(entity, this.bucket[layer]);
            }

            // Update list of removal
            this._toRemove = []
            this._toAdd = []

            // Iterate our systems
            for (var system in this.systems) {
                this.systems[system].update();
            }

            // Re sort our entities
            this.entityGroup.sort('y', Phaser.Group.SORT_ASCENDING);

        }

        private setupNetworkHandlers() {
            var network = NetworkManager.getInstance();

            network.registerPacket(OpCode.SMSG_MOB_CREATE, (packet: any) => {
                var entity = Zone.current.addNetworkEntityToZone(packet.mobile);

                // Apply the fade effect
                EffectFactory.fadeEntityIn(entity);

            });

            network.registerPacket(OpCode.SMSG_MOB_DESTROY, (packet: any) => {
                Zone.current._toRemove.push(packet.id);
            });

            network.registerPacket(OpCode.SMSG_ENTITY_PROPERTY_CHANGE, (packet: any) => {
                var entity: any = Zone.current.entities[packet.entityId];
                entity.mergeWith(packet.properties);

                for (var key in packet.properties) {
                    var value = packet.properties[key];
                    entity.propertyChanged(key, value);
                }
            });

        }



    }
} 