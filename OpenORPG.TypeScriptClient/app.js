var OpenORPG;
(function (OpenORPG) {
    (function (Direction) {
        Direction[Direction["South"] = 0] = "South";
        Direction[Direction["West"] = 1] = "West";
        Direction[Direction["North"] = 2] = "North";
        Direction[Direction["East"] = 3] = "East";
    })(OpenORPG.Direction || (OpenORPG.Direction = {}));
    var Direction = OpenORPG.Direction;
})(OpenORPG || (OpenORPG = {}));
var OpenORPG;
(function (OpenORPG) {
    (function (CharacterState) {
        CharacterState[CharacterState["Idle"] = 0] = "Idle";
        CharacterState[CharacterState["Moving"] = 1] = "Moving";
        CharacterState[CharacterState["UsingSkill"] = 2] = "UsingSkill";
    })(OpenORPG.CharacterState || (OpenORPG.CharacterState = {}));
    var CharacterState = OpenORPG.CharacterState;
})(OpenORPG || (OpenORPG = {}));
var __extends = this.__extends || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    __.prototype = b.prototype;
    d.prototype = new __();
};
var OpenORPG;
(function (OpenORPG) {
    var DamageText = (function (_super) {
        __extends(DamageText, _super);
        function DamageText(entity, damage) {
            var _this = this;
            var style = FontFactory.getDamageFont();

            // Setup the important things here
            _super.call(this, entity.game, 0, 0, damage.toString(), style);

            this.anchor.set(0.5, 0.5);
            this.position.setTo(entity.width / 2, 0);
            entity.addChild(this);

            var effect = EffectFactory.floatAndFadeAway(entity.game, this);

            // When the effect is finished, scrap this
            effect.onComplete.add(function () {
                _this.destroy();
            }, this);
        }
        return DamageText;
    })(Phaser.Text);
    OpenORPG.DamageText = DamageText;
})(OpenORPG || (OpenORPG = {}));
var OpenORPG;
(function (OpenORPG) {
    var Entity = (function (_super) {
        __extends(Entity, _super);
        function Entity(game, x, y) {
            _super.call(this, game, x, y);
            // Our name tag header if required
            this.nameTagText = null;
            this.skillAnimation = null;

            this.anchor.setTo(0, 0);

            // Do something with this entity
            this.game.physics.enable(this, Phaser.Physics.ARCADE);
            this.body.collideWorldBounds = true;

            // Disable smoothing on the sprite
            this.smoothed = false;
        }
        Entity.prototype.update = function () {
            var directionString = this.directionToString();

            // Use of a skill animate over-takes everything
            if (this.skillAnimation != null) {
                if (this.skillAnimation.isFinished)
                    this.skillAnimation = null;
                return;
            }

            switch (this.characterState) {
                case 0 /* Idle */:
                    this.playIdle(directionString);
                    break;
                case 2 /* UsingSkill */:
                    this.playReadyingSkill(directionString);
                    break;
                case 1 /* Moving */:
                    this.playWalk(directionString);
                    break;
            }
        };

        Entity.prototype.playIdle = function (direction) {
            this.animations.play("idle_" + direction);
        };

        Entity.prototype.playReadyingSkill = function (direction) {
            this.animations.play("atk_" + direction);
        };

        Entity.prototype.playWalk = function (direction) {
            this.animations.play("walk_" + direction);
        };

        Entity.prototype.playSkillAnimation = function () {
            var direction = this.directionToString();
            this.skillAnimation = this.animations.play("atk_" + direction, 12, false);
        };

        Entity.prototype.directionToString = function () {
            switch (this.direction) {
                case 2 /* North */:
                    return "up";
                case 0 /* South */:
                    return "down";
                case 3 /* East */:
                    return "right";
                case 1 /* West */:
                    return "left";
            }
        };

        Entity.prototype.propertyChanged = function (name, value) {
            switch (name) {
                case "name":
                    this.updateName(value);
                    break;
                case "sprite":
                    this.updateSprite(value);
                    break;
            }
        };

        Entity.prototype.updateName = function (name) {
            if (this.nameTagText == null) {
                this.nameTagText = new Phaser.Text(this.game, 0, 0, name, FontFactory.getPlayerFont());
                this.nameTagText.anchor.set(0.5, 0.5);
                this.nameTagText.position.setTo(this.texture.width / 2, 0);
                this.addChild(this.nameTagText);
            } else {
                this.nameTagText.text = name;
            }
        };

        Entity.prototype.updateSprite = function (sprite) {
            var textureId = "entity_sprite_" + sprite;
            var image = this.game.cache.getImage(textureId);
            this.loadTexture(textureId, 0);

            var spriteDefinition = this.game.cache.getJSON("spritedef_" + textureId);
            var rowFrames = image.width / spriteDefinition.width;

            for (var animKey in spriteDefinition.animations) {
                var anim = spriteDefinition.animations[animKey];
                var index = rowFrames * anim.row;
                var frames = [];

                for (var i = 0; i < anim.length; i++) {
                    frames.push(index + i);
                }

                this.animations.add(animKey, frames, 4, true, true);
            }

            // Finished updating sprite
            this.animations.play("idle_down");
        };

        /* This is used for network transmission only, it can be dangerous if used
        without a second thought. For the most part, only let the networking code
        invoke this and do not use on any other methods that could be harmful.
        */
        Entity.prototype.mergeWith = function (object) {
            // Extends the objects with jQuery
            $.extend(this, object);
        };
        return Entity;
    })(Phaser.Sprite);
    OpenORPG.Entity = Entity;
})(OpenORPG || (OpenORPG = {}));
var OpenORPG;
(function (OpenORPG) {
    // A zone trigger is used to fire off events when they are entered
    var ZoneTrigger = (function () {
        function ZoneTrigger(game, x, y, width, height, callback, zone) {
            this.triggered = false;
            this.game = game;
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
            this.callback = callback;
            this.zone = zone;
        }
        ZoneTrigger.prototype.update = function (entity) {
            if (this.triggered)
                return;

            var rectangle = new Phaser.Rectangle(this.x, this.y, this.width, this.height);
            if (Phaser.Rectangle.intersects(rectangle, entity.body)) {
                this.triggered = true;
                this.callback();
            }
        };
        return ZoneTrigger;
    })();
    OpenORPG.ZoneTrigger = ZoneTrigger;
})(OpenORPG || (OpenORPG = {}));
var OpenORPG;
(function (OpenORPG) {
    /// <reference path="Infrastructure/World/GameSystem.ts" />
    var GameSystem = (function () {
        function GameSystem(parent) {
            this.parent = parent;
        }
        // Use this to update as required
        GameSystem.prototype.update = function () {
        };

        GameSystem.prototype.destroy = function () {
        };
        return GameSystem;
    })();
    OpenORPG.GameSystem = GameSystem;

    var CombatSystem = (function (_super) {
        __extends(CombatSystem, _super);
        function CombatSystem(zone, player) {
            var _this = this;
            _super.call(this, zone);

            // Setup our key presses
            this.meleeKey = zone.game.input.keyboard.addKey(Phaser.Keyboard.CONTROL);
            this.meleeKey.onDown.add(this.sendMeleeSkill, this);

            // Setup our network events
            var network = OpenORPG.NetworkManager.getInstance();
            network.registerPacket(18 /* SMSG_SKILL_USE_RESULT */, function (packet) {
                // Play animation on the client
                var user = _this.parent.entities[packet.userId];
                user.playSkillAnimation();

                // Do stuff to the victim
                var victim = _this.parent.entities[packet.targetId];
                var victimDamageText = new OpenORPG.DamageText(victim, packet.damage);
                EffectFactory.pulseDamage(victim);
            });
        }
        CombatSystem.prototype.attachTo = function (player) {
            this.player = player;
        };

        CombatSystem.prototype.sendMeleeSkill = function () {
            var network = OpenORPG.NetworkManager.getInstance();
            var packet = PacketFactory.createSkillUsePacket(1, -1);
            network.sendPacket(packet);
        };

        CombatSystem.prototype.destroy = function () {
            this.parent.game.input.keyboard.removeKey(Phaser.Keyboard.CONTROL);
        };

        CombatSystem.prototype.update = function () {
            // Poll for our input here
        };
        return CombatSystem;
    })(GameSystem);
    OpenORPG.CombatSystem = CombatSystem;

    var MovementSystem = (function (_super) {
        __extends(MovementSystem, _super);
        function MovementSystem(parent, player) {
            var _this = this;
            _super.call(this, parent);

            this.player = player;
            MovementSystem.current = this;

            var network = OpenORPG.NetworkManager.getInstance();
            network.registerPacket(21 /* SMSG_ENTITY_MOVE */, function (packet) {
                _this.handleEntityMove(packet);
            });

            this.timerToken = setInterval(this.generateMovementTicket, MovementSystem.MOVEMENT_TICKET_FREQUENCY);

            // Setup our zone trigger if it is at all possible
            this.topZoneTrigger = new OpenORPG.ZoneTrigger(this.parent.game, 0, 0, this.parent.tileMap.widthInPixels, this.parent.tileMap.tileHeight, this.topCallback, this.parent);
            this.bottomZoneTrigger = new OpenORPG.ZoneTrigger(this.parent.game, 0, this.parent.tileMap.heightInPixels - 32, this.parent.tileMap.widthInPixels, this.parent.tileMap.tileHeight, this.bottomCallback, this.parent);
            this.leftZoneTrigger = new OpenORPG.ZoneTrigger(this.parent.game, 0, 0, this.parent.tileMap.tileWidth, this.parent.tileMap.heightInPixels, this.leftCallback, this.parent);
            this.rightZoneTrigger = new OpenORPG.ZoneTrigger(this.parent.game, this.parent.tileMap.widthInPixels - 32, 0, this.parent.tileMap.tileWidth, this.parent.tileMap.heightInPixels, this.rightCallback, this.parent);
        }
        MovementSystem.prototype.topCallback = function () {
            // We make sure to force a movement update before all of these
            MovementSystem.current.generateMovementTicket(true);

            var network = OpenORPG.NetworkManager.getInstance();
            network.sendPacket(PacketFactory.createZoneRequestChange(2 /* North */));
        };

        MovementSystem.prototype.bottomCallback = function () {
            // We make sure to force a movement update before all of these
            MovementSystem.current.generateMovementTicket(true);

            var network = OpenORPG.NetworkManager.getInstance();
            network.sendPacket(PacketFactory.createZoneRequestChange(0 /* South */));
        };

        MovementSystem.prototype.rightCallback = function () {
            // We make sure to force a movement update before all of these
            MovementSystem.current.generateMovementTicket(true);

            var network = OpenORPG.NetworkManager.getInstance();
            network.sendPacket(PacketFactory.createZoneRequestChange(3 /* East */));
        };

        MovementSystem.prototype.leftCallback = function () {
            // We make sure to force a movement update before all of these
            MovementSystem.current.generateMovementTicket(true);

            var network = OpenORPG.NetworkManager.getInstance();
            network.sendPacket(PacketFactory.createZoneRequestChange(1 /* West */));
        };

        MovementSystem.prototype.update = function () {
            // If we're not viewing anything, quit
            if (this.player == null)
                return;

            // This chunk of code is used to control the player physics body around the map
            if (this.player.characterState == 1 /* Moving */ || this.player.characterState == 0 /* Idle */) {
                var speed = 120;
                if (this.parent.game.input.keyboard.isDown(Phaser.Keyboard.LEFT)) {
                    this.player.body.velocity.setTo(-120, 0);
                    this.player.direction = 1 /* West */;
                } else if (this.parent.game.input.keyboard.isDown(Phaser.Keyboard.RIGHT)) {
                    this.player.body.velocity.setTo(120, 0);
                    this.player.direction = 3 /* East */;
                } else if (this.parent.game.input.keyboard.isDown(Phaser.Keyboard.UP)) {
                    this.player.body.velocity.setTo(0, -120);
                    this.player.direction = 2 /* North */;
                } else if (this.parent.game.input.keyboard.isDown(Phaser.Keyboard.DOWN)) {
                    this.player.body.velocity.setTo(0, 120);
                    this.player.direction = 0 /* South */;
                } else {
                    if (!this.player.body.velocity.isZero())
                        this.generateMovementTicket(true);
                    this.player.body.velocity.setTo(0, 0);
                }
            }

            // We can check our zone triggers here now, these can fire whenever
            this.topZoneTrigger.update(this.player);
            this.bottomZoneTrigger.update(this.player);
            this.leftZoneTrigger.update(this.player);
            this.rightZoneTrigger.update(this.player);
        };

        MovementSystem.prototype.destroy = function () {
            this.player = null;

            // Stop and destroy the timer
            clearInterval(this.timerToken);
        };

        MovementSystem.prototype.attachEntity = function (entity) {
            this.player = entity;
        };

        MovementSystem.prototype.handleEntityMove = function (packet) {
            var id = packet.id;
            var position = packet.position;
            var entity = this.parent.entities[id];

            // Set a direction
            entity.direction = packet.direction;

            var properties = {
                x: position.x,
                y: position.y
            };

            // Start the tween immediately
            this.parent.game.add.tween(entity).to(properties, MovementSystem.MOVEMENT_TICKET_FREQUENCY + 50, Phaser.Easing.Linear.None, true);
        };

        MovementSystem.prototype.generateMovementTicket = function (override) {
            if (typeof override === "undefined") { override = false; }
            if (MovementSystem.current.player != null) {
                if (!MovementSystem.current.player.body.velocity.isZero()) {
                    var packet = PacketFactory.createMovementPacket(MovementSystem.current.player.x, MovementSystem.current.player.y, override, MovementSystem.current.player.direction);
                    OpenORPG.NetworkManager.getInstance().sendPacket(packet);
                }
            }
        };
        MovementSystem.MOVEMENT_TICKET_FREQUENCY = 200;
        MovementSystem.current = null;
        return MovementSystem;
    })(GameSystem);
    OpenORPG.MovementSystem = MovementSystem;
})(OpenORPG || (OpenORPG = {}));
var PacketFactory;
(function (PacketFactory) {
    function createLoginPacket(user, password) {
        return {
            opCode: 0 /* CMSG_LOGIN_REQUEST */,
            username: user,
            password: password
        };
    }
    PacketFactory.createLoginPacket = createLoginPacket;

    function createHeroSelectPacket(id) {
        return {
            opCode: 3 /* CMSG_HERO_SELECT */,
            heroId: id
        };
    }
    PacketFactory.createHeroSelectPacket = createHeroSelectPacket;

    function createMovementPacket(x, y, terminate, direction) {
        return {
            opCode: 15 /* CMSG_MOVEMENT_REQUEST */,
            currentPosition: {
                x: x,
                y: y
            },
            terminates: terminate,
            direction: direction
        };
    }
    PacketFactory.createMovementPacket = createMovementPacket;

    function createZoneRequestChange(dir) {
        return {
            opCode: 22 /* CMMSG_ZONE_CHANGE */,
            direction: dir
        };
    }
    PacketFactory.createZoneRequestChange = createZoneRequestChange;

    function createSkillUsePacket(skillId, targetId) {
        return {
            opCode: 17 /* CMSG_USE_SKILL */,
            skillId: skillId,
            targetId: targetId
        };
    }
    PacketFactory.createSkillUsePacket = createSkillUsePacket;
})(PacketFactory || (PacketFactory = {}));
var SpriteManager;
(function (SpriteManager) {
    function loadSpriteInfo(game) {
        game.load.json("spritepack", DirectoryHelper.getSpritePath() + "sprites.json");
    }
    SpriteManager.loadSpriteInfo = loadSpriteInfo;

    function loadSpriteDefintions(game) {
        var spritePackFromCache = game.cache.getJSON("spritepack");

        for (var key in spritePackFromCache) {
            // Get the value
            var value = spritePackFromCache[key];

            // Now, perform the actual load
            game.load.json("spritedef_" + key, DirectoryHelper.getSpritePath() + value + ".json");
        }
    }
    SpriteManager.loadSpriteDefintions = loadSpriteDefintions;

    function loadSpriteImages(game) {
        var spritePackFromCache = game.cache.getJSON("spritepack");

        for (var key in spritePackFromCache) {
            var spriteDef = game.cache.getJSON("spritedef_" + key);
            var value = spritePackFromCache[key];
            game.load.spritesheet(key, DirectoryHelper.getSpritePath() + value + ".png", spriteDef.width, spriteDef.height);
        }
    }
    SpriteManager.loadSpriteImages = loadSpriteImages;
})(SpriteManager || (SpriteManager = {}));
var OpenORPG;
(function (OpenORPG) {
    var BootState = (function (_super) {
        __extends(BootState, _super);
        function BootState() {
            _super.apply(this, arguments);
        }
        BootState.prototype.create = function () {
            var _this = this;
            this.game.stage.disableVisibilityChange = true;

            // Setup our connecting splash
            var connectingSplash = this.game.add.sprite(1024 / 2, 768 / 2, "connecting", 0);
            connectingSplash.anchor.setTo(0.5, 0.5);

            // Connect
            var network = OpenORPG.NetworkManager.getInstance();

            console.log("Preparing boot state...");

            network.onConnectionCallback = function () {
                _this.game.state.add("mainmenu", new OpenORPG.LoginMenuState());
                _this.game.state.start("mainmenu");
            };

            network.onConnectionErrorCallback = function () {
                _this.game.state.start("errorstate");
            };

            // Do a connection
            network.connect();
        };

        BootState.prototype.preload = function () {
            //TODO: Do some of the sprite loading we might want to do here
            this.game.load.image("connecting", "assets/ui/connecting.png");
        };
        return BootState;
    })(Phaser.State);
    OpenORPG.BootState = BootState;
})(OpenORPG || (OpenORPG = {}));
var OpenORPG;
(function (OpenORPG) {
    var Game = (function () {
        function Game() {
            // Init our game
            this.game = new Phaser.Game(1024, 768, Phaser.CANVAS, 'gameContainer', null, true, false);

            this.game.state.add("boot", new OpenORPG.BootState(), true);
            this.game.state.start("boot");
        }
        Game.prototype.preload = function () {
        };

        Game.prototype.create = function () {
        };
        return Game;
    })();
    OpenORPG.Game = Game;
})(OpenORPG || (OpenORPG = {}));
var OpenORPG;
(function (OpenORPG) {
    // The gameplay state manages
    var GameplayState = (function (_super) {
        __extends(GameplayState, _super);
        function GameplayState() {
            _super.call(this);
            this.zone = null;
        }
        GameplayState.prototype.preload = function () {
            SpriteManager.loadSpriteImages(this.game);

            // Load up everything else
            var loader = this.game.load;

            loader.tilemap("map_1", "assets/Maps/1.json", null, Phaser.Tilemap.TILED_JSON);
            loader.tilemap("map_2", "assets/Maps/2.json", null, Phaser.Tilemap.TILED_JSON);

            loader.image("tilesheet", "assets/Maps/tilesheet_16.png");
        };

        GameplayState.prototype.create = function () {
            var _this = this;
            // Start our physics systems
            this.game.physics.startSystem(Phaser.Physics.ARCADE);

            var network = OpenORPG.NetworkManager.getInstance();
            network.registerPacket(8 /* SMSG_ZONE_CHANGED */, function (packet) {
                if (_this.zone != null)
                    _this.zone.clearZone();

                _this.zone = new OpenORPG.Zone(_this.game, packet.zoneId);

                for (var entityKey in packet.entities) {
                    var entity = packet.entities[entityKey];

                    // Create your objects here
                    var worldEntity = _this.zone.addNetworkEntityToZone(entity);

                    // Do camera following here
                    if (worldEntity.id == packet.heroId) {
                        _this.game.camera.follow(worldEntity);
                        _this.zone.movementSystem.attachEntity(worldEntity);
                    }
                }
            });
        };

        GameplayState.prototype.update = function () {
            if (this.zone != null)
                this.zone.update();
        };
        return GameplayState;
    })(Phaser.State);
    OpenORPG.GameplayState = GameplayState;
})(OpenORPG || (OpenORPG = {}));
var OpenORPG;
(function (OpenORPG) {
    var HeroSelectState = (function (_super) {
        __extends(HeroSelectState, _super);
        function HeroSelectState() {
            _super.call(this);
        }
        HeroSelectState.prototype.preload = function () {
            // Prepare sprites
            SpriteManager.loadSpriteDefintions(this.game);
        };

        HeroSelectState.prototype.create = function () {
            var _this = this;
            // Select our first hero
            var packet = PacketFactory.createHeroSelectPacket(1);

            // Bind a network event
            var network = OpenORPG.NetworkManager.getInstance();
            network.registerPacket(6 /* SMSG_HERO_SELECT_RESPONSE */, function (packet) {
                _this.game.state.add("game", new OpenORPG.GameplayState());
                _this.game.state.start("game");
            });

            // Send our packet as required
            network.sendPacket(packet);
        };
        return HeroSelectState;
    })(Phaser.State);
    OpenORPG.HeroSelectState = HeroSelectState;
})(OpenORPG || (OpenORPG = {}));
var OpenORPG;
(function (OpenORPG) {
    var LoginMenuState = (function (_super) {
        __extends(LoginMenuState, _super);
        function LoginMenuState() {
            _super.apply(this, arguments);
        }
        LoginMenuState.prototype.create = function () {
            var _this = this;
            // Create our login button
            var button = this.game.add.button(1024 / 2, 768 / 2 + 120, "play_button", null, this, 1, 1, 2);
            button.anchor.setTo(0.5, 0.5);

            var text = this.game.add.text(1024 / 2, 768 / 2 - 170, "Select your hero", FontFactory.getPlayerFont());
            text.anchor.set(0.5, 0.5);

            var network = OpenORPG.NetworkManager.getInstance();

            network.registerPacket(1 /* SMSG_LOGIN_RESPONSE */, function (packet) {
                if (packet.status == 1) {
                    _this.game.state.add("heroselect", new OpenORPG.HeroSelectState());
                    _this.game.state.start("heroselect");
                }
            });

            if (Settings.autoLoginSet()) {
                var username = this.game.net.getQueryString("username");
                var password = this.game.net.getQueryString("password");

                //TODO: Get query parameters working
                var loginPacket = PacketFactory.createLoginPacket("Vaughan1", "Vaughan");
                network.sendPacket(loginPacket);
            }
        };

        LoginMenuState.prototype.preload = function () {
            var loader = this.game.load;

            // Load up the resources we need for here
            loader.image("scroll_bg", "assets/ui/scroll.png");
            loader.image("player_active", "assets/ui/player_active.png");
            loader.image("player_inactive", "assets/ui/player_inactive.png");
            loader.spritesheet("play_button", "assets/ui/play_button.png", 394, 154);

            SpriteManager.loadSpriteInfo(this.game);
        };
        return LoginMenuState;
    })(Phaser.State);
    OpenORPG.LoginMenuState = LoginMenuState;
})(OpenORPG || (OpenORPG = {}));
var OpenORPG;
(function (OpenORPG) {
    var Zone = (function () {
        function Zone(game, mapId) {
            // Internal lists for usage later
            this._toRemove = [];
            this._toAdd = [];
            // An array of entities to use
            this.entities = new Array();
            this.systems = new Array();
            this.game = game;
            this._mapId = mapId;

            Zone.current = this;

            // Setup tilemap
            this.tileMap = game.add.tilemap("map_" + mapId);
            this.tileMap.addTilesetImage("tilesheet");

            // Size and prepare
            var self = this;

            for (var layerKey in this.tileMap.layers) {
                var layer = this.tileMap.layers[layerKey];
                var worldLayer = this.tileMap.createLayer(layer.name);
                worldLayer.resizeWorld();
            }

            // Create our systems as we need them
            this.movementSystem = new OpenORPG.MovementSystem(this, null);
            var combatSystem = new OpenORPG.CombatSystem(this, null);
            this.systems.push(this.movementSystem);
            this.systems.push(combatSystem);

            this.setupNetworkHandlers();
        }
        Zone.prototype.addNetworkEntityToZone = function (entity) {
            var worldEntity = new OpenORPG.Entity(this.game, 0, 0);
            worldEntity.mergeWith(entity);

            this.entities[worldEntity.id] = worldEntity;
            this.game.add.existing(worldEntity);

            for (var key in entity) {
                var value = entity[key];
                worldEntity.propertyChanged(key, value);
            }

            return worldEntity;
        };

        Zone.prototype.clearZone = function () {
            for (var entityKey in this.entities) {
                var entity = this.entities[entityKey];
                entity.destroy();

                // remove from list
                delete this.entities[entityKey];
            }

            // Destroy our tilemap
            this.tileMap.destroy();

            for (var systemKey in this.systems) {
                var system = this.systems[systemKey];
                system.destroy();
            }
        };

        Zone.prototype.update = function () {
            for (var toRemove in this._toRemove) {
                var value = this._toRemove[toRemove];
                var entity = this.entities[value];
                entity.destroy();
                delete this.entities[toRemove];
            }

            // Update list of removal
            this._toRemove = [];

            for (var system in this.systems) {
                this.systems[system].update();
            }
        };

        Zone.prototype.setupNetworkHandlers = function () {
            var network = OpenORPG.NetworkManager.getInstance();

            network.registerPacket(9 /* SMSG_MOB_CREATE */, function (packet) {
                var entity = Zone.current.addNetworkEntityToZone(packet.mobile);

                // Apply the fade effect
                EffectFactory.fadeEntityIn(entity);
            });

            network.registerPacket(13 /* SMSG_MOB_DESTROY */, function (packet) {
                Zone.current._toRemove.push(packet.id);
            });

            network.registerPacket(20 /* SMSG_ENTITY_PROPERTY_CHANGE */, function (packet) {
                var entity = Zone.current.entities[packet.entityId];
                entity.mergeWith(packet.properties);

                for (var key in packet.properties) {
                    var value = packet.properties[key];
                    entity.propertyChanged(key, value);
                }
            });
        };
        return Zone;
    })();
    OpenORPG.Zone = Zone;
})(OpenORPG || (OpenORPG = {}));
var DirectoryHelper;
(function (DirectoryHelper) {
    // The base asset path
    var baseAssetPath = "assets/";

    function getSpritePath() {
        return baseAssetPath + "sprites/";
    }
    DirectoryHelper.getSpritePath = getSpritePath;

    function getAudioPath() {
        return baseAssetPath + "audio/";
    }
    DirectoryHelper.getAudioPath = getAudioPath;
})(DirectoryHelper || (DirectoryHelper = {}));
var EffectFactory;
(function (EffectFactory) {
    function fadeEntityIn(entity) {
        // Set the alpha to 0 to begin the fade in
        entity.alpha = 0;

        var properties = {
            alpha: 1
        };
        return entity.game.add.tween(entity).to(properties, 750, Phaser.Easing.Quadratic.In, true);
    }
    EffectFactory.fadeEntityIn = fadeEntityIn;

    // This effects causes an object to float and fade away
    function floatAndFadeAway(game, gameObject) {
        var properties = {
            y: gameObject.y - 32,
            alpha: 0
        };

        return game.add.tween(gameObject).to(properties, 1000, Phaser.Easing.Linear.None, true);
    }
    EffectFactory.floatAndFadeAway = floatAndFadeAway;

    function pulseDamage(entity) {
        var properties = {
            tint: 0x7E3517,
            alpha: 0.7
        };

        // Pulse the damage output
        return entity.game.add.tween(entity).to(properties, 250, Phaser.Easing.Linear.None, true, 0, 1, true);
    }
    EffectFactory.pulseDamage = pulseDamage;
})(EffectFactory || (EffectFactory = {}));
var FontFactory;
(function (FontFactory) {
    // Returns a basic, usable font for the game
    function getBasicFont() {
        return null;
    }
    FontFactory.getBasicFont = getBasicFont;

    // Returns a font for a specific player
    function getPlayerFont() {
        var font = new OpenORPG.FontDefinition("12px Georgia", "#ffff00", "center", "#000000", 3);
        return font;
    }
    FontFactory.getPlayerFont = getPlayerFont;

    function getDamageFont() {
        return new OpenORPG.FontDefinition("22px Georgia", "##AA0114", "center", "#000000", 3);
    }
    FontFactory.getDamageFont = getDamageFont;
})(FontFactory || (FontFactory = {}));
var OpenORPG;
(function (OpenORPG) {
    var FontDefinition = (function () {
        function FontDefinition(font, fill, align, stroke, strokeThickness, wordWrap, wordWrapWidth) {
            this.font = font;
            this.fill = fill;
            this.align = align;
            this.stroke = stroke;
            this.strokeThickness = strokeThickness;
            this.wordWrap = wordWrap;
            this.wordWrapWidth = wordWrapWidth;
        }
        return FontDefinition;
    })();
    OpenORPG.FontDefinition = FontDefinition;
})(OpenORPG || (OpenORPG = {}));
var OpenORPG;
(function (OpenORPG) {
    var NetworkManager = (function () {
        function NetworkManager(host, port) {
            this._port = 0;
            this._socket = null;
            // Our internal packet callbacks
            this._packetCallbacks = [];
            if (NetworkManager._instance) {
                throw new Error("Error: Instantiation failed: Use SingletonDemo.getInstance() instead of new.");
            }
            NetworkManager._instance = this;

            this._host = host;
            this._port = port;
        }
        NetworkManager.getInstance = function () {
            if (NetworkManager._instance === null) {
                NetworkManager._instance = new NetworkManager("localhost", 1234);
            }
            return NetworkManager._instance;
        };

        NetworkManager.prototype.sendPacket = function (packet) {
            var json = JSON.stringify(packet);
            this._socket.send(json);
        };

        NetworkManager.prototype.registerPacket = function (opCode, callback) {
            this._packetCallbacks[opCode] = callback;
        };

        NetworkManager.prototype.connect = function () {
            var _this = this;
            // Create our socket
            var url = "ws://" + this._host + ":" + this._port + "/";
            this._socket = new WebSocket(url);

            this._socket.onopen = function (event) {
                _this.onConnectionCallback();
            };

            this._socket.onerror = function (event) {
                _this.onConnectionErrorCallback();
            };

            this._socket.onmessage = function (event) {
                _this.parseMessage(event);
            };
        };

        // Parses an incoming message accordingly
        NetworkManager.prototype.parseMessage = function (response) {
            var packet = JSON.parse(response.data);
            if (this._packetCallbacks[packet.opCode] != undefined)
                this._packetCallbacks[packet.opCode](packet);
            else
                console.log("Packet with opCode " + packet.OpCode + " was recieved but not handled.");
        };
        NetworkManager._instance = null;
        return NetworkManager;
    })();
    OpenORPG.NetworkManager = NetworkManager;
})(OpenORPG || (OpenORPG = {}));
var OpenORPG;
(function (OpenORPG) {
    (function (OpCode) {
        OpCode[OpCode["CMSG_LOGIN_REQUEST"] = 0] = "CMSG_LOGIN_REQUEST";
        OpCode[OpCode["SMSG_LOGIN_RESPONSE"] = 1] = "SMSG_LOGIN_RESPONSE";
        OpCode[OpCode["SMSG_HERO_LIST"] = 2] = "SMSG_HERO_LIST";
        OpCode[OpCode["CMSG_HERO_SELECT"] = 3] = "CMSG_HERO_SELECT";
        OpCode[OpCode["CMSG_HERO_CREATE"] = 4] = "CMSG_HERO_CREATE";

        OpCode[OpCode["SMSG_GAME_OBJECT_UPDATE"] = 5] = "SMSG_GAME_OBJECT_UPDATE";
        OpCode[OpCode["SMSG_HERO_SELECT_RESPONSE"] = 6] = "SMSG_HERO_SELECT_RESPONSE";
        OpCode[OpCode["SMSG_HERO_CREATE_RESPONSE"] = 7] = "SMSG_HERO_CREATE_RESPONSE";
        OpCode[OpCode["SMSG_ZONE_CHANGED"] = 8] = "SMSG_ZONE_CHANGED";
        OpCode[OpCode["SMSG_MOB_CREATE"] = 9] = "SMSG_MOB_CREATE";

        OpCode[OpCode["SMSG_CHAT_MESSAGE"] = 10] = "SMSG_CHAT_MESSAGE";
        OpCode[OpCode["SMSG_JOIN_CHANNEL"] = 11] = "SMSG_JOIN_CHANNEL";
        OpCode[OpCode["CMSG_CHAT_MESSAGE"] = 12] = "CMSG_CHAT_MESSAGE";
        OpCode[OpCode["SMSG_MOB_DESTROY"] = 13] = "SMSG_MOB_DESTROY";
        OpCode[OpCode["SMSG_ATTRIBUTES_CHANGED"] = 14] = "SMSG_ATTRIBUTES_CHANGED";
        OpCode[OpCode["CMSG_MOVEMENT_REQUEST"] = 15] = "CMSG_MOVEMENT_REQUEST";
        OpCode[OpCode["SMSG_MOB_MOVEMENT"] = 16] = "SMSG_MOB_MOVEMENT";
        OpCode[OpCode["CMSG_USE_SKILL"] = 17] = "CMSG_USE_SKILL";
        OpCode[OpCode["SMSG_SKILL_USE_RESULT"] = 18] = "SMSG_SKILL_USE_RESULT";
        OpCode[OpCode["SMSG_FLOATING_NUMBER"] = 19] = "SMSG_FLOATING_NUMBER";

        OpCode[OpCode["SMSG_ENTITY_PROPERTY_CHANGE"] = 20] = "SMSG_ENTITY_PROPERTY_CHANGE";
        OpCode[OpCode["SMSG_ENTITY_MOVE"] = 21] = "SMSG_ENTITY_MOVE";
        OpCode[OpCode["CMMSG_ZONE_CHANGE"] = 22] = "CMMSG_ZONE_CHANGE";
        OpCode[OpCode["CMSG_HERO_EQUIP"] = 23] = "CMSG_HERO_EQUIP";
    })(OpenORPG.OpCode || (OpenORPG.OpCode = {}));
    var OpCode = OpenORPG.OpCode;
})(OpenORPG || (OpenORPG = {}));
var Settings;
(function (Settings) {
    function autoLoginSet() {
        return true;
    }
    Settings.autoLoginSet = autoLoginSet;
})(Settings || (Settings = {}));
/// <reference path="jquery.d.ts" />
/// <reference path="phaser.d.ts" />
/// <reference path="Game/CharacterState.ts" />
/// <reference path="Game/World/Zone.ts" />
/// <reference path="Game/Direction.ts" />
window.onload = function () {
    var game = new OpenORPG.Game();
};
//# sourceMappingURL=app.js.map
