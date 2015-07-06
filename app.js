var OpenORPG;
(function (OpenORPG) {
    var ChatChannel = (function () {
        function ChatChannel(channelId, channelName, chatType) {
            this.channelId = channelId;
            this.channelName = channelName;
            this.channelType = chatType;
        }
        return ChatChannel;
    })();
    OpenORPG.ChatChannel = ChatChannel;
    (function (ChannelType) {
        ChannelType[ChannelType["Global"] = 0] = "Global";
        ChannelType[ChannelType["Zone"] = 1] = "Zone";
        ChannelType[ChannelType["Administration"] = 2] = "Administration";
        ChannelType[ChannelType["Trade"] = 3] = "Trade";
        ChannelType[ChannelType["Party"] = 4] = "Party";
        ChannelType[ChannelType["Guild"] = 5] = "Guild";
        ChannelType[ChannelType["System"] = 6] = "System";
    })(OpenORPG.ChannelType || (OpenORPG.ChannelType = {}));
    var ChannelType = OpenORPG.ChannelType;
})(OpenORPG || (OpenORPG = {}));
var OpenORPG;
(function (OpenORPG) {
    /*
     * A class responsible for handling the various client sided commands
     */
    var ChatCommandHandler = (function () {
        function ChatCommandHandler() {
            this.callbackTable = {};
        }
        /*
         * Allows registration of callbacks from the outside world. The chat manager will
         * be responsible for matching and executing these. Callbacks will be fired accordingly.
         */
        ChatCommandHandler.prototype.registerCallback = function (command, callback) {
            this.callbackTable[command] = callback;
        };
        /*
         * Handles the incoming command type and attempts to fire the callback from the table.
           If it cannot be found, then it is ignored.
         */
        ChatCommandHandler.prototype.handleCommand = function (command, message) {
            var args = this.getArgumentsFromMessage(message);
            if (this.callbackTable[command]) {
                this.callbackTable[command](args);
            }
        };
        /**
         * Given a string from the user input, returns the parameters from them.
         * Parameters are identified by spaces, unless they are double quoted. For example:
         *
         * 'Vaughan Apple Banana' would be three parameters, ['Vaughan', 'Apple', 'Banana']
         * 'Vaughan "Apple Banana"' would be two parameters, ['Vaughan', 'Apple Banana']
         *
         * The latter is primairly used for looking up user names and the like, things with
         * spaces in them. Generally, parameters are seperated by spaces.
         */
        ChatCommandHandler.prototype.getArgumentsFromMessage = function (message) {
            var args = message.match(/(?:[^\s"]+|"[^"]*")+/g);
            args.shift();
            return args;
        };
        return ChatCommandHandler;
    })();
    OpenORPG.ChatCommandHandler = ChatCommandHandler;
})(OpenORPG || (OpenORPG = {}));
var OpenORPG;
(function (OpenORPG) {
    // This manager is used for maintaing and hooking into chat related functionality in the game
    // If it has to do with talking in the game world, you'll find it here.
    var ChatManager = (function () {
        function ChatManager() {
            var _this = this;
            this._chatChannels = new Array();
            this.commandParser = new OpenORPG.CommandParser();
            this.chatCommandHandler = new OpenORPG.ChatCommandHandler();
            this._chatLogElement = "chatlog";
            this.channelColorMap = new Array();
            // Hook into the DOM
            $(document).on('keypress', "#chatmessage", function (event) {
                if (event.which == 13) {
                    var message = $("#chatmessage").val();
                    var command = _this.commandParser.parseMessageType(message);
                    if (command == 0 /* UnknownCommand */) {
                        _this.sendMessageToServer(message);
                    }
                    else {
                        _this.chatCommandHandler.handleCommand(command, message);
                    }
                    $("#chatmessage").val(""); // clear chat box
                    event.preventDefault();
                }
            });
            $.getJSON("assets/config/chat_color_map.json", function (data) {
                var i = 0;
                for (var key in data) {
                    var value = data[key];
                    _this.channelColorMap[i] = value;
                    i++;
                }
            });
            this.setupNetworkHandlers();
        }
        ChatManager.prototype.sendMessageToServer = function (message) {
            var packet = PacketFactory.createChatPacket(0, message);
            OpenORPG.NetworkManager.getInstance().sendPacket(packet);
        };
        ChatManager.prototype.setupNetworkHandlers = function () {
            var _this = this;
            var network = OpenORPG.NetworkManager.getInstance();
            // Init
            OpenORPG.LocaleManager.getInstance();
            // Handle channel registration
            network.registerPacket(11 /* SMSG_JOIN_CHANNEL */, function (packet) {
                var channel = new OpenORPG.ChatChannel(packet.channelId, packet.channelName, packet.channelType);
                _this._chatChannels[channel.channelId] = channel;
            });
            network.registerPacket(24 /* SMSG_LEAVE_CHAT_CHANNEL */, function (packet) {
                delete _this._chatChannels[packet.channelId];
            });
            network.registerPacket(10 /* SMSG_CHAT_MESSAGE */, function (packet) {
                var message = packet.message;
                var id = packet.channelId;
                var sender = packet.sender;
                _this.processIncomingMessage(sender, message, id);
            });
            network.registerPacket(34 /* SMSG_SEND_GAMEMESSAGE */, function (packet) {
                var messageType = packet.messageType;
                var args = packet.arguments;
                var message = OpenORPG.LocaleManager.getInstance().getString(messageType, args);
                _this.addMessage(message, "", 6 /* System */);
            });
            // Setup our handlers for doing stuff here; not sure how we'll support more broad commands as of yet
            this.chatCommandHandler.registerCallback(3 /* Echo */, function (args) {
                _this.addMessage(args.join(" "), "", 6 /* System */);
            });
        };
        ChatManager.prototype.processIncomingMessage = function (sender, message, id) {
            var chatChannel = this._chatChannels[id];
            if (chatChannel != null) {
                this.addMessage(message, sender + ": ", chatChannel.channelType);
            }
            else {
                Logger.warn("ChatManager - Failed to find chat channel with id of " + id + ". Was it not registered");
            }
        };
        ChatManager.prototype.addMessage = function (message, user, channel) {
            var _this = this;
            if (user === void 0) { user = ""; }
            if (channel === void 0) { channel = 6 /* System */; }
            $.get("assets/hud/chat/chat_message_line.html", function (html) {
                var data = {
                    playerName: user,
                    message: message
                };
                var chatLineHtml = _.template(html, data);
                var chatElement = $(chatLineHtml);
                $(chatElement).css("color", _this.channelColorMap[channel]);
                $("#chatlog").append(chatElement);
                // Scroll down
                $("#chatlog").animate({ scrollTop: $("#chatlog")[0].scrollHeight }, 1000);
            });
        };
        return ChatManager;
    })();
    OpenORPG.ChatManager = ChatManager;
})(OpenORPG || (OpenORPG = {}));
var OpenORPG;
(function (OpenORPG) {
    (function (CommandType) {
        CommandType[CommandType["UnknownCommand"] = 0] = "UnknownCommand";
        CommandType[CommandType["Fps"] = 1] = "Fps";
        CommandType[CommandType["Logout"] = 2] = "Logout";
        CommandType[CommandType["Echo"] = 3] = "Echo";
    })(OpenORPG.CommandType || (OpenORPG.CommandType = {}));
    var CommandType = OpenORPG.CommandType;
})(OpenORPG || (OpenORPG = {}));
var OpenORPG;
(function (OpenORPG) {
    /*
     * The command parser will take a given message and parse it for the expected result.
     * It will return the message type afterwards and then a 'value'. This value can be used
     * to determine the action to be performed.
     *
     * Text that cannot be matched by the parser will be forwarded accordingly.
     */
    var CommandParser = (function () {
        function CommandParser() {
            this.commandLookupTable = {};
            this.generateMessageLookup();
        }
        /*
         * Generates a lookup table so that commands can have their types parsed correctly.
         */
        CommandParser.prototype.generateMessageLookup = function () {
            this.addCommandToLookup("/echo", 3 /* Echo */);
            this.addCommandToLookup("/fps", 1 /* Fps */);
        };
        /*
         * Adds a command by string to the lookup table
         */
        CommandParser.prototype.addCommandToLookup = function (key, command) {
            if (this.commandLookupTable[key])
                throw new Error("A command with the key " + key + " already exists for the command type " + command);
            this.commandLookupTable[key] = command;
        };
        /*
         * Parses a given message string and spits out an enumeration with the proper.
         */
        CommandParser.prototype.parseMessageType = function (message) {
            var key = message.split(' ')[0];
            if (this.commandLookupTable[key])
                return this.commandLookupTable[key];
            return 0 /* UnknownCommand */;
        };
        return CommandParser;
    })();
    OpenORPG.CommandParser = CommandParser;
})(OpenORPG || (OpenORPG = {}));
var OpenORPG;
(function (OpenORPG) {
    /*
     * A representation of a skill on the client side. Contains properties that the client might need to render and view
     * information about.
     */
    var Skill = (function () {
        function Skill(skillTemplate) {
            this.template = skillTemplate;
            this.cooldown = skillTemplate.cooldown;
        }
        Object.defineProperty(Skill.prototype, "id", {
            get: function () {
                return this.template.id;
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(Skill.prototype, "cooldownTime", {
            get: function () {
                return this.template.cooldownTime;
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(Skill.prototype, "name", {
            get: function () {
                return this.template.name;
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(Skill.prototype, "iconId", {
            get: function () {
                return this.template.iconId;
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(Skill.prototype, "description", {
            get: function () {
                return this.template.description;
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(Skill.prototype, "type", {
            get: function () {
                return this.template.type;
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(Skill.prototype, "castTime", {
            get: function () {
                return this.template.castTime;
            },
            enumerable: true,
            configurable: true
        });
        /*
         * Resets the given cooldown timer to the alloted value on the internal representation.
         * This should only be called once the client has acknowledge a server side order. Otherwise,
         * the server will reject the skill request.
         */
        Skill.prototype.resetCooldown = function () {
            this.cooldown = this.cooldownTime;
            Logger.info("Cooldown for skill " + this.id + " has been reset to " + this.cooldown);
        };
        return Skill;
    })();
    OpenORPG.Skill = Skill;
})(OpenORPG || (OpenORPG = {}));
var OpenORPG;
(function (OpenORPG) {
    (function (StatTypes) {
        StatTypes[StatTypes["Hitpoints"] = 0] = "Hitpoints";
        StatTypes[StatTypes["SkillResource"] = 1] = "SkillResource";
        StatTypes[StatTypes["Strength"] = 2] = "Strength";
        StatTypes[StatTypes["Dexterity"] = 3] = "Dexterity";
        StatTypes[StatTypes["Vitality"] = 4] = "Vitality";
        StatTypes[StatTypes["Intelligence"] = 5] = "Intelligence";
        StatTypes[StatTypes["Luck"] = 6] = "Luck";
        StatTypes[StatTypes["Mind"] = 7] = "Mind";
    })(OpenORPG.StatTypes || (OpenORPG.StatTypes = {}));
    var StatTypes = OpenORPG.StatTypes;
})(OpenORPG || (OpenORPG = {}));
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
            // If we were healed, then we want to show a 'healing' color
            if (damage < 0) {
                damage *= -1;
                style.fill = "#00CC00";
            }
            // Be sure to call Phaser constructor properly                        
            _super.call(this, entity.game, 0, 0, damage.toString(), style);
            this.anchor.set(0.5, 0.5);
            this.position.setTo(entity.width / 2, -10);
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
            game.physics.enable(this, Phaser.Physics.ARCADE, true);
            this.body.collideWorldBounds = true;
            // Disable smoothing on the sprite so pixel art looks good.
            // This could be changed if you were using smoother gradient assets
            this.smoothed = false;
            // Input is enabled so we can have subscriptions on entities for systems            
            this.inputEnabled = true;
            // Setup target icon
            this.targetIcon = game.add.sprite(0, -32, "target_icon");
            this.targetIcon.x = this.targetIcon.width / 2;
            this.targetIcon.anchor.set(0, 0.5);
            this.targetIcon.visible = false;
            this.targetIcon.scale.set(0.75, 0.75);
            this.addChild(this.targetIcon);
            // Start the 'bounce' effect
            EffectFactory.bounceSprite(this.targetIcon);
        }
        Entity.prototype.update = function () {
            if (this.interpolator != null)
                this.interpolator.update();
            if (this.nameTagText != null)
                this.nameTagText.position.setTo(this.x + this.width / 2, this.y);
            var directionString = this.directionToString();
            // Use of a skill animate over-takes everything
            if (this.skillAnimation != null) {
                if (this.skillAnimation.isFinished)
                    this.skillAnimation = null;
                return;
            }
            switch (this.characterState) {
                case 0 /* Idle */:
                    // If we're networked and currently interpolating, we will always prefer to play the movement string
                    if (this.interpolator.isIdle())
                        this.playIdle(directionString);
                    else
                        this.playWalk(directionString);
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
            return this.animations.play("idle_" + direction);
        };
        Entity.prototype.playReadyingSkill = function (direction) {
            this.animations.play("atk_" + direction);
        };
        Entity.prototype.playWalk = function (direction) {
            return this.animations.play("walk_" + direction);
        };
        Entity.prototype.playSkillAnimation = function () {
            var direction = this.directionToString();
            this.skillAnimation = this.animations.play("atk_" + direction, 12, false);
        };
        Entity.prototype.initAsNetworkable = function () {
            this.interpolator = new OpenORPG.EntityInterpolator(this);
        };
        Entity.prototype.render = function () {
            if (OpenORPG.Settings.getInstance().debugShowBodies)
                this.game.debug.body(this);
            // Begins drawing a path for an interpolator
            if (OpenORPG.Settings.getInstance().debugShowInterpolationPaths) {
                if (this.interpolator != null)
                    this.interpolator.render();
            }
        };
        Entity.prototype.performSelection = function () {
            this.targetIcon.visible = true;
        };
        Entity.prototype.performDeselection = function () {
            this.targetIcon.visible = false;
        };
        Entity.prototype.destroyNamePlate = function () {
            if (this.nameTagText != null)
                this.nameTagText.destroy();
            this.destroy();
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
                case "characterState":
                    break;
            }
        };
        Entity.prototype.updateName = function (name) {
            if (this.nameTagText == null) {
                this.nameTagText = new Phaser.Text(this.game, 0, 0, name, FontFactory.getPlayerFont());
                this.nameTagText.anchor.set(0.5, 0.5);
                this.game.world.add(this.nameTagText);
            }
            else {
                this.nameTagText.text = name;
            }
            if (this.entityType == "Npc") {
                this.nameTagText.fill = FontFactory.getNpcFont().fill;
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
                if (animKey.indexOf("walk") > -1) {
                    var element = frames.shift();
                    frames.push(element);
                }
                this.animations.add(animKey, frames, 4, true, true);
            }
            // Finished updating sprite
            this.animations.play("idle_down");
            // Setup the body for this sprite
            this.body.setSize(32, 32, this.width / 2 - 16, this.height - 32);
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
    /**
     * Stores a single state of interpolated state from an entity movement from the remote client
     * and or server.
     */
    var InterpolatorState = (function () {
        function InterpolatorState(x, y, dir) {
            this._x = x;
            this._y = y;
            this._dir = dir;
            this._timestamp = new Date().getTime();
        }
        Object.defineProperty(InterpolatorState.prototype, "timestamp", {
            /**
             * Returns the timestamp as of the UNIX epoch from when this state was created.
             */
            get: function () {
                return this._timestamp;
            },
            enumerable: true,
            configurable: true
        });
        return InterpolatorState;
    })();
    /**
     * Provides services for interpolating a remote entity through various states fed into it.
     * The interpolator will automatically keep up with the states fed with it, it is only neccessary
     * to ensure update is called on it and the neccessary properties will be updated.
     */
    var EntityInterpolator = (function () {
        function EntityInterpolator(entity) {
            this._states = new Array();
            /**
             * Represents the amount of time a InterpolatorState will take to be tweened to by default.
             * This value is only the standard -- interpolator states may be consumed faster when running behind.
             */
            this._backTime = 100;
            /**
             * A maximum amount of packets that can be queued up before being forced to releases
               This is roughly 1 * backTime ms behind.
            
               When set to 10, this is a full second behind (which is quite a lot)
            */
            this._maxStateBacklog = 20;
            /**
             * If the distance between the current property value and the next InterpolatorState value is less than this value,
             * then no tween is performed and we snap to this value. This prevents cases where small changes cause jitter.
             */
            this._cutoffThreshold = 1;
            this._entity = entity;
        }
        /**
         * Adds state data to this new EntityInterpolator from a remote network feed.
         */
        EntityInterpolator.prototype.addData = function (x, y, dir) {
            // Add the state the data we have available to us
            var state = new InterpolatorState(x, y, dir);
            this._states.push(state);
            if (this._states.length > this._maxStateBacklog) {
                this.forceFlush();
            }
            this.update();
        };
        // This is only used in debug mode
        EntityInterpolator.prototype.render = function () {
            var _this = this;
            // Render the shapes as required
            this._states.forEach(function (state) {
                _this._entity.game.debug.geom(new Phaser.Circle(state._x, state._y, 10));
            });
        };
        EntityInterpolator.prototype.update = function () {
            var _this = this;
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
                    var state;
                    state = this._states.shift();
                    var tweenData = {
                        x: state._x,
                        y: state._y
                    };
                    var point = new Phaser.Point(tweenData.x, tweenData.y);
                    var point2 = new Phaser.Point(this._entity.x, this._entity.y);
                    this._entity.direction = state._dir;
                    var distance = Phaser.Point.distance(point, point2);
                    // If the distance is small, a teleport is in order and we continue on. This prevents small little hiccups
                    if (distance < this._cutoffThreshold) {
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
                    this._currentMovementTween.onComplete.addOnce(function () {
                        if (_this._currentMovementTween != null) {
                            _this._currentMovementTween.stop();
                            _this._currentMovementTween = null;
                        }
                        _this.update();
                    });
                }
                else {
                    if (this._currentMovementTween != null) {
                        this._currentMovementTween.stop();
                    }
                    this._currentMovementTween = null;
                }
            }
        };
        /**
         * Based on the current backlog, returns a modified time to tween to speed up state consumption.
         * This is generally done to outpace the server sending rate.
         */
        EntityInterpolator.prototype.getInterpolationInterval = function () {
            if (this._states.length < 5)
                return this._backTime;
            if (this._states.length < 6)
                return this._backTime - 20;
            if (this._states.length < 10)
                return this._backTime - 30;
            if (this._states.length < 12)
                return this._backTime - 40;
            return this._backTime;
        };
        EntityInterpolator.prototype.isIdle = function () {
            if (this._currentMovementTween == null)
                return true;
            return false;
        };
        /**
         * Forces a flush of all states. All propertie will be immediately set to the very last state
         * available and the backlog will be cleared.
         */
        EntityInterpolator.prototype.forceFlush = function () {
            if (this._currentMovementTween != null) {
                this._currentMovementTween.stop();
                this._currentMovementTween = null;
            }
            var lastState = this._states[this._states.length - 1];
            this._entity.x = lastState._x;
            this._entity.y = lastState._y;
            this._entity.direction = lastState._dir;
            Logger.warn("A force flush was performed; looks like the network is falling behind.");
            while (this._states.length) {
                var state = this._states.shift();
            }
        };
        /**
         * Resets the interpolator and all state assosciated with it.
         */
        EntityInterpolator.prototype.reset = function () {
            if (this._currentMovementTween != null) {
                this._currentMovementTween.stop();
                this._currentMovementTween = null;
            }
            while (this._states.length) {
                this._states.pop();
            }
        };
        return EntityInterpolator;
    })();
    OpenORPG.EntityInterpolator = EntityInterpolator;
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
        GameSystem.prototype.initZone = function () {
        };
        GameSystem.prototype.render = function () {
        };
        /*
         * This method is invoked when an entity has become added to the current zone for ease of
         * feedback for clients that are subscribing.
         */
        GameSystem.prototype.onEntityAdded = function (entity) {
        };
        /*
         * This method is invoked when an entity has become removed from the current zone for ease of
         * feedback for clients that are subscribing.
         */
        GameSystem.prototype.onEntityRemoved = function (entity) {
        };
        return GameSystem;
    })();
    OpenORPG.GameSystem = GameSystem;
    var CombatSystem = (function (_super) {
        __extends(CombatSystem, _super);
        function CombatSystem(zone, player, playerInfo) {
            var _this = this;
            _super.call(this, zone);
            // Setup our key presses and do sonly once
            this.meleeKey = zone.game.input.keyboard.addKey(Phaser.Keyboard.CONTROL);
            this.meleeKey.onDown.add(this.sendMeleeSkill, this);
            this.interactKey = zone.game.input.keyboard.addKey(Phaser.Keyboard.SHIFT);
            this.interactKey.onDown.add(this.sendInteraction, this);
            this.playerInfo = playerInfo;
            // Setup our network events
            var network = OpenORPG.NetworkManager.getInstance();
            network.registerPacket(18 /* SMSG_SKILL_USE_RESULT */, function (packet) {
                // Play animation on the client
                var user = _this.parent.entities[packet.userId];
                if (user != null) {
                    user.playSkillAnimation();
                    // reset the skill cooldown if required
                    if (user.id == _this.player.id) {
                        var skillId = packet.skillId;
                        var skill = _.find(_this.playerInfo.characterSkills, function (s) {
                            return s.id == skillId;
                        });
                        if (skill)
                            skill.resetCooldown();
                        else
                            Logger.error("Attempted to reset cooldown for skill " + skillId + " but the player did not possess the skill.");
                    }
                }
                // Do stuff to the victim
                var victim = _this.parent.entities[packet.targetId];
                // If this packet is delayed, we might get a null reference
                if (victim != null) {
                    _this.applyToVictim(victim, packet.damage);
                }
                else {
                    Logger.warn("CombatSystem - Attempted to apply skill sequence to victim but no longer existed");
                }
            });
        }
        CombatSystem.prototype.onEntityAdded = function (entity) {
            entity.events.onInputDown.add(this.handleSelection, this);
        };
        CombatSystem.prototype.onEntityRemoved = function (entity) {
            entity.events.onInputDown.remove(this.handleSelection, this);
        };
        /*
         * This method is called when an entity has been selected.
         * The system will handle what will happen.
         */
        CombatSystem.prototype.handleSelection = function (entity) {
            if (this.targetEntity == entity) {
                this.targetEntity.performDeselection();
                this.targetEntity = null;
            }
            else
                this.selectTarget(entity);
        };
        CombatSystem.prototype.selectTarget = function (entity) {
            // Select our new target
            entity.performSelection();
            if (this.targetEntity != null)
                this.targetEntity.performDeselection();
            this.targetEntity = entity;
            var packet = PacketFactory.createTargetNotification(this.targetEntity.id);
            OpenORPG.NetworkManager.getInstance().sendPacket(packet);
        };
        CombatSystem.prototype.applyToVictim = function (victim, damage) {
            var victimDamageText = new OpenORPG.DamageText(victim, damage);
            EffectFactory.pulseDamage(victim);
            // Play hit effect
            var effect = this.parent.game.add.audio("audio_effect_hit", 0.3, false, true);
            effect.play();
        };
        CombatSystem.prototype.attachTo = function (player) {
            this.player = player;
        };
        CombatSystem.prototype.sendMeleeSkill = function () {
            var network = OpenORPG.NetworkManager.getInstance();
            var packet = PacketFactory.createSkillUsePacket(1, -1);
            network.sendPacket(packet);
        };
        CombatSystem.prototype.sendInteraction = function () {
            var network = OpenORPG.NetworkManager.getInstance();
            var packet = PacketFactory.createInteractionRequest();
            network.sendPacket(packet);
        };
        CombatSystem.prototype.destroy = function () {
            this.parent.game.input.keyboard.removeKey(Phaser.Keyboard.CONTROL);
        };
        CombatSystem.prototype.update = function () {
            var _this = this;
            this.playerInfo.characterSkills.forEach(function (skill) {
                skill.cooldown = Math.max(0, skill.cooldown - _this.parent.game.time.elapsed / 1000);
            });
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
            network.registerPacket(46 /* SMSG_ENTITY_TELEPORT */, function (packet) {
                var entity = _this.parent.entities[packet.id];
                // Reset the interpolator, set the position
                entity.interpolator.reset();
                entity.x = packet.position.x;
                entity.y = packet.position.y;
            });
            this.parent.game.input.onDown.add(function () {
                var mouseData = {
                    x: _this.parent.game.input.activePointer.worldX,
                    y: _this.parent.game.input.activePointer.worldY
                };
                // Something was clicked, check if ALT was hit and teleport if needed
                if (_this.parent.game.input.keyboard.isDown(Phaser.Keyboard.ALT)) {
                    var packet = PacketFactory.createTeleportRequest(mouseData.x, mouseData.y);
                    OpenORPG.NetworkManager.getInstance().sendPacket(packet);
                }
            }, this);
            this.timerToken = setInterval(this.generateMovementTicket, MovementSystem.MOVEMENT_TICKET_FREQUENCY);
        }
        MovementSystem.prototype.initZone = function () {
            // Setup our zone trigger if it is at all possible
            this.topZoneTrigger = new OpenORPG.ZoneTrigger(this.parent.game, 0, 0, this.parent.tileMap.widthInPixels, this.parent.tileMap.tileHeight, this.topCallback.bind(this), this.parent);
            this.bottomZoneTrigger = new OpenORPG.ZoneTrigger(this.parent.game, 0, this.parent.tileMap.heightInPixels - 32, this.parent.tileMap.widthInPixels, this.parent.tileMap.tileHeight, this.bottomCallback.bind(this), this.parent);
            this.leftZoneTrigger = new OpenORPG.ZoneTrigger(this.parent.game, 0, 0, this.parent.tileMap.tileWidth, this.parent.tileMap.heightInPixels, this.leftCallback.bind(this), this.parent);
            this.rightZoneTrigger = new OpenORPG.ZoneTrigger(this.parent.game, this.parent.tileMap.widthInPixels - 32, 0, this.parent.tileMap.tileWidth, this.parent.tileMap.heightInPixels, this.rightCallback.bind(this), this.parent);
        };
        MovementSystem.prototype.render = function () {
            if (this.player != null) {
            }
        };
        MovementSystem.prototype.topCallback = function () {
            // We make sure to force a movement update before all of these
            MovementSystem.current.generateMovementTicket(true);
            this.sendZoneChangeRequest(2 /* North */);
        };
        MovementSystem.prototype.bottomCallback = function () {
            // We make sure to force a movement update before all of these
            MovementSystem.current.generateMovementTicket(true);
            var network = OpenORPG.NetworkManager.getInstance();
            this.sendZoneChangeRequest(0 /* South */);
        };
        MovementSystem.prototype.rightCallback = function () {
            // We make sure to force a movement update before all of these
            MovementSystem.current.generateMovementTicket(true);
            this.sendZoneChangeRequest(3 /* East */);
        };
        MovementSystem.prototype.leftCallback = function () {
            // We make sure to force a movement update before all of these
            MovementSystem.current.generateMovementTicket(true);
            this.sendZoneChangeRequest(1 /* West */);
        };
        MovementSystem.prototype.sendZoneChangeRequest = function (direction) {
            var network = OpenORPG.NetworkManager.getInstance();
            network.sendPacket(PacketFactory.createZoneRequestChange(direction));
        };
        MovementSystem.prototype.update = function () {
            // If we're not viewing anything, quit
            if (this.player == null)
                return;
            // This chunk of code is used to control the player physics body around the map
            if (this.player.characterState == 1 /* Moving */ || this.player.characterState == 0 /* Idle */) {
                var speed = 120;
                var velocity = new Phaser.Point(0, 0);
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
                    this.generateMovementTicket(true);
                    this.player.body.velocity.setTo(0, 0);
                }
                if (!velocity.isZero()) {
                    this.player.body.velocity.setTo(velocity.x, velocity.y);
                    if (velocity.x < 0)
                        this.player.direction = 1 /* West */;
                    else if (velocity.x > 0)
                        this.player.direction = 3 /* East */;
                    if (velocity.y < 0)
                        this.player.direction = 2 /* North */;
                    else if (velocity.y > 0)
                        this.player.direction = 0 /* South */;
                    this.player.playWalk(this.player.directionToString());
                }
            } // end movement controller
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
            if (entity) {
                // Set a direction
                entity.interpolator.addData(position.x, position.y, packet.direction);
            }
            else {
                Logger.warn("MovementSystem - Attempted to move and tween a non-existant entity. It probably moved zones.");
            }
        };
        MovementSystem.prototype.generateMovementTicket = function (override) {
            if (override === void 0) { override = false; }
            if (MovementSystem.current.player != null) {
                if (!MovementSystem.current.player.body.velocity.isZero()) {
                    var packet = PacketFactory.createMovementPacket(MovementSystem.current.player.x, MovementSystem.current.player.y, override, MovementSystem.current.player.direction);
                    OpenORPG.NetworkManager.getInstance().sendPacket(packet);
                }
            }
        };
        MovementSystem.MOVEMENT_TICKET_FREQUENCY = 100;
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
    function createTeleportRequest(px, py) {
        return {
            opCode: 45 /* CMSG_CLICK_WARP_REQUEST */,
            x: Math.round(px),
            y: Math.round(py)
        };
    }
    PacketFactory.createTeleportRequest = createTeleportRequest;
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
    function createInteractionRequest() {
        return {
            opCode: 25 /* CMSG_INTERACT_REQUEST */,
            data: "Something to fill up space"
        };
    }
    PacketFactory.createInteractionRequest = createInteractionRequest;
    function createChatPacket(channeld, message) {
        return {
            opCode: 12 /* CMSG_CHAT_MESSAGE */,
            channelId: channeld,
            message: message
        };
    }
    PacketFactory.createChatPacket = createChatPacket;
    function createStorageMoveRequest(source, dest, stype) {
        return {
            opCode: 31 /* CMSG_STORAGE_MOVE_SLOT */,
            sourceSlot: source,
            destSlot: dest,
            type: stype
        };
    }
    PacketFactory.createStorageMoveRequest = createStorageMoveRequest;
    function createStorageDropRequest(slotId, amount) {
        return {
            opCode: 33 /* CMSG_STORAGE_DROP */,
            slotId: slotId,
            amount: amount
        };
    }
    PacketFactory.createStorageDropRequest = createStorageDropRequest;
    function createItemuseRequest(slotId) {
        return {
            opCode: 23 /* CMSG_ITEM_USE */,
            slotId: slotId
        };
    }
    PacketFactory.createItemuseRequest = createItemuseRequest;
    function createUnEqupRequest(slot) {
        return {
            opCode: 35 /* CMSG_UNEQUIP_ITEM */,
            slot: slot
        };
    }
    PacketFactory.createUnEqupRequest = createUnEqupRequest;
    function createQuestAcceptRequest(questId) {
        return {
            opCode: 27 /* CMSG_QUEST_ACCEPT */,
            questId: questId
        };
    }
    PacketFactory.createQuestAcceptRequest = createQuestAcceptRequest;
    function createTargetNotification(targetId) {
        return {
            opCode: 40 /* CMSG_ENTITY_TARGET */,
            entityId: targetId
        };
    }
    PacketFactory.createTargetNotification = createTargetNotification;
    function createGameLoadedPacket() {
        return {
            opCode: 41 /* CMSG_GAME_LOADED */,
            payload: "OK"
        };
    }
    PacketFactory.createGameLoadedPacket = createGameLoadedPacket;
    function createDialogLink(index) {
        return {
            opCode: 44 /* CMSG_DIALOG_LINK_SELECTION */,
            linkId: index
        };
    }
    PacketFactory.createDialogLink = createDialogLink;
})(PacketFactory || (PacketFactory = {}));
var OpenORPG;
(function (OpenORPG) {
    /*
     * A simple container class that has the job of simply containing amounts of information.
     * A simple flyweight class that can be passed around to tie in various pieces of info and
     * prevent duplication. Contains information like
     *
     */
    var PlayerInfo = (function () {
        function PlayerInfo() {
            // A small interface to character stats
            this.characterStats = new Array();
            this.characterSkills = new Array();
            this.characterStatsCallbacks = new Array();
            this.inventoryCallbacks = [];
            var that = this;
        }
        return PlayerInfo;
    })();
    OpenORPG.PlayerInfo = PlayerInfo;
})(OpenORPG || (OpenORPG = {}));
var OpenORPG;
(function (OpenORPG) {
    var PlayerInfoMontior = (function () {
        // Setup some network events here and be done with it
        function PlayerInfoMontior(playerInfo) {
            var _this = this;
            this.playerInfo = playerInfo;
            var that = this;
            var network = OpenORPG.NetworkManager.getInstance();
            setInterval(function () {
                AngularInterop.updateAngularScope();
            }, 1000);
            // Register for stat changes on ourselves, and apply them
            network.registerPacket(37 /* SMSG_STAT_CHANGE */, function (packet) {
                if (_this.playerInfo.player.id == packet.characterId) {
                    _this.playerInfo.characterStats[packet.stat].currentValue = packet.currentValue;
                    _this.playerInfo.characterStats[packet.stat].maximumValue = packet.maximumValue;
                    AngularInterop.updateAngularScope();
                }
            });
            // Hook into our network events
            network.registerPacket(32 /* SMSG_STORAGE_HERO_SEND */, function (packet) {
                _this.playerInfo.inventory = [];
                _this.playerInfo.inventory.push.apply(_this.playerInfo.inventory, packet.itemStorage);
                AngularInterop.updateAngularScope();
                Logger.info("PlayerInfoMonitor - The player inventory has been updated.");
            });
            network.registerPacket(39 /* SMSG_QUEST_SEND_LIST */, function (packet) {
                _this.playerInfo.quests = [];
                Logger.debug("PlayerInfoMonitor - Dumping new quest data...");
                Logger.debug(packet.questLog);
                _.each(packet.questLog, function (value) {
                    OpenORPG.ContentManager.getInstance().getContent(2 /* Quest */, value.quest.questId, function (data) {
                        // Copy the state over for usage
                        data.state = value.state;
                        data.currentStep = value.currentStep;
                        data.questInfo = {};
                        // Push up our progress levels to accordingly
                        _.each(value.progress, function (progressLevel, index) {
                            data.questInfo.requirementProgress = [];
                            data.questInfo.requirementProgress.push({ progress: progressLevel });
                        });
                        _this.playerInfo.quests.push(data);
                        AngularInterop.broadcastEvent('QuestsChanged');
                        AngularInterop.updateAngularScope();
                    });
                });
            });
            // An update for quest progress stuff
            network.registerPacket(42 /* SMSG_QUEST_PROGRESS_UPDATE */, function (packet) {
                var updatedQuest = _.find(_this.playerInfo.quests, function (quest) {
                    return quest.id == packet.questId;
                });
                // Update the progress indiciator; trigger a UI refresh
                updatedQuest.questInfo.requirementProgress[packet.requirementIndex].progress = packet.progress;
                AngularInterop.broadcastEvent('QuestsChanged');
                AngularInterop.updateAngularScope();
            });
            // skills monitoring
            // Listen to events about player information we might care about
            network.registerPacket(38 /* SMSG_SKILL_CHANGE */, function (packet) {
                _this.playerInfo.characterSkills = [];
                for (var key in packet.skills) {
                    var skill = packet.skills[key];
                    OpenORPG.ContentManager.getInstance().getContent(3 /* Skill */, (parseInt(key) + 1), function (fSkill) {
                        fSkill.cooldown = skill.cooldown;
                        _this.playerInfo.characterSkills.push(new OpenORPG.Skill(fSkill));
                        // Add some logging
                        Logger.info("PlayerInfoMonitor - Player skills have been updated.");
                        Logger.info(_this.playerInfo.characterSkills);
                    });
                }
            });
        }
        return PlayerInfoMontior;
    })();
    OpenORPG.PlayerInfoMontior = PlayerInfoMontior;
})(OpenORPG || (OpenORPG = {}));
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
/// <reference path="../../phaser.d.ts" />
var OpenORPG;
(function (OpenORPG) {
    var AbstractState = (function (_super) {
        __extends(AbstractState, _super);
        function AbstractState() {
            _super.apply(this, arguments);
        }
        Object.defineProperty(AbstractState.prototype, "centerX", {
            get: function () {
                return this.game.scale.bounds.centerX;
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(AbstractState.prototype, "centerY", {
            get: function () {
                return this.game.scale.bounds.centerY;
            },
            enumerable: true,
            configurable: true
        });
        return AbstractState;
    })(Phaser.State);
    OpenORPG.AbstractState = AbstractState;
})(OpenORPG || (OpenORPG = {}));
/// <reference path="./AbstractState.ts" />
var OpenORPG;
(function (OpenORPG) {
    var BootState = (function (_super) {
        __extends(BootState, _super);
        function BootState() {
            _super.apply(this, arguments);
        }
        BootState.prototype.create = function () {
            var _this = this;
            Logger.info("Booting the game...");
            this.game.stage.disableVisibilityChange = true;
            this.game.scale.setGameSize(window.innerWidth, window.innerHeight);
            /* Setup the connecting splash screen */
            var connectingSplash = this.game.add.sprite(this.centerX, this.centerY, "connecting", 0);
            connectingSplash.anchor.setTo(0.5, 0.5);
            /* Set up the network object. */
            var network = OpenORPG.NetworkManager.getInstance();
            network.onConnectionCallback = function () {
                _this.game.state.add("mainmenu", new OpenORPG.LoginMenuState());
                _this.game.state.start("mainmenu");
            };
            network.onConnectionErrorCallback = function () {
                _this.game.state.start("errorstate");
            };
            /* Connect to the server */
            Logger.info("Connecting to the server...");
            network.connect();
        };
        BootState.prototype.preload = function () {
            //TODO: Do some of the sprite loading we might want to do here
            this.game.load.image("connecting", "assets/ui/connecting.png");
        };
        return BootState;
    })(OpenORPG.AbstractState);
    OpenORPG.BootState = BootState;
})(OpenORPG || (OpenORPG = {}));
var OpenORPG;
(function (OpenORPG) {
    var Game = (function () {
        function Game() {
            var windowWidth = window.innerWidth;
            var windowHeight = window.innerHeight;
            var width = windowWidth; //Math.round(Math.min(windowWidth, windowHeight * 16.0 / 9.0));
            var height = windowHeight; //Math.round(Math.min(windowHeight, width * 9.0 / 16.0));
            //var canvasHolder = document.getElementById("canvasholder");
            this.game = new Phaser.Game(width, height, this.pickRenderer(), 'canvasholder', this, true, false);
            window.onresize = function (evt) {
                this.game.camera.setSize(window.innerWidth, window.innerHeight);
                this.game.scale.setGameSize(window.innerWidth, window.innerHeight);
                this.game.scale.refresh();
                console.log(this.game.scale);
                console.log(this.game.camera);
                this.game.scale.onSizeChange.dispatch();
            }.bind(this);
            this.game.state.add("boot", new OpenORPG.BootState(), false);
        }
        Game.prototype.pickRenderer = function () {
            if (OpenORPG.Settings.getInstance().debugForceWebGl)
                return Phaser.WEBGL;
            return Phaser.CANVAS;
        };
        Game.prototype.aspectRatio = function () {
            return (window.innerWidth / window.innerHeight);
        };
        Game.prototype.preload = function () {
        };
        Game.prototype.create = function () {
            // Setup the game manager here, respond to changes in settings across the global phaser network
            var settings = OpenORPG.Settings.getInstance();
            settings.onChange(this.updateSettings, this);
            settings.flush();
            this.game.state.start("boot");
        };
        Game.prototype.updateSettings = function () {
            var settings = OpenORPG.Settings.getInstance();
            this.game.sound.mute = !settings.playBGM;
            // Phaser uses a range between 0 and 1 for audio, we need to scale it down
            this.game.sound.volume = settings.volume / 100;
        };
        return Game;
    })();
    OpenORPG.Game = Game;
})(OpenORPG || (OpenORPG = {}));
/// <reference path="./AbstractState.ts" />
var OpenORPG;
(function (OpenORPG) {
    // The gameplay state manages 
    var GameplayState = (function (_super) {
        __extends(GameplayState, _super);
        function GameplayState() {
            _super.call(this);
            this.zone = null;
            this.questWindow = new OpenORPG.QuestWindow();
            this.dialogWindow = new OpenORPG.DialogWindow();
            // Keep track of character info
            this.playerInfo = new OpenORPG.PlayerInfo();
            Logger.trace("GameplayState - Creating object and setting up handlers");
            var that = this;
            var $body = angular.element(document.body); // 1
            var $rootScope = $body.scope();
            $rootScope = $rootScope.$root;
            $rootScope.$apply(function () {
                $rootScope.playerInfo = that.playerInfo;
            });
            this.playerMonitor = new OpenORPG.PlayerInfoMontior(this.playerInfo);
            this.chatManager = new OpenORPG.ChatManager();
            this.characterHud = new OpenORPG.CharacterStatusWidget($("#canvasholder"), this.playerInfo);
            this.bottomBarWidget = new OpenORPG.BottombarWidget($("#canvasholder"));
            this.chatWidget = new OpenORPG.ChatWidget($("#canvasholder"));
            this.menuWidget = new OpenORPG.MenuTrayWidget($("#canvasholder"), this.playerInfo);
        }
        GameplayState.prototype.preload = function () {
            SpriteManager.loadSpriteImages(this.game);
            // Load up everything else
            var loader = this.game.load;
            loader.tilemap("map_1", "assets/Maps/1.json", null, Phaser.Tilemap.TILED_JSON);
            loader.tilemap("map_2", "assets/Maps/2.json", null, Phaser.Tilemap.TILED_JSON);
            loader.tilemap("map_3", "assets/Maps/3.json", null, Phaser.Tilemap.TILED_JSON);
            loader.image("tilesheet", "assets/Maps/tilesheet_16.png");
            loader.image("target_icon", "assets/img/target_selector.png");
            // Load all our audio
            loader.audio("audio_music_town", [DirectoryHelper.getMusicPath() + "town.ogg"]);
            loader.audio("audio_effect_hit", [DirectoryHelper.getAudioEffectPath() + "hit1.ogg"]);
        };
        GameplayState.prototype.render = function () {
            if (this.zone != null)
                this.zone.render();
        };
        GameplayState.prototype.create = function () {
            var _this = this;
            // Start our physics systems
            this.game.physics.startSystem(Phaser.Physics.ARCADE);
            this.zone = new OpenORPG.Zone(this.game, this.playerInfo);
            var network = OpenORPG.NetworkManager.getInstance();
            network.registerPacket(8 /* SMSG_ZONE_CHANGED */, function (packet) {
                _this.zone.clearZone();
                // Stop current music and all sound effects
                _this.game.sound.stopAll();
                // Load new audio track in
                _this.currenTrack = _this.game.add.audio("audio_music_town", 0.5, true, true);
                _this.currenTrack.play();
                _this.zone.initLocalZone(packet.zoneId);
                for (var entityKey in packet.entities) {
                    var entity = packet.entities[entityKey];
                    // Create your objects here 
                    var worldEntity = _this.zone.addNetworkEntityToZone(entity);
                    // Do camera following here
                    if (worldEntity.id == packet.heroId) {
                        _this.game.camera.follow(worldEntity);
                        _this.zone.movementSystem.attachEntity(worldEntity);
                        _this.zone.combatSystem.attachTo(worldEntity);
                        _this.playerInfo.player = worldEntity;
                        for (var key in entity.characterStats.stats) {
                            var statObject = entity.characterStats.stats[key];
                            _this.playerInfo.characterStats[statObject.statType] = { currentValue: statObject.currentValue, maximumValue: statObject.maximumValue };
                        }
                        _this.playerInfo.name = worldEntity.name;
                        var $body = angular.element(document.body); // 1
                        var $rootScope = $body.scope();
                        $rootScope = $rootScope.$root;
                        $rootScope.$apply();
                    }
                }
            });
            network.registerPacket(30 /* SMSG_SERVER_OFFER_QUEST */, function (packet) {
                _this.questWindow.presentQuest(packet.questId);
            });
            network.registerPacket(43 /* SMSG_DIALOG_PRESENT */, function (packet) {
                _this.dialogWindow.presentDialog(packet.message, packet.links);
            });
            // Ready! Tell the server to bring it on
            network.sendPacket(PacketFactory.createGameLoadedPacket());
        };
        GameplayState.prototype.update = function () {
            if (this.zone != null)
                this.zone.update();
        };
        return GameplayState;
    })(OpenORPG.AbstractState);
    OpenORPG.GameplayState = GameplayState;
})(OpenORPG || (OpenORPG = {}));
/// <reference path="./AbstractState.ts" />
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
    })(OpenORPG.AbstractState);
    OpenORPG.HeroSelectState = HeroSelectState;
})(OpenORPG || (OpenORPG = {}));
/**
 * DOCTODO
 *
 * @preferred
 */
var OpenORPG;
(function (OpenORPG) {
    var UI;
    (function (UI) {
        /**
         * DOCTODO
         */
        var Element = (function () {
            /**
             * DOCTODO
             */
            function Element(parent, element) {
                if (parent instanceof Element) {
                    /* If we were passed an actual UIElement,
                     * our registered parent is the JQuery
                     * element of our parent. */
                    this._parent = parent.element;
                }
                else {
                    /* Otherwise this must just be our JQuery parent. */
                    this._parent = parent;
                }
                if (typeof (element) !== "string") {
                    /* If element is not a string it's a JQuery element */
                    this._element = element;
                    /* Let's initialize. */
                    this.initialize();
                }
                else {
                    /* Otherwise if it is a string it's a selector or source file */
                    var selection = null;
                    try {
                        selection = parent == null ? $(element) : parent.find(element); /* Or look at descendants. */
                    }
                    catch (e) {
                        /* Definitely not a selector */
                        selection = { length: 0 };
                    }
                    /* If it was a valid selector length will be non-zero. */
                    if (selection.length > 0) {
                        this._element = selection.first();
                        /* Let's initialize. */
                        this.initialize();
                    }
                    else {
                        /* Assume that it's a source file */
                        var that = this;
                        /* We can instance our template path here
                         * to generate our DOM element */
                        $.get(element, function (data) {
                            /* Since we should be getting by id we only care
                             * about the first element, since there shouldn't
                             * be any others. */
                            that._element = $(data).first();
                            /* Before trying to just add the element to the parent,
                             * we need to make sure that there is a parent. */
                            if (that._parent != null) {
                                $(that.parent).append(that.element);
                            }
                            else {
                                /* If we don't have a parent, let's set it. */
                                that._parent = that.element.parent();
                            }
                            /* VAUGHAN DOCTODO: Fill in this comment. */
                            angular.element(document).injector().invoke(function ($compile) {
                                var container = that.element;
                                var scope = angular.element(container).scope();
                                $compile(container)(scope);
                                scope.$apply();
                            });
                            /* Since we finished loading from the source file,
                             * let's initialize. */
                            that.initialize();
                        });
                    }
                }
            }
            /**
             * Used to initialize the element, retrieve values that it caches
             * and to bind events. Sub-classes that want to do initialization
             * work should have an override for the onLoad(JQueryEventObject)
             * method, which is called at the end of this method's execution.
             */
            Element.prototype.initialize = function () {
                this.refresh();
                /* Bind the most important events first */
                this.element.load(this.onLoad);
                this.element.unload(this.onUnload);
                /* Bind all of the other events */
                this.element.blur(this.onBlur);
                this.element.click(this.onClick);
                this.element.change(this.onChange);
                this.element.dblclick(this.onDoubleClick);
                this.element.focus(this.onFocus);
                this.element.keydown(this.onKeyDown);
                this.element.keypress(this.onKeyPress);
                this.element.keyup(this.onKeyUp);
                this.element.mousedown(this.onMouseDown);
                this.element.mousemove(this.onMouseMove);
                this.element.mouseout(this.onMouseOut);
                this.element.mouseover(this.onMouseOver);
                this.element.mouseup(this.onMouseUp);
                this.element.resize(this.onResize);
                this.element.scroll(this.onScroll);
                this.element.select(this.onSelect);
                this.element.submit(this.onSubmit);
                this.onLoad(null);
            };
            /* ================================================================ *
             *                           Cache methods                          *
             * ================================================================ */
            /**
             * Flushes properties that have not been updated to the DOM.
             */
            Element.prototype.flush = function () {
                this.flushAlign();
            };
            /**
             * Loads properties that need to be cached from the DOM and sets
             * the cache values accordingly.
             */
            Element.prototype.refresh = function () {
                this._align = OpenORPG.getAlign(this.css("text-align"), this.css("vertical-align"));
            };
            /* ================================================================ *
             *                           Flush methods                          *
             * These should only be created and used for multi-value flushes.   *
             * ================================================================ */
            /**
             * Sets the Element's alignment CSS properties with the current value.
             */
            Element.prototype.flushAlign = function () {
                this.css("text-align", this.halign);
                this.css("vertical-align", this.valign);
            };
            /* ================================================================ *
             *                              Events                              *
             * Some of the events here are standard events, some are jQuery     *
             * specific. Note that any event that is explicitly marked as being *
             * non-standard should not be added. If it's present as jQuery but  *
             * not marked non-standard, it is fine to include it.               *
             * To determine whether it is explicitly non-standard, refer to:    *
             * https://developer.mozilla.org/en-US/docs/Web/API/HTMLElement     *
             * ================================================================ */
            /**
             * Called when the jQuery blur() event is invoked.
             *
             * @param event an object containing the event information
             */
            Element.prototype.onBlur = function (event) {
            };
            /**
             * Called when the jQuery change() event is invoked.
             *
             * @param event an object containing the event information
             */
            Element.prototype.onChange = function (event) {
            };
            /**
             * Called when the jQuery click() event is invoked.
             *
             * @param event an object containing the event information
             */
            Element.prototype.onClick = function (event) {
            };
            /**
             * Called when the jQuery dblclick() event is invoked.
             *
             * @param event an object containing the event information
             */
            Element.prototype.onDoubleClick = function (event) {
            };
            /**
             * Called when the jQuery focus() event is invoked.
             *
             * @param event an object containing the event information
             */
            Element.prototype.onFocus = function (event) {
            };
            /**
             * Called when the jQuery keydown() event is invoked.
             *
             * @param event an object containing the event information
             */
            Element.prototype.onKeyDown = function (event) {
            };
            /**
             * Called when the jQuery keypress() event is invoked.
             *
             * @param event an object containing the event information
             */
            Element.prototype.onKeyPress = function (event) {
            };
            /**
             * Called when the jQuery keyup() event is invoked.
             *
             * @param event an object containing the event information
             */
            Element.prototype.onKeyUp = function (event) {
            };
            /**
             * Called when the jQuery load() event is invoked, or
             * in the Element.initialize() method, which is invoked
             * by the Element constructor.
             *
             * @param event an object containing the event information
             */
            Element.prototype.onLoad = function (event) {
            };
            /**
             * Called when the jQuery mosuedown() event is invoked.
             *
             * @param event an object containing the event information
             */
            Element.prototype.onMouseDown = function (event) {
            };
            /**
             * Called when the jQuery mousemove() event is invoked.
             *
             * @param event an object containing the event information
             */
            Element.prototype.onMouseMove = function (event) {
            };
            /**
             * Called when the jQuery mouseout() event is invoked.
             *
             * @param event an object containing the event information
             */
            Element.prototype.onMouseOut = function (event) {
            };
            /**
             * Called when the jQuery mouseover() event is invoked.
             *
             * @param event an object containing the event information
             */
            Element.prototype.onMouseOver = function (event) {
            };
            /**
             * Called when the jQuery mouseup() event is invoked.
             *
             * @param event an object containing the event information
             */
            Element.prototype.onMouseUp = function (event) {
            };
            /**
             * Called when the jQuery resize() event is invoked.
             *
             * @param event an object containing the event information
             */
            Element.prototype.onResize = function (event) {
            };
            /**
             * Called when the jQuery scroll() event is invoked.
             *
             * @param event an object containing the event information
             */
            Element.prototype.onScroll = function (event) {
            };
            /**
             * Called when the jQuery select() event is invoked.
             *
             * @param event an object containing the event information
             */
            Element.prototype.onSelect = function (event) {
            };
            /**
             * Called when the jQuery submit() event is invoked.
             *
             * @param event an object containing the event information
             */
            Element.prototype.onSubmit = function (event) {
            };
            /**
             * Called when the jQuery unload() event is invoked.
             *
             * @param event an object containing the event information
             */
            Element.prototype.onUnload = function (event) {
            };
            /* Functions */
            /**
             * Shows the Element.
             */
            Element.prototype.show = function () {
                this._element.show();
            };
            /**
             * Hides the Element.
             */
            Element.prototype.hide = function () {
                this._element.hide();
            };
            /**
             * Used to interact with the jQuery.css() method group.
             */
            Element.prototype.css = function (propertyNameOrProperties, value) {
                if (value === void 0) { value = null; }
                /* If we have an Object of properties
                 * let's return the JQuery object. */
                if (propertyNameOrProperties instanceof Object) {
                    return this._element.css(propertyNameOrProperties);
                }
                /* Otherwise if we're just getting, return a string */
                if (value == null) {
                    return this._element.css(propertyNameOrProperties);
                }
                /* Otherwise we are returning a JQuery object. */
                return this._element.css(propertyNameOrProperties, value);
            };
            Object.defineProperty(Element.prototype, "parent", {
                /**
                 * Getts for the JQuery object for the parent element. This does
                 * not return a parent Element object, as Element objects are
                 * merely meant to serve as references to HTML elements, and
                 * has no heiarchy of its own.
                 *
                 * @returns the JQuery object for the parent element
                 */
                get: function () {
                    return this._parent;
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Element.prototype, "element", {
                /**
                 * Getter for the JQuery object for the element.
                 *
                 * @returns the JQuery object for the element
                 */
                get: function () {
                    return this._element;
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Element.prototype, "align", {
                /**
                 * Getter for the alignment of contents for this the element.
                 *
                 * @returns an Align enum
                 */
                get: function () {
                    return this._align;
                },
                /**
                 * Setter for this Elements content alignment. This affects
                 * the positioning of the contents of this element, and does
                 * not affect the position of the element itself.
                 *
                 * @param align the alignment value to set
                 */
                set: function (align) {
                    this._align = align;
                    this.flushAlign();
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Element.prototype, "halign", {
                /**
                 * Getter for the CSS horizontal alignment value, based on
                 * the current value from the `align` getter. The return
                 * values are valid values for the CSS `text-align` property.
                 *
                 * @returns the horizontal alignment CSS value
                 */
                get: function () {
                    return OpenORPG.getHAlignName(this._align);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Element.prototype, "valign", {
                /**
                 * Getter for the CSS alignment value, based on
                 * the current value from the `align` getter.
                 * The return values are valid values for the
                 * CSS `vertical-align` property.
                 *
                 * @returns the vertical alignment CSS value
                 */
                get: function () {
                    return OpenORPG.getVAlignName(this._align);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Element.prototype, "backgroundColor", {
                /**
                 * DOCTODO
                 *
                 * @returns
                 */
                get: function () {
                    return this.css("background-color");
                },
                /**
                 * DOCTODO
                 *
                 * @param value
                 */
                set: function (value) {
                    this.css("background-color", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Element.prototype, "textColor", {
                /**
                 * DOCTODO
                 *
                 * @returns
                 */
                get: function () {
                    return this.css("color");
                },
                /**
                 * DOCTODO
                 *
                 * @param value
                 */
                set: function (value) {
                    this.css("color", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Element.prototype, "textSize", {
                /**
                 * DOCTODO
                 *
                 * @returns
                 */
                get: function () {
                    return this.css("font-size");
                },
                /**
                 * DOCTODO
                 *
                 * @param value
                 */
                set: function (value) {
                    this.css("font-size", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Element.prototype, "font", {
                /**
                 * DOCTODO
                 *
                 * @returns
                 */
                get: function () {
                    return this.css("font-family");
                },
                /**
                 * DOCTODO
                 *
                 * @param value
                 */
                set: function (value) {
                    this.css("font-family", value);
                },
                enumerable: true,
                configurable: true
            });
            return Element;
        })();
        UI.Element = Element;
    })(UI = OpenORPG.UI || (OpenORPG.UI = {}));
})(OpenORPG || (OpenORPG = {}));
///<reference path="Element.ts" />
var OpenORPG;
(function (OpenORPG) {
    var UI;
    (function (UI) {
        /**
         * DOCTODO
         */
        var Panel = (function (_super) {
            __extends(Panel, _super);
            function Panel() {
                _super.apply(this, arguments);
            }
            return Panel;
        })(UI.Element);
        UI.Panel = Panel;
    })(UI = OpenORPG.UI || (OpenORPG.UI = {}));
})(OpenORPG || (OpenORPG = {}));
/// <reference path="./AbstractState.ts" />
/// <reference path="../UI/Panel.ts" />
var OpenORPG;
(function (OpenORPG) {
    var LoginMenuState = (function (_super) {
        __extends(LoginMenuState, _super);
        function LoginMenuState() {
            _super.call(this);
            OpenORPG.Angular.Game.module.controller('ControllerPanelLogin', [
                '$scope',
                '$rootScope',
                function ($scope, $rootScope) {
                    $scope.settings = $.extend({}, OpenORPG.Settings.getInstance());
                    $scope.rememberUsername = function (event) {
                        LoginMenuState.instance.loginPanel.refreshCheckboxes(true, false);
                    };
                    /* IDEA: Create controller object model with root interface to be added to each game state */
                    $scope.rememberPassword = function (event) {
                        LoginMenuState.instance.loginPanel.refreshCheckboxes(false, true);
                    };
                    $scope.login = function () {
                        LoginMenuState.instance.loginPanel.updateSettings();
                        LoginMenuState.instance.login();
                    };
                    $scope.register = function () {
                    };
                }
            ]);
            OpenORPG.Angular.Game.register();
            LoginMenuState._instance = this;
            this.loginPanel = new LoginPanel($("#canvasholder"));
        }
        Object.defineProperty(LoginMenuState, "instance", {
            get: function () {
                return this._instance;
            },
            enumerable: true,
            configurable: true
        });
        LoginMenuState.prototype.create = function () {
            var _this = this;
            var network = OpenORPG.NetworkManager.getInstance();
            network.registerPacket(1 /* SMSG_LOGIN_RESPONSE */, function (packet) {
                if (packet.status == 1) {
                    _this.game.state.add("heroselect", new OpenORPG.HeroSelectState());
                    _this.game.state.start("heroselect");
                }
            });
            if (OpenORPG.Settings.getInstance().autoLoginSet) {
                this.login();
            }
        };
        LoginMenuState.prototype.login = function () {
            /*//TODO: Get query parameters working
            var options = this.game.net.getQueryString("username");*/
            var loginPacket = PacketFactory.createLoginPacket(OpenORPG.Settings.getInstance().savedUsername, OpenORPG.Settings.getInstance().savedPassword);
            OpenORPG.NetworkManager.getInstance().sendPacket(loginPacket);
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
        LoginMenuState.prototype.shutdown = function () {
            this.loginPanel.hide();
        };
        return LoginMenuState;
    })(OpenORPG.AbstractState);
    OpenORPG.LoginMenuState = LoginMenuState;
    /* TODO: Refactor stuff when done writing more UI code */
    var LoginPanel = (function (_super) {
        __extends(LoginPanel, _super);
        function LoginPanel(parent) {
            _super.call(this, parent, "assets/states/login.html");
        }
        LoginPanel.prototype.onLoad = function (event) {
            _super.prototype.onLoad.call(this, event);
            var that = this;
            this._rememberUser = new OpenORPG.UI.Checkbox(this.element, "#remember-user");
            this._rememberPass = new OpenORPG.UI.Checkbox(this.element, "#remember-pass");
            this._username = new OpenORPG.UI.Element(this.element, "#login-username");
            if (OpenORPG.Settings.getInstance().saveUsername) {
                this._username.element.val(OpenORPG.Settings.getInstance().savedUsername);
            }
            this._password = new OpenORPG.UI.Element(this.element, "#login-password");
            if (OpenORPG.Settings.getInstance().savePassword) {
                this._password.element.val(OpenORPG.Settings.getInstance().savedPassword);
            }
            this.refreshCheckboxes();
        };
        LoginPanel.prototype.updateSettings = function () {
            OpenORPG.Settings.getInstance().savedUsername = null;
            OpenORPG.Settings.getInstance().savedPassword = null;
            if (OpenORPG.Settings.getInstance().saveUsername) {
                OpenORPG.Settings.getInstance().savedUsername = this._username.element.val();
            }
            if (OpenORPG.Settings.getInstance().savePassword) {
                OpenORPG.Settings.getInstance().savedPassword = this._password.element.val();
            }
            OpenORPG.Settings.getInstance().flush();
            OpenORPG.Settings.getInstance().save();
        };
        LoginPanel.prototype.refreshCheckboxes = function (toggleUser, togglePass) {
            if (toggleUser === void 0) { toggleUser = false; }
            if (togglePass === void 0) { togglePass = false; }
            if (toggleUser) {
                OpenORPG.Settings.getInstance().saveUsername = !OpenORPG.Settings.getInstance().saveUsername;
                /* This makes it so toggling the username saving off while
                password saving is on will disable password saving as well. */
                OpenORPG.Settings.getInstance().savePassword = OpenORPG.Settings.getInstance().saveUsername && OpenORPG.Settings.getInstance().savePassword;
            }
            if (togglePass) {
                OpenORPG.Settings.getInstance().savePassword = !OpenORPG.Settings.getInstance().savePassword;
            }
            /* This makes it so toggling the password saving on while
            username saving is off will enable user saving as well. */
            OpenORPG.Settings.getInstance().saveUsername = OpenORPG.Settings.getInstance().saveUsername || OpenORPG.Settings.getInstance().savePassword;
            OpenORPG.Settings.getInstance().flush();
            OpenORPG.Settings.getInstance().save();
            this._rememberUser.checked = OpenORPG.Settings.getInstance().saveUsername;
            this._rememberPass.checked = OpenORPG.Settings.getInstance().savePassword;
        };
        return LoginPanel;
    })(OpenORPG.UI.Panel);
})(OpenORPG || (OpenORPG = {}));
var OpenORPG;
(function (OpenORPG) {
    /**
     * Alignment enum.
     * DOCTODO: This documentation is terrible (and I wrote it)
     */
    (function (Align) {
        Align[Align["Center"] = 0xF] = "Center";
        Align[Align["Left"] = 0xD] = "Left";
        Align[Align["Right"] = 0xE] = "Right";
        Align[Align["Top"] = 0x7] = "Top";
        Align[Align["Bottom"] = 0xB] = "Bottom";
        Align[Align["TopLeft"] = 0x5] = "TopLeft";
        Align[Align["TopRight"] = 0x6] = "TopRight";
        Align[Align["BottomLeft"] = 0x9] = "BottomLeft";
        Align[Align["BottomRight"] = 0xA] = "BottomRight";
        Align[Align["Default"] = Align.TopLeft] = "Default";
    })(OpenORPG.Align || (OpenORPG.Align = {}));
    var Align = OpenORPG.Align;
    /**
     * DOCTODO
     *
     * @param hname
     * @param vname
     * @returns
     */
    function getAlign(hname, vname) {
        return getHAlign(hname) & getVAlign(vname);
    }
    OpenORPG.getAlign = getAlign;
    /**
     * DOCTODO
     *
     * @param name
     * @returns
     */
    function getHAlign(name) {
        switch (name) {
            case "center": return 15 /* Center */;
            case "left": return 13 /* Left */;
            case "right": return 14 /* Right */;
            default: return 13 /* Left */;
        }
    }
    OpenORPG.getHAlign = getHAlign;
    /**
     * DOCTODO
     *
     * @param name
     * @returns
     */
    function getVAlign(name) {
        switch (name) {
            case "baseline":
            case "middle":
                return 15 /* Center */;
            case "text-top":
            case "super":
            case "top":
                return 7 /* Top */;
            case "text-bottom":
            case "sub":
            case "bottom":
                return 11 /* Bottom */;
            default: return 7 /* Top */;
        }
    }
    OpenORPG.getVAlign = getVAlign;
    /**
     * DOCTODO
     *
     * @param align
     * @returns
     */
    function getHAlignName(align) {
        switch (align) {
            case 13 /* Left */:
            case 5 /* TopLeft */:
            case 9 /* BottomLeft */:
                return "left";
            case 15 /* Center */:
            case 7 /* Top */:
            case 11 /* Bottom */:
                return "center";
            case 14 /* Right */:
            case 6 /* TopRight */:
            case 10 /* BottomRight */:
                return "right";
            default: return "top";
        }
    }
    OpenORPG.getHAlignName = getHAlignName;
    /**
     * DOCTODO
     *
     * @param align
     * @returns
     */
    function getVAlignName(align) {
        switch (align) {
            case 15 /* Center */:
            case 13 /* Left */:
            case 14 /* Right */:
                return "middle";
            case 7 /* Top */:
            case 5 /* TopLeft */:
            case 6 /* TopRight */:
                return "top";
            case 11 /* Bottom */:
            case 9 /* BottomLeft */:
            case 10 /* BottomRight */:
                return "bottom";
            default: return "top";
        }
    }
    OpenORPG.getVAlignName = getVAlignName;
})(OpenORPG || (OpenORPG = {}));
var AngularInterop;
(function (AngularInterop) {
    /**
     * Updates the AngularJS scope on this page
     */
    function updateAngularScope() {
        var $body = angular.element(document.body); // 1    
        var service = $body.injector().get('$timeout');
        var $rootScope = $body.scope();
        var phase = $rootScope.$root.$$phase;
        if (phase == '$apply' || phase == '$digest') {
        }
        else {
            $rootScope.$apply();
        }
    }
    AngularInterop.updateAngularScope = updateAngularScope;
    function broadcastEvent(eventName) {
        var $body = angular.element(document.body);
        var $rootScope = $body.scope();
        // Send event
        $rootScope.$broadcast(eventName);
        this.updateAngularScope();
    }
    AngularInterop.broadcastEvent = broadcastEvent;
})(AngularInterop || (AngularInterop = {}));
///<reference path="Element.ts" />
var OpenORPG;
(function (OpenORPG) {
    var UI;
    (function (UI) {
        /**
         * DOCTODO
         */
        var Button = (function (_super) {
            __extends(Button, _super);
            function Button() {
                _super.apply(this, arguments);
            }
            return Button;
        })(UI.Element);
        UI.Button = Button;
    })(UI = OpenORPG.UI || (OpenORPG.UI = {}));
})(OpenORPG || (OpenORPG = {}));
var OpenORPG;
(function (OpenORPG) {
    var EquipmentItemContextMenu = (function () {
        /*
         * The container elements are intialized here
         */
        function EquipmentItemContextMenu(container) {
            var jquery = $;
            jquery(container).contextmenu({
                delegate: ".equipitem",
                menu: [
                    {
                        title: "Remove",
                        uiIcon: "",
                        action: function (event, ui) {
                            if ($(ui.target).hasClass("itemtext")) {
                                ui.target = $(ui.target).parent();
                            }
                            var id = parseInt($(ui.target).attr("slot"));
                            var request = PacketFactory.createUnEqupRequest(id);
                            OpenORPG.NetworkManager.getInstance().sendPacket(request);
                        }
                    }
                ]
            });
            // End of event construction here
        }
        return EquipmentItemContextMenu;
    })();
    OpenORPG.EquipmentItemContextMenu = EquipmentItemContextMenu;
})(OpenORPG || (OpenORPG = {}));
///<reference path="Element.ts" />
var OpenORPG;
(function (OpenORPG) {
    var UI;
    (function (UI) {
        /**
         * DOCTODO
         */
        var Checkbox = (function (_super) {
            __extends(Checkbox, _super);
            function Checkbox() {
                _super.apply(this, arguments);
            }
            Object.defineProperty(Checkbox.prototype, "checked", {
                get: function () {
                    return Boolean(this.element.attr("ng-checked"));
                },
                set: function (value) {
                    if (typeof value == "number") {
                        value = value == 0 ? false : true;
                    }
                    this.check(Boolean(value));
                },
                enumerable: true,
                configurable: true
            });
            Checkbox.prototype.check = function (value) {
                if (value === void 0) { value = true; }
                if (value) {
                    this.element.prop("checked", "checked");
                }
                else {
                    this.element.removeProp("checked");
                }
            };
            Checkbox.prototype.uncheck = function () {
                this.check(false);
            };
            return Checkbox;
        })(UI.Element);
        UI.Checkbox = Checkbox;
    })(UI = OpenORPG.UI || (OpenORPG.UI = {}));
})(OpenORPG || (OpenORPG = {}));
var OpenORPG;
(function (OpenORPG) {
    /**
     * A HUD widget is anything that is thrown on top of the main canvas container
     */
    var HudWidget = (function () {
        function HudWidget(canvas, templatePath) {
            this.canvas = canvas;
            var that = this;
            // We can instance our template path here to generate our DOM element           
            $.get(templatePath, function (data) {
                that.container = $(data);
                $(canvas).append(that.container);
                angular.element(document).injector().invoke(function ($compile) {
                    var container = that.container;
                    var scope = angular.element(container).scope();
                    $compile(container)(scope);
                    scope.$apply();
                });
                that.onLoaded();
            });
        }
        /**
         * This method is called when loaded
         */
        HudWidget.prototype.onLoaded = function () {
        };
        HudWidget.prototype.show = function () {
            this.container.show();
        };
        HudWidget.prototype.hide = function () {
            this.container.hide();
        };
        return HudWidget;
    })();
    OpenORPG.HudWidget = HudWidget;
    /*
     * A small widget that is bound to the top left of the window space that displays information regarding character status.
     */
    var CharacterStatusWidget = (function (_super) {
        __extends(CharacterStatusWidget, _super);
        function CharacterStatusWidget(canvas, player) {
            _super.call(this, canvas, "assets/templates/widgets/character_status.html");
        }
        CharacterStatusWidget.prototype.onLoaded = function () {
        };
        return CharacterStatusWidget;
    })(HudWidget);
    OpenORPG.CharacterStatusWidget = CharacterStatusWidget;
    var BottombarWidget = (function (_super) {
        __extends(BottombarWidget, _super);
        function BottombarWidget(canvas) {
            _super.call(this, canvas, "assets/templates/widgets/bottom_bar.html");
        }
        return BottombarWidget;
    })(HudWidget);
    OpenORPG.BottombarWidget = BottombarWidget;
    var MenuTrayWidget = (function (_super) {
        __extends(MenuTrayWidget, _super);
        function MenuTrayWidget(canvas, playerInfo) {
            _super.call(this, canvas, "assets/templates/widgets/menu_tray.html");
            this.playerInfo = playerInfo;
        }
        MenuTrayWidget.prototype.onLoaded = function () {
            var _this = this;
            this.inventoryWindow = new OpenORPG.InventoryWindow();
            this.characterWindow = new OpenORPG.CharacterWindow(this.playerInfo);
            this.questListWindow = new OpenORPG.QuestListWindow();
            this.settingsWindow = new OpenORPG.SettingsWindow();
            var that = this;
            // A few events quickly to bind our menu items
            this.container.find(".menu-item-backpack").on("click", function () {
                that.inventoryWindow.toggleVisibility();
            });
            this.container.find(".menu-item-equip").on("click", function () {
                that.characterWindow.toggleVisibility();
            });
            this.container.find(".menu-item-achievements").on("click", function () {
                that.questListWindow.toggleVisibility();
            });
            this.container.find(".menu-item-settings").on("click", function () {
                that.settingsWindow.toggleVisibility();
            });
            this.container.find(".menu-item-skills").on("click", function () {
                var q;
                q = new OpenORPG.SkillWindow(_this.playerInfo);
                q.toggleVisibility();
            });
        };
        return MenuTrayWidget;
    })(HudWidget);
    OpenORPG.MenuTrayWidget = MenuTrayWidget;
    var LoginPanelWidget = (function (_super) {
        __extends(LoginPanelWidget, _super);
        function LoginPanelWidget(canvas) {
            _super.call(this, canvas, "assets/templates/widgets/login_panel.html");
        }
        LoginPanelWidget.prototype.onLoaded = function () {
            var that = this;
            //var tat = new UIElement(null, "#login-panel");
            $("#remember-user").on("click", function (eventObject) {
                var args = [];
                for (var _i = 1; _i < arguments.length; _i++) {
                    args[_i - 1] = arguments[_i];
                }
                that.refreshCheckboxes(true, false);
            });
            $("#remember-pass").on("click", function (eventObject) {
                var args = [];
                for (var _i = 1; _i < arguments.length; _i++) {
                    args[_i - 1] = arguments[_i];
                }
                that.refreshCheckboxes(false, true);
            });
            this.refreshCheckboxes();
        };
        LoginPanelWidget.prototype.refreshCheckboxes = function (toggleUser, togglePass) {
            if (toggleUser === void 0) { toggleUser = false; }
            if (togglePass === void 0) { togglePass = false; }
            if (toggleUser) {
                OpenORPG.Settings.getInstance().saveUsername = !OpenORPG.Settings.getInstance().saveUsername;
            }
            if (togglePass) {
                OpenORPG.Settings.getInstance().savePassword = !OpenORPG.Settings.getInstance().savePassword;
            }
            OpenORPG.Settings.getInstance().savePassword = OpenORPG.Settings.getInstance().saveUsername && OpenORPG.Settings.getInstance().savePassword;
            OpenORPG.Settings.getInstance().flush();
            OpenORPG.Settings.getInstance().save();
            var rememberUser = $("#remember-user");
            if (OpenORPG.Settings.getInstance().saveUsername) {
                console.log("yes user");
                rememberUser.attr("checked", "true");
            }
            else {
                console.log("not user");
                rememberUser.removeAttr("checked");
            }
            var rememberPass = $("#remember-pass");
            if (OpenORPG.Settings.getInstance().savePassword) {
                console.log("yes pass");
                rememberPass.attr("checked", "true");
            }
            else {
                console.log("not pass");
                rememberPass.removeAttr("checked");
            }
        };
        return LoginPanelWidget;
    })(HudWidget);
    OpenORPG.LoginPanelWidget = LoginPanelWidget;
    var ChatWidget = (function (_super) {
        __extends(ChatWidget, _super);
        function ChatWidget(canvas) {
            _super.call(this, canvas, "assets/templates/widgets/chat.html");
        }
        ChatWidget.prototype.onLoaded = function () {
            // Do some basic key bindings
            $(document).on('keypress', function (event) {
                if (document.activeElement) {
                    var x = document.activeElement;
                    var id = x.id;
                    if (event.which == 13) {
                        if (id == "chatmessage") {
                            Logger.trace("ChatWidget - Focused game");
                            $("#canvasholder").focus();
                        }
                        else {
                            Logger.trace("ChatWidget - Focused chat");
                            $("#chatmessage").focus();
                        }
                    }
                }
            });
        };
        return ChatWidget;
    })(HudWidget);
    OpenORPG.ChatWidget = ChatWidget;
})(OpenORPG || (OpenORPG = {}));
var OpenORPG;
(function (OpenORPG) {
    /*
     *  An interface window inside of a game
     */
    var InterfaceWindow = (function () {
        function InterfaceWindow(windowFile, windowName) {
            var _this = this;
            this.$window = $(windowName).dialog({
                autoOpen: true,
                resizable: false,
                modal: false,
                width: 'auto',
                open: function () {
                    $(this).parent().css("padding", "0px");
                    $(this).parent().css("background", "transparent");
                }
            });
            var that = this;
            // Get jQuery to load the static content into the page
            $.get(windowFile, function (data) {
                $(windowName).html(data);
                _this.ready();
                that.toggleVisibility();
                angular.element(document).injector().invoke(function ($compile) {
                    var container = $(that.windowName);
                    var scope = angular.element(container).scope();
                    $compile(container)(scope);
                    scope.$apply();
                });
            });
            this.windowName = windowName;
        }
        // Close this interface window
        InterfaceWindow.prototype.toggleVisibility = function () {
            ($(this.$window).dialog("isOpen") == false) ? $(this.$window).dialog("open") : $(this.$window).dialog("close");
        };
        InterfaceWindow.prototype.close = function () {
            $(this.$window).dialog("close");
        };
        InterfaceWindow.prototype.open = function () {
            $(this.$window).dialog("open");
        };
        InterfaceWindow.prototype.ready = function () {
        };
        return InterfaceWindow;
    })();
    OpenORPG.InterfaceWindow = InterfaceWindow;
    var QuestListWindow = (function (_super) {
        __extends(QuestListWindow, _super);
        function QuestListWindow() {
            _super.call(this, "assets/hud/quest_list.html", "#quest-list-dialog");
        }
        return QuestListWindow;
    })(InterfaceWindow);
    OpenORPG.QuestListWindow = QuestListWindow;
    var SettingsWindow = (function (_super) {
        __extends(SettingsWindow, _super);
        function SettingsWindow() {
            _super.call(this, "assets/hud/settings.html", "#settings-dialog");
        }
        return SettingsWindow;
    })(InterfaceWindow);
    OpenORPG.SettingsWindow = SettingsWindow;
    var DialogWindow = (function (_super) {
        __extends(DialogWindow, _super);
        function DialogWindow() {
            _super.call(this, "assets/hud/dialog.html", "#dialog-window");
        }
        //jQuery it up.. oh god this needs a cleaning in the worst way
        DialogWindow.prototype.presentDialog = function (text, links) {
            this.render(text, links);
            this.open();
        };
        DialogWindow.prototype.render = function (text, links) {
            $(this.windowName).find("#description").text(text);
            $(this.windowName).find(".action-bar").children().remove();
            links.forEach(function (link, index) {
                var x = $("<span class='action-button'></span>");
                x.data("link", link.index);
                x.text(link.link);
                $(".action-bar").append(x);
            });
            $(this.windowName).find(".action-button").click(function (event) {
                var element = $(event.target);
                var index = parseInt(element.data("link"));
                var packet = PacketFactory.createDialogLink(index);
                OpenORPG.NetworkManager.getInstance().sendPacket(packet);
            });
        };
        return DialogWindow;
    })(InterfaceWindow);
    OpenORPG.DialogWindow = DialogWindow;
    /*
     * A window that is used for displaying quest related stuff
     */
    var QuestWindow = (function (_super) {
        __extends(QuestWindow, _super);
        function QuestWindow() {
            _super.call(this, "assets/hud/quest.html", "#quest-dialog");
        }
        QuestWindow.prototype.presentQuest = function (questId) {
            this.id = questId;
            this.render();
            this.open();
        };
        QuestWindow.prototype.ready = function () {
        };
        /*
         * We stick with jQuery here for legacy reasons. No need to change stuff that isn't broken.
         */
        QuestWindow.prototype.render = function () {
            var _this = this;
            $(this.windowName).prev().hide();
            // Load the quest info and get ready
            OpenORPG.ContentManager.getInstance().getContent(2 /* Quest */, this.id, function (quest) {
                $(_this.windowName).find("#description").text(quest.description);
                $(_this.windowName).find(".quest-header").text(quest.name);
            });
            var that = this;
            $(this.windowName).find("#accept-button").click(function () {
                var packet = PacketFactory.createQuestAcceptRequest(that.id);
                OpenORPG.NetworkManager.getInstance().sendPacket(packet);
                that.close();
            });
            $(this.windowName).find("#decline-button").click(function () {
                that.close();
            });
        };
        return QuestWindow;
    })(InterfaceWindow);
    OpenORPG.QuestWindow = QuestWindow;
    var SkillWindow = (function (_super) {
        __extends(SkillWindow, _super);
        function SkillWindow(playerInfo) {
            _super.call(this, "assets/hud/skills.html", "#skill-dialog");
            this.playerInfo = playerInfo;
        }
        SkillWindow.prototype.ready = function () {
        };
        return SkillWindow;
    })(InterfaceWindow);
    OpenORPG.SkillWindow = SkillWindow;
    var CharacterWindow = (function (_super) {
        __extends(CharacterWindow, _super);
        function CharacterWindow(playerInfo) {
            var _this = this;
            _super.call(this, "assets/hud/character.html", "#characterdialog");
            this.equipmentBindings = [];
            this.bindEquipmentSlots();
            this.playerInfo = playerInfo;
            // Setup a binding to change on character state change
            //this.playerInfo.listenCharacterStatChange($.proxy(this.renderStats, this));
            OpenORPG.NetworkManager.getInstance().registerPacket(36 /* SMSG_EQUIPMENT_UPDATE */, function (packet) {
                // Update all the things
                var slot = packet.slot;
                var equipment = packet.equipment;
                // Update the slot with the item you need
                var domSlot = _this.equipmentBindings[slot];
                $(domSlot).empty();
                if (equipment != null) {
                    var item = $("<div class='equipitem'></div>");
                    var image = GraphicsUtil.getIconCssFromId(equipment.iconId);
                    $(item).css('background', image);
                    $(domSlot).append(item);
                    $(item).attr("slot", slot);
                    var menu = new OpenORPG.EquipmentItemContextMenu(item.parent()[0]);
                    var tooltip = new OpenORPG.ItemTooltipInfo($(domSlot)[0], equipment);
                }
            });
        }
        CharacterWindow.prototype.bindEquipmentSlots = function () {
            this.equipmentBindings[0 /* Weapon */] = (".weaponslot");
            this.equipmentBindings[1 /* Head */] = (".headslot");
            this.equipmentBindings[5 /* Hands */] = (".handsslot");
            this.equipmentBindings[4 /* Feet */] = (".feetslot");
            this.equipmentBindings[2 /* Body */] = (".bodyslot");
            this.equipmentBindings[3 /* Back */] = (".backslot");
        };
        /*
         * This function is reponsible for rendering character statistics onto the form.
         */
        CharacterWindow.prototype.renderStats = function () {
            // Render info
            var selector = $(this.windowName).find("#statspanel").selector;
            $(selector).find('#charactername').text(this.playerInfo.name);
            // Remove old stat stuff
            $(".statrow").remove();
            var names = [];
            for (var n in OpenORPG.StatTypes) {
                if (typeof OpenORPG.StatTypes[n] === 'number')
                    names.push(n);
            }
            for (var key in this.playerInfo.characterStats) {
                var value = this.playerInfo.characterStats[key];
                var content = $('<div class="statrow"></div>');
                if (value.maximumValue > 0)
                    $(content).html(names[key] + ': <b class="statnumber">' + value.currentValue + '/' + value.maximumValue + '</b>');
                else
                    $(content).html(names[key] + ': <b class="statnumber">' + value.currentValue + '</b>');
                $(selector).append(content);
            }
        };
        return CharacterWindow;
    })(InterfaceWindow);
    OpenORPG.CharacterWindow = CharacterWindow;
    var InventoryWindow = (function (_super) {
        __extends(InventoryWindow, _super);
        // Create our inventory window
        function InventoryWindow() {
            var _this = this;
            _super.call(this, "assets/hud/inventory.html", "#inventorydialog");
            // Hook into our network events
            OpenORPG.NetworkManager.getInstance().registerPacket(32 /* SMSG_STORAGE_HERO_SEND */, function (packet) {
                // Do something about the inventory update   
                _this.renderInventory(packet.itemStorage);
            });
        }
        InventoryWindow.prototype.ready = function () {
        };
        InventoryWindow.prototype.renderInventory = function (inventory) {
            $("#itemback").empty();
            for (var i = 0; i < inventory.capacity; i++) {
                var $slot = $("<div class='itemslot'/>").attr("slotId", i);
                $("#itemback").append($slot);
            }
            for (var slotId in inventory.storage) {
                var item = $("<div class='item'> <div class='itemtext'/>  </div>");
                var gameItem = inventory.storage[slotId];
                $('[slotId="' + slotId + '"]').append(item);
                var image = GraphicsUtil.getIconCssFromId(gameItem.item.iconId);
                $(item).css('background', image);
                item.children().first().text(gameItem.amount);
                // Set title
                // $(item).attr("title", "Name: " + gameItem.item.name + " | Description: " + gameItem.item.description);
                var tooltip = new OpenORPG.ItemTooltipInfo($('[slotId="' + slotId + '"]')[0], gameItem.item);
            }
            // Setup drag events
            $(".item").draggable({ revert: 'invalid' });
            $('.itemslot').droppable({
                accept: '.item',
                drop: function (ev, ui) {
                    var dropped = ui.draggable;
                    var droppedOn = $(this);
                    //
                    var sourceSlotId = parseInt($(dropped).parent().attr("slotId"));
                    var destSlotId = parseInt($(droppedOn).attr("slotId"));
                    $(dropped).detach().css({ top: 0, left: 0 }).appendTo(droppedOn);
                    var packet = PacketFactory.createStorageMoveRequest(sourceSlotId, destSlotId, 0);
                    OpenORPG.NetworkManager.getInstance().sendPacket(packet);
                }
            });
            // Attach a context menu
            var menu = new OpenORPG.InventoryContextMenu($("#itemback").get(0));
        };
        InventoryWindow.prototype.itemInSpot = function (drag_item, spot) {
            var item = $('<div />'); // create new img element
            item.attr({
                src: drag_item.attr('src'),
            }).attr('class', drag_item.attr('class')).appendTo(spot).draggable({ revert: 'invalid' }); // add to spot + make draggable
            drag_item.remove(); // remove the old object
        };
        return InventoryWindow;
    })(InterfaceWindow);
    OpenORPG.InventoryWindow = InventoryWindow;
})(OpenORPG || (OpenORPG = {}));
var OpenORPG;
(function (OpenORPG) {
    var InventoryContextMenu = (function () {
        /*
         * The container elements are intialized here
         */
        function InventoryContextMenu(container) {
            var jquery = $;
            jquery(container).contextmenu({
                delegate: ".item",
                menu: [
                    {
                        title: "Use",
                        uiIcon: "",
                        action: function (event, ui) {
                            if ($(ui.target).hasClass("itemtext")) {
                                ui.target = $(ui.target).parent();
                            }
                            var id = parseInt($(ui.target).parent().attr("slotId"));
                            var request = PacketFactory.createItemuseRequest(id);
                            OpenORPG.NetworkManager.getInstance().sendPacket(request);
                        }
                    },
                    {
                        title: "Drop",
                        uiIcon: "",
                        action: function (event, ui) {
                            if ($(ui.target).hasClass("itemtext")) {
                                ui.target = $(ui.target).parent();
                            }
                            var id = parseInt($(ui.target).attr("slot"));
                            var request = PacketFactory.createStorageDropRequest(id, 1);
                            OpenORPG.NetworkManager.getInstance().sendPacket(request);
                        }
                    }
                ]
            });
            // End of event construction here
        }
        return InventoryContextMenu;
    })();
    OpenORPG.InventoryContextMenu = InventoryContextMenu;
})(OpenORPG || (OpenORPG = {}));
var OpenORPG;
(function (OpenORPG) {
    /*
     * This class is responsible for displaying tooltip info for items.
     * It will display some general information
     */
    var ItemTooltipInfo = (function () {
        /*
         * Contstructs a new tooltip
         */
        function ItemTooltipInfo(element, gameItem) {
            var _this = this;
            this.element = element;
            this.gameItem = gameItem;
            $.get("assets/hud/itemtooltip.html", function (data) {
                _this.data = data;
                _this.ready();
            });
        }
        ItemTooltipInfo.prototype.ready = function () {
            var _this = this;
            $(this.element).tooltip({
                position: {
                    my: "center top",
                    at: "center bottom",
                    using: function (position, feedback) {
                        $(this).css(position);
                        $("<div>").addClass("arrow").addClass(feedback.vertical).addClass(feedback.horizontal).appendTo(this);
                    }
                },
                items: '.item, .equipitem',
                content: function () {
                    var context = $(_this.data);
                    var image = GraphicsUtil.getIconCssFromId(_this.gameItem.iconId);
                    $(context).find("#IconHolder").css('background', image);
                    $(context).find("#ItemName").text(_this.gameItem.name);
                    $(context).find(".ItemDesc").text(_this.gameItem.description);
                    $(context).find("#ItemTypex").text(_this.gameItem.type);
                    //ItemName
                    return context[2].outerHTML;
                }
            });
            // Do some markup stuff here now
            // IconHolder
        };
        return ItemTooltipInfo;
    })();
    OpenORPG.ItemTooltipInfo = ItemTooltipInfo;
})(OpenORPG || (OpenORPG = {}));
var OpenORPG;
(function (OpenORPG) {
    var ItemRequirementFormatter = (function () {
        function ItemRequirementFormatter() {
            this.infoType = "QuestHasItemRequirement";
        }
        ItemRequirementFormatter.prototype.getLocalizedString = function (info, progress, callback) {
            var _this = this;
            var itemId = info.itemId;
            var itemAmount = info.itemAmount;
            OpenORPG.ContentManager.getInstance().getContent(0 /* Item */, info.itemId, function (item) {
                var result = OpenORPG.LocaleManager.getInstance().getString(_this.infoType, [item.name, itemAmount]);
                callback(result);
            });
        };
        return ItemRequirementFormatter;
    })();
    OpenORPG.ItemRequirementFormatter = ItemRequirementFormatter;
})(OpenORPG || (OpenORPG = {}));
var OpenORPG;
(function (OpenORPG) {
    var MonstersKilledFormatter = (function () {
        function MonstersKilledFormatter() {
            this.infoType = "QuestMonstersKilledRequirement";
        }
        MonstersKilledFormatter.prototype.getLocalizedString = function (info, progress, callback) {
            var _this = this;
            var monsterId = info.monsterId;
            var monsterAmount = info.monsterAmount;
            OpenORPG.ContentManager.getInstance().getContent(1 /* Monster */, info.monsterId, function (monster) {
                var result = OpenORPG.LocaleManager.getInstance().getString(_this.infoType, [monster.name, monsterAmount, progress]);
                callback(result);
            });
        };
        return MonstersKilledFormatter;
    })();
    OpenORPG.MonstersKilledFormatter = MonstersKilledFormatter;
})(OpenORPG || (OpenORPG = {}));
var OpenORPG;
(function (OpenORPG) {
    /*
     *  The RequirementFormatter is responsible for maintaing a simple map of formatters and printing out the
     *  quest requirements in an easy to digest, and easy to read way. It utilizes the local manager internally
     *  to ensure these are completely localized.
     *
     *  NOTE: Do not hard-code any requirement strings in here
     */
    var RequirementFormatter = (function () {
        function RequirementFormatter() {
            this.formatters = new Object();
            this.formatters["QuestMonstersKilledRequirement"] = new OpenORPG.MonstersKilledFormatter();
            this.formatters["QuestHasItemRequirement"] = new OpenORPG.ItemRequirementFormatter();
        }
        RequirementFormatter.prototype.getFormattedRequirement = function (type, info, progress, callback) {
            if (this.formatters[type]) {
                this.formatters[type].getLocalizedString(info, progress, function (result) {
                    callback(result);
                });
            }
            else {
                Logger.warn("The quest requirement formatter for the type of " + type + " could not be found. It should be added or UI elements will look wrong.");
                callback(OpenORPG.LocaleManager.getInstance().getString("RequirementFormatterMissing", []));
            }
        };
        return RequirementFormatter;
    })();
    OpenORPG.RequirementFormatter = RequirementFormatter;
})(OpenORPG || (OpenORPG = {}));
///<reference path="Element.ts" />
var OpenORPG;
(function (OpenORPG) {
    var UI;
    (function (UI) {
        /**
         * DOCTODO
         */
        var RadioButton = (function (_super) {
            __extends(RadioButton, _super);
            function RadioButton() {
                _super.apply(this, arguments);
            }
            return RadioButton;
        })(UI.Element);
        UI.RadioButton = RadioButton;
    })(UI = OpenORPG.UI || (OpenORPG.UI = {}));
})(OpenORPG || (OpenORPG = {}));
///<reference path="Element.ts" />
var OpenORPG;
(function (OpenORPG) {
    var UI;
    (function (UI) {
        /**
         * DOCTODO
         */
        var Textbox = (function (_super) {
            __extends(Textbox, _super);
            function Textbox() {
                _super.apply(this, arguments);
            }
            return Textbox;
        })(UI.Element);
        UI.Textbox = Textbox;
    })(UI = OpenORPG.UI || (OpenORPG.UI = {}));
})(OpenORPG || (OpenORPG = {}));
///<reference path="Element.ts" />
var OpenORPG;
(function (OpenORPG) {
    var UI;
    (function (UI) {
        /**
         * DOCTODO
         */
        var Window = (function (_super) {
            __extends(Window, _super);
            function Window() {
                _super.apply(this, arguments);
            }
            return Window;
        })(UI.Panel);
        UI.Window = Window;
    })(UI = OpenORPG.UI || (OpenORPG.UI = {}));
})(OpenORPG || (OpenORPG = {}));
var OpenORPG;
(function (OpenORPG) {
    var Zone = (function () {
        function Zone(game, playerInfo) {
            var _this = this;
            // Internal lists for usage later
            this._toRemove = [];
            this._toAdd = [];
            this._toUpdate = [];
            // An array of entities to use
            this.entities = new Array();
            this.systems = new Array();
            this.game = game;
            this.playerInfo = playerInfo;
            Zone.current = this;
            this.setupNetworkHandlers();
            this.game.scale.onSizeChange.add(function () {
                console.log("dirty");
                _this.bucket.forEach(function (layer) {
                    layer.dirty = true;
                    // TODO: Uncomment when it gets implemented in a stable Phaser
                    /*debugger;
                    layer.resizeCanvas(); */
                });
            }, this);
        }
        Zone.prototype.initLocalZone = function (mapId) {
            // Setup tilemap
            var game = this.game;
            this._mapId = mapId;
            this.tileMap = game.add.tilemap("map_" + mapId);
            this.tileMap.addTilesetImage("tilesheet");
            // Size and prepare
            var self = this;
            this.bucket = [];
            for (var layerKey in this.tileMap.layers) {
                var layer = this.tileMap.layers[layerKey];
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
                this.movementSystem = new OpenORPG.MovementSystem(this, null);
                this.systems.push(this.movementSystem);
                this.combatSystem = new OpenORPG.CombatSystem(this, null, this.playerInfo);
                this.systems.push(this.combatSystem);
            }
            this.systems.forEach(function (system) {
                system.initZone();
            });
        };
        Zone.prototype.addNetworkEntityToZone = function (entity) {
            var worldEntity = new OpenORPG.Entity(this.game, 0, 0);
            worldEntity.mergeWith(entity);
            worldEntity.initAsNetworkable();
            this.entities[worldEntity.id] = worldEntity;
            this.entityGroup.addChild(worldEntity);
            for (var key in entity) {
                var value = entity[key];
                worldEntity.propertyChanged(key, value);
            }
            this._toAdd.push(worldEntity.id);
            // Allow adding hooks where needed
            this.systems.forEach(function (system) { return system.onEntityAdded(worldEntity); });
            return worldEntity;
        };
        Zone.prototype.clearZone = function () {
            for (var entityKey in this.entities) {
                var entity = this.entities[entityKey];
                entity.destroy();
                entity.destroyNamePlate();
                // remove from list
                delete this.entities[entityKey];
            }
            // Create
            this.entities = new Array();
            for (var bucketKey in this.bucket) {
                var layer = this.bucket[bucketKey];
                layer.destroy();
            }
            // Destroy our tilemap
            if (this.tileMap != null)
                this.tileMap.destroy();
            if (this.tileMap != null)
                this.entityGroup.destroy(true);
        };
        Zone.prototype.generateCollisionMap = function () {
            var props = this.tileMap.tilesets[0];
            var tileProps = props.tileProperties;
            for (var propKey in tileProps) {
                var propValue = tileProps[propKey];
                if (propValue.hasOwnProperty("c")) {
                    for (var i = 0; i < this.tileMap.layers.length; i++) {
                        this.tileMap.setCollision([parseInt(propKey) + 1], true, i);
                    }
                }
            }
        };
        Zone.prototype.render = function () {
            /* FUTUREIMPL: Currently does nothing. */
            /*for (var system in this.systems) {
                this.systems[system].render();
            }

            for (var entityKey in this.entities) {
                var entity = this.entities[entityKey];
                entity.render();
            }*/
        };
        Zone.prototype.update = function () {
            for (var toUpdate in this._toUpdate) {
                var valueB = this._toUpdate[toUpdate];
                var entityB = Zone.current.entities[valueB.entityId];
                // We should check for the existance before updating. 
                if (entityB) {
                    entityB.mergeWith(valueB.properties);
                    for (var key in valueB.properties) {
                        var v = valueB.properties[key];
                        entityB.propertyChanged(key, v);
                    }
                }
                else {
                    Logger.warn("An update was sent for an entity that no longer exists on the client. Out of view?");
                }
            }
            for (var toRemove in this._toRemove) {
                var value = this._toRemove[toRemove];
                var entity = this.entities[value];
                Logger.debug("Entity was removed from the current zone");
                Logger.debug(entity);
                // Allow any unhooking that needs to be done first
                this.systems.forEach(function (system) { return system.onEntityRemoved(entity); });
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
            for (var entityKey in this.entities) {
                var entity = this.entities[entityKey];
                for (var layer in this.bucket)
                    this.game.physics.arcade.collide(entity, this.bucket[layer]);
            }
            // Update list of removal
            this._toRemove = [];
            this._toAdd = [];
            this._toUpdate = [];
            for (var system in this.systems) {
                this.systems[system].update();
            }
            // Re sort our entities
            if (this.entityGroup != null)
                this.entityGroup.sort('y', Phaser.Group.SORT_ASCENDING);
        };
        Zone.prototype.setupNetworkHandlers = function () {
            var network = OpenORPG.NetworkManager.getInstance();
            network.registerPacket(9 /* SMSG_MOB_CREATE */, function (packet) {
                Zone.current._toAdd.push(packet.mobile);
            });
            network.registerPacket(13 /* SMSG_MOB_DESTROY */, function (packet) {
                Zone.current._toRemove.push(packet.id);
            });
            network.registerPacket(20 /* SMSG_ENTITY_PROPERTY_CHANGE */, function (packet) {
                Zone.current._toUpdate.push(packet);
            });
        };
        return Zone;
    })();
    OpenORPG.Zone = Zone;
})(OpenORPG || (OpenORPG = {}));
var OpenORPG;
(function (OpenORPG) {
    /*
     * The ContentManager is responsible for loading in various game files and keeping them cached.
     */
    var ContentManager = (function () {
        function ContentManager() {
            this.contentCache = {};
        }
        ContentManager.prototype.getContent = function (type, id, callback) {
            var name = OpenORPG.ContentType[type].toLowerCase() + "s";
            var cache = this.contentCache[name];
            if (!cache)
                this.contentCache[name] = {};
            // Attempt to fetch the content from the cache
            var content = this.contentCache[name][id];
            var that = this;
            if (content)
                callback(content);
            else {
                $.getJSON(DirectoryHelper.getGameFilesPath() + name + "/" + id.toString() + ".json", function (data) {
                    that.contentCache[name][id] = data;
                    callback(data);
                });
            }
        };
        ContentManager.getInstance = function () {
            if (ContentManager._instance === null) {
                ContentManager._instance = new ContentManager();
            }
            return ContentManager._instance;
        };
        ContentManager._instance = null;
        return ContentManager;
    })();
    OpenORPG.ContentManager = ContentManager;
})(OpenORPG || (OpenORPG = {}));
var OpenORPG;
(function (OpenORPG) {
    (function (ContentType) {
        ContentType[ContentType["Item"] = 0] = "Item";
        ContentType[ContentType["Monster"] = 1] = "Monster";
        ContentType[ContentType["Quest"] = 2] = "Quest";
        ContentType[ContentType["Skill"] = 3] = "Skill";
    })(OpenORPG.ContentType || (OpenORPG.ContentType = {}));
    var ContentType = OpenORPG.ContentType;
})(OpenORPG || (OpenORPG = {}));
var DirectoryHelper;
(function (DirectoryHelper) {
    // The base asset path
    var baseAssetPath = "assets/";
    function getSpritePath() {
        return baseAssetPath + "sprites/";
    }
    DirectoryHelper.getSpritePath = getSpritePath;
    function getMusicPath() {
        return baseAssetPath + "audio/music/";
    }
    DirectoryHelper.getMusicPath = getMusicPath;
    function getAudioEffectPath() {
        return baseAssetPath + "audio/sounds/";
    }
    DirectoryHelper.getAudioEffectPath = getAudioEffectPath;
    function getItemsPath() {
        return baseAssetPath + "items/";
    }
    DirectoryHelper.getItemsPath = getItemsPath;
    function getGameFilesPath() {
        return baseAssetPath + "gamesfiles/";
    }
    DirectoryHelper.getGameFilesPath = getGameFilesPath;
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
        return game.add.tween(gameObject).to(properties, 1200, Phaser.Easing.Linear.None, true);
    }
    EffectFactory.floatAndFadeAway = floatAndFadeAway;
    function pulseDamage(entity) {
        var properties = {
            tint: 0x7E3517,
            alpha: 0.7
        };
        entity.tint = 0xFFFFFF;
        entity.alpha = 1;
        // Pulse the damage output
        return entity.game.add.tween(entity).to(properties, 250, Phaser.Easing.Linear.None, true, 0, 1, true);
    }
    EffectFactory.pulseDamage = pulseDamage;
    function bounceSprite(sprite) {
        var properties = {
            y: sprite.y + 8
        };
        return sprite.game.add.tween(sprite).to(properties, 500, Phaser.Easing.Linear.None, true, 0, 1000000, true);
    }
    EffectFactory.bounceSprite = bounceSprite;
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
        var font = new OpenORPG.FontDefinition("14px AdvoCut", "#ffff00", "center", "#000000", 3);
        return font;
    }
    FontFactory.getPlayerFont = getPlayerFont;
    function getDamageFont() {
        return new OpenORPG.FontDefinition("26px AdvoCut", "#FFFFFF", "center", "#000000", 4);
    }
    FontFactory.getDamageFont = getDamageFont;
    function getNpcFont() {
        var font = new OpenORPG.FontDefinition("14px AdvoCut", "#FFFFFF", "center", "#000000", 3);
        return font;
    }
    FontFactory.getNpcFont = getNpcFont;
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
var GraphicsUtil;
(function (GraphicsUtil) {
    function itemIdToImagePath(itemId) {
        return DirectoryHelper.getItemsPath() + itemId + ".png";
    }
    GraphicsUtil.itemIdToImagePath = itemIdToImagePath;
    function getIconCssFromId(iconId) {
        var width = 1156;
        var iconSize = 34;
        var widthIcons = width / iconSize;
        var x = (iconId % widthIcons) * iconSize;
        var y = Math.floor(iconId / widthIcons) * iconSize;
        var s = "url(assets/iconsheet.png) -{0}px -{1}px";
        s = s.replace("{0}", x.toString());
        s = s.replace("{1}", y.toString());
        return s;
    }
    GraphicsUtil.getIconCssFromId = getIconCssFromId;
})(GraphicsUtil || (GraphicsUtil = {}));
var OpenORPG;
(function (OpenORPG) {
    (function (EquipmentSlot) {
        EquipmentSlot[EquipmentSlot["Weapon"] = 0] = "Weapon";
        EquipmentSlot[EquipmentSlot["Head"] = 1] = "Head";
        EquipmentSlot[EquipmentSlot["Body"] = 2] = "Body";
        EquipmentSlot[EquipmentSlot["Back"] = 3] = "Back";
        EquipmentSlot[EquipmentSlot["Feet"] = 4] = "Feet";
        EquipmentSlot[EquipmentSlot["Hands"] = 5] = "Hands";
    })(OpenORPG.EquipmentSlot || (OpenORPG.EquipmentSlot = {}));
    var EquipmentSlot = OpenORPG.EquipmentSlot;
})(OpenORPG || (OpenORPG = {}));
var OpenORPG;
(function (OpenORPG) {
    /**
     * Provides localization services to the entire game application.
     */
    var LocaleManager = (function () {
        function LocaleManager(locale) {
            LocaleManager._instance = this;
            this.setLocale(locale);
        }
        /**
         * Given a dot delimited message type, applies arguments to it and returns the localized version of it
         * for the current locale code.
         */
        LocaleManager.prototype.getString = function (messageType, args) {
            var str = this.currentLocaleData[messageType];
            if (str) {
                if (args) {
                    for (var i = 0; i < args.length; i++)
                        str = str.split("{" + i + "}").join(args[i]);
                }
            }
            else {
                str = "Error! No translation found for message type of " + messageType + " given locale code of " + this._localeCode;
            }
            return str;
        };
        /**
         * Sets the locale of the manager based on a locale code. Supported locale codes are:
         * EN: English
         * FR: French
         * JP: Japanese
         * DV: Development
         */
        LocaleManager.prototype.setLocale = function (localeCode) {
            var _this = this;
            this._localeCode = localeCode;
            $.getJSON("assets/locale/" + localeCode + ".json", function (data) {
                _this.currentLocaleData = data;
            });
        };
        LocaleManager.getInstance = function () {
            if (LocaleManager._instance === null) {
                LocaleManager._instance = new LocaleManager("en");
            }
            return LocaleManager._instance;
        };
        LocaleManager._instance = null;
        return LocaleManager;
    })();
    OpenORPG.LocaleManager = LocaleManager;
})(OpenORPG || (OpenORPG = {}));
var Logger;
(function (Logger) {
    var log4javascript = window["log4javascript"].getLogger();
    var log = log4javascript;
    log.setLevel(window["log4javascript"]["Level"]["ALL"]);
    var consoleLogger = new window["log4javascript"]["BrowserConsoleAppender"]();
    var popUpLayout = new window["log4javascript"]["PatternLayout"]("%d{HH:mm:ss} %-5p - %m");
    consoleLogger.setLayout(popUpLayout);
    consoleLogger.setThreshold(window["log4javascript"]["Level"]["TRACE"]);
    log.addAppender(consoleLogger);
    log.debug("Logging system has been booted up succesfully");
    function trace(params) {
        log.trace(params);
    }
    Logger.trace = trace;
    function debug(params) {
        log.debug(params);
    }
    Logger.debug = debug;
    function info(x) {
        log.info(x);
    }
    Logger.info = info;
    function warn(x) {
        log.warn(x);
    }
    Logger.warn = warn;
    function error(x) {
        log.error(x);
    }
    Logger.error = error;
    function fatal(x) {
        log.fatal(x);
    }
    Logger.fatal = fatal;
})(Logger || (Logger = {}));
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
        Object.defineProperty(NetworkManager, "hostname", {
            get: function () {
                if (this._hostname == null) {
                    return window.location.hostname;
                }
                return this._hostname;
            },
            set: function (val) {
                this._hostname = val;
            },
            enumerable: true,
            configurable: true
        });
        NetworkManager.getInstance = function () {
            if (NetworkManager._instance === null) {
                NetworkManager._instance = new NetworkManager(this.hostname, 4488);
            }
            return NetworkManager._instance;
        };
        NetworkManager.prototype.sendPacket = function (packet) {
            var json = JSON.stringify(packet);
            this._socket.send(json);
        };
        NetworkManager.prototype.registerPacket = function (opCode, callback) {
            if (!this._packetCallbacks[opCode])
                this._packetCallbacks[opCode] = [];
            this._packetCallbacks[opCode].push(callback);
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
            if (this._packetCallbacks[packet.opCode] != undefined) {
                _.forEach(this._packetCallbacks[packet.opCode], function (value) {
                    value(packet);
                });
            }
            else
                Logger.warn("A packet with the ID of " + packet.opCode + " received but not handled");
        };
        /* Networking */
        NetworkManager._hostname = null;
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
        OpCode[OpCode["CMSG_ITEM_USE"] = 23] = "CMSG_ITEM_USE";
        OpCode[OpCode["SMSG_LEAVE_CHAT_CHANNEL"] = 24] = "SMSG_LEAVE_CHAT_CHANNEL";
        OpCode[OpCode["CMSG_INTERACT_REQUEST"] = 25] = "CMSG_INTERACT_REQUEST";
        OpCode[OpCode["CMSG_QUEST_REQUEST_COMPLETION"] = 26] = "CMSG_QUEST_REQUEST_COMPLETION";
        OpCode[OpCode["CMSG_QUEST_ACCEPT"] = 27] = "CMSG_QUEST_ACCEPT";
        OpCode[OpCode["SMSG_QUEST_ACCEPT_RESULT"] = 28] = "SMSG_QUEST_ACCEPT_RESULT";
        OpCode[OpCode["SMSG_QUEST_COMPLETE_RESULT"] = 29] = "SMSG_QUEST_COMPLETE_RESULT";
        OpCode[OpCode["SMSG_SERVER_OFFER_QUEST"] = 30] = "SMSG_SERVER_OFFER_QUEST";
        OpCode[OpCode["CMSG_STORAGE_MOVE_SLOT"] = 31] = "CMSG_STORAGE_MOVE_SLOT";
        OpCode[OpCode["SMSG_STORAGE_HERO_SEND"] = 32] = "SMSG_STORAGE_HERO_SEND";
        OpCode[OpCode["CMSG_STORAGE_DROP"] = 33] = "CMSG_STORAGE_DROP";
        OpCode[OpCode["SMSG_SEND_GAMEMESSAGE"] = 34] = "SMSG_SEND_GAMEMESSAGE";
        OpCode[OpCode["CMSG_UNEQUIP_ITEM"] = 35] = "CMSG_UNEQUIP_ITEM";
        OpCode[OpCode["SMSG_EQUIPMENT_UPDATE"] = 36] = "SMSG_EQUIPMENT_UPDATE";
        OpCode[OpCode["SMSG_STAT_CHANGE"] = 37] = "SMSG_STAT_CHANGE";
        OpCode[OpCode["SMSG_SKILL_CHANGE"] = 38] = "SMSG_SKILL_CHANGE";
        OpCode[OpCode["SMSG_QUEST_SEND_LIST"] = 39] = "SMSG_QUEST_SEND_LIST";
        OpCode[OpCode["CMSG_ENTITY_TARGET"] = 40] = "CMSG_ENTITY_TARGET";
        OpCode[OpCode["CMSG_GAME_LOADED"] = 41] = "CMSG_GAME_LOADED";
        OpCode[OpCode["SMSG_QUEST_PROGRESS_UPDATE"] = 42] = "SMSG_QUEST_PROGRESS_UPDATE";
        OpCode[OpCode["SMSG_DIALOG_PRESENT"] = 43] = "SMSG_DIALOG_PRESENT";
        OpCode[OpCode["CMSG_DIALOG_LINK_SELECTION"] = 44] = "CMSG_DIALOG_LINK_SELECTION";
        OpCode[OpCode["CMSG_CLICK_WARP_REQUEST"] = 45] = "CMSG_CLICK_WARP_REQUEST";
        OpCode[OpCode["SMSG_ENTITY_TELEPORT"] = 46] = "SMSG_ENTITY_TELEPORT";
    })(OpenORPG.OpCode || (OpenORPG.OpCode = {}));
    var OpCode = OpenORPG.OpCode;
})(OpenORPG || (OpenORPG = {}));
var OpenORPG;
(function (OpenORPG) {
    /*
     * A class which contains some basic question information, which can be loaded from the disk.
     */
    var QuestInfo = (function () {
        function QuestInfo(questId) {
            this.questId = questId;
        }
        return QuestInfo;
    })();
    OpenORPG.QuestInfo = QuestInfo;
})(OpenORPG || (OpenORPG = {}));
var OpenORPG;
(function (OpenORPG) {
    var Settings = (function () {
        function Settings() {
            this.settingsNamespace = "orpg_settings";
            /* Login */
            this.autoLoginSet = true;
            this.saveUsername = false;
            this.savePassword = false;
            this.playBGM = true;
            this.playSE = true;
            this._volume = 100;
            // This flag is responsible for forcing WebGL views
            this.debugForceWebGl = false;
            this.attemptLoad();
        }
        Object.defineProperty(Settings.prototype, "volume", {
            get: function () {
                return this._volume;
            },
            set: function (val) {
                this._volume = parseInt(val);
            },
            enumerable: true,
            configurable: true
        });
        Settings.prototype.attemptLoad = function () {
            var settings = localStorage[this.settingsNamespace];
            // Set some defaults if required
            if (!settings) {
                this.autoLoginSet = true;
                this.saveUsername = false;
                this.savePassword = false;
                this.save();
            }
            else {
                // Copy the entire settings into here
                _.extend(this, JSON.parse(settings));
                this.save();
            }
        };
        Settings.getInstance = function () {
            if (Settings._instance === null) {
                Settings._instance = new Settings();
            }
            return Settings._instance;
        };
        Settings.prototype.onChange = function (handler, context) {
            this._handler = handler;
            this._context = context;
        };
        Settings.prototype.flush = function () {
            this._handler.call(this._context);
        };
        /*
         * Persists the entire settings to local storage
         */
        Settings.prototype.save = function () {
            var _this = this;
            var json = JSON.stringify(this, function (key, value) {
                if (typeof value == "function" || value == _this._context) {
                    return undefined;
                }
                if (typeof value == "boolean") {
                    return (value ? 1 : 0);
                }
                return value;
            });
            localStorage[this.settingsNamespace] = json;
        };
        Settings.prototype.reset = function () {
            localStorage.removeItem(this.settingsNamespace);
            this.attemptLoad();
        };
        Settings._instance = null;
        return Settings;
    })();
    OpenORPG.Settings = Settings;
})(OpenORPG || (OpenORPG = {}));
var __extends = this.__extends || function (d, b) {
    for (var p in b)
        if (b.hasOwnProperty(p))
            d[p] = b[p];
    function __() {
        this.constructor = d;
    }
    __.prototype = b.prototype;
    d.prototype = new __();
};
/// <reference path="jquery.d.ts" />
/// <reference path="underscore.d.ts" />
/// <reference path="Scripts/typings/jqueryui/jqueryui.d.ts"/>
/// <reference path="phaser.d.ts" />
/// <reference path="Game/CharacterState.ts" />
/// <reference path="Game/World/Zone.ts" />
/// <reference path="Game/Direction.ts" />
var OpenORPG;
(function (OpenORPG) {
    var Indexer = (function () {
        function Indexer() {
        }
        return Indexer;
    })();
    OpenORPG.Indexer = Indexer;
    var Dictionary = (function () {
        function Dictionary() {
            this._indexer = new Indexer();
        }
        Object.defineProperty(Dictionary.prototype, "indexer", {
            get: function () {
                return this._indexer;
            },
            enumerable: true,
            configurable: true
        });
        Dictionary.prototype.get = function (key) {
            return this.indexer[key];
        };
        Dictionary.prototype.set = function (key, value) {
            this.indexer[key] = value;
            return value;
        };
        Dictionary.prototype.has = function (key) {
            return !(this.indexer[key] === undefined || this.indexer[key] === null);
        };
        Dictionary.prototype.del = function (key) {
            if (!this.has(key)) {
                return false;
            }
            this.indexer[key] = undefined;
            return true;
        };
        return Dictionary;
    })();
    OpenORPG.Dictionary = Dictionary;
    var Angular = (function () {
        function Angular() {
        }
        Object.defineProperty(Angular, "modules", {
            get: function () {
                if (this._modules === undefined || this._modules === null) {
                    this._modules = new Dictionary();
                }
                return this._modules;
            },
            enumerable: true,
            configurable: true
        });
        Angular.safeGet = function (name) {
            if (!this.modules.has(name)) {
                return this.modules.set(name, new Angular.Module(name));
            }
            return this.modules.get(name);
        };
        Object.defineProperty(Angular, "Game", {
            get: function () {
                return this.safeGet('game');
            },
            enumerable: true,
            configurable: true
        });
        Angular.initialize = function () {
            Angular.Game.module.directive('openDialog', function () {
                return {
                    restrict: 'A',
                    link: function (scope, elem, attr, ctrl) {
                        var dialogId = '#' + attr.openDialog;
                        elem.bind('click', function (e) {
                            $(dialogId).dialog('open');
                        });
                    }
                };
            });
            Angular.Game.module.filter("skillFormatter", function () {
                return function (input) {
                    return Math.max(Math.ceil(input), 0) + "s";
                };
            });
        };
        Angular._modules = null;
        return Angular;
    })();
    OpenORPG.Angular = Angular;
    var Angular;
    (function (Angular) {
        var Providers = (function () {
            function Providers() {
            }
            return Providers;
        })();
        Angular.Providers = Providers;
        var Module = (function () {
            function Module(name) {
                this.providers = new Angular.Providers();
                var instance = this;
                this.ngModule = angular.module(name, [], function ($controllerProvider, $compileProvider, $provide) {
                    instance.providers.$controllerProvider = $controllerProvider;
                    instance.providers.$compileProvider = $compileProvider;
                    instance.providers.$provide = $provide;
                });
            }
            Object.defineProperty(Module.prototype, "module", {
                get: function () {
                    return this.ngModule;
                },
                enumerable: true,
                configurable: true
            });
            Module.prototype.flushQueueLength = function () {
                this.queueLength = this.ngModule['_invokeQueue'].length;
            };
            Module.prototype.register = function () {
                var queue = this.ngModule['_invokeQueue'];
                for (var i = this.queueLength; i < queue.length; i++) {
                    var call = queue[i];
                    var provider = this.providers[call[0]];
                    if (provider) {
                        provider[call[1]].apply(provider, call[2]);
                    }
                }
                this.queueLength = queue.length;
            };
            return Module;
        })();
        Angular.Module = Module;
    })(Angular = OpenORPG.Angular || (OpenORPG.Angular = {}));
    Angular.initialize();
    Angular.Game.module.controller('inventoryController', [
        '$scope',
        '$rootScope',
        function ($scope, $rootScope) {
            $scope.gold = 4000;
        }
    ]);
    Angular.Game.module.controller('SettingsController', [
        '$scope',
        '$rootScope',
        function ($scope, $rootScope) {
            $scope.settings = $.extend({}, OpenORPG.Settings.getInstance());
            $scope.save = function () {
                $.extend(OpenORPG.Settings.getInstance(), $scope.settings);
                OpenORPG.Settings.getInstance().flush();
                OpenORPG.Settings.getInstance().save();
            };
            $scope.discard = function () {
                $scope.settings = $.extend({}, OpenORPG.Settings.getInstance());
            };
            $scope.reset = function () {
                OpenORPG.Settings.getInstance().reset();
            };
        }
    ]);
    Angular.Game.module.controller('QuestListController', [
        '$scope',
        function ($scope) {
            var formatter = new OpenORPG.RequirementFormatter();
            $scope.$on('QuestsChanged', function (event, data) {
                $scope.selectQuest($scope.selectedIndex);
            });
            $scope.selectQuest = function (index) {
                $scope.selectedQuest = $scope.playerInfo.quests[index];
                $scope.selectedIndex = index;
                //TODO: We need to make this take all requirements into account; not just the first
                if ($scope.selectedQuest.state == 1) {
                    $scope.localizeRequirements($scope.selectedQuest.currentStep.requirements);
                }
                else {
                    $scope.currentTask = OpenORPG.LocaleManager.getInstance().getString("QuestCompletedFormatter", []);
                }
            };
            $scope.localizeRequirements = function (requirements) {
                $scope.currentTask = " ";
                _.each(requirements, function (requirement, index) {
                    formatter.getFormattedRequirement(requirement.type, requirement.info, $scope.selectedQuest.questInfo.requirementProgress[index].progress, function (result) {
                        $scope.currentTask += result;
                    });
                });
            };
        }
    ]);
    Angular.Game.module.controller('SkillListController', [
        '$scope',
        function ($scope) {
            $scope.getIcon = function (index) {
                var skill = $scope.playerInfo.characterSkills[index];
                var icon = GraphicsUtil.getIconCssFromId(skill.iconId);
                return icon;
            };
            $scope.useSkill = function (skill) {
                if (skill.cooldown <= 0) {
                    var packet = PacketFactory.createSkillUsePacket(skill.id, -1);
                    OpenORPG.NetworkManager.getInstance().sendPacket(packet);
                }
            };
        }
    ]);
    Angular.Game.module.controller('DialogController', [
        '$scope',
        function ($scope) {
            // Respond to a dialog change
            $scope.$on('DialogChanged', function (event, data) {
                $scope.selectQuest($scope.selectedIndex);
            });
            $scope.getIcon = function (index) {
            };
        }
    ]);
    Angular.Game.module.controller('CharacterStatusController', [
        '$scope',
        function ($scope) {
            $scope.getVitalPercent = function (type) {
                var vital = this.playerInfo.characterStats[type];
                if (!vital)
                    return 0;
                var percent = (vital.currentValue / vital.maximumValue) * 100;
                return percent;
            };
            $scope.getVitalLabel = function (type) {
                var vital = this.playerInfo.characterStats[type];
                if (!vital)
                    return "0";
                return vital.currentValue.toString() + "/" + vital.maximumValue.toString();
            };
        }
    ]);
    Angular.Game.module.controller('BottomBarController', [
        '$scope',
        function ($scope) {
            $scope.getExpPercent = function (type) {
                if (!this.playerInfo.player)
                    return 0;
                var exp = this.playerInfo.player.experience;
                if (!exp)
                    return 0;
                var percent = (exp / (this.playerInfo.player.level * 500)) * 100;
                return percent;
            };
            $scope.getExpLabel = function () {
                if (!this.playerInfo.player)
                    return "";
                return this.playerInfo.player.experience + "/" + this.playerInfo.player.level * 500;
            };
        }
    ]);
    Angular.Game.module.controller('CharacterWindowController', [
        '$scope',
        function ($scope) {
            // We can do some cool stuff here if we want to, but otherwise we're ok 
            $scope.getStatNameFromIndex = function (index) {
                var names = [];
                for (var n in OpenORPG.StatTypes) {
                    if (typeof OpenORPG.StatTypes[n] === 'number')
                        names.push(n);
                }
                return names[index];
            };
        }
    ]);
    Angular.Game.flushQueueLength();
})(OpenORPG || (OpenORPG = {}));
window.onload = function () {
    // Setup underscore
    var game = new OpenORPG.Game();
};
//# sourceMappingURL=app.js.map