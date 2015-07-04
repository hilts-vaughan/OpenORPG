module OpenORPG {
    // This manager is used for maintaing and hooking into chat related functionality in the game
    // If it has to do with talking in the game world, you'll find it here.
    export class ChatManager {

        private _chatChannels: Array<ChatChannel> = new Array<ChatChannel>();
        private commandParser = new CommandParser();
        private chatCommandHandler: ChatCommandHandler = new ChatCommandHandler();

        private _chatLogElement: string = "chatlog";
        private channelColorMap: Array<string> = new Array<string>();

        constructor() {
            // Hook into the DOM
            $(document).on('keypress', "#chatmessage", (event: JQueryEventObject) => {
                if (event.which == 13) {

                    var message: string = $("#chatmessage").val();
                    var command: CommandType = this.commandParser.parseMessageType(message);

                    if (command == CommandType.UnknownCommand) {
                        this.sendMessageToServer(message);
                    } else {
                        this.chatCommandHandler.handleCommand(command, message);
                    }

                    $("#chatmessage").val(""); // clear chat box
                    event.preventDefault();
                }
            });

            $.getJSON("assets/config/chat_color_map.json", data => {
                var i = 0;
                for (var key in data) {
                    var value = data[key];
                    this.channelColorMap[i] = value;
                    i++;
                }
            });

            this.setupNetworkHandlers();
        }

        sendMessageToServer(message: string) {
            var packet = PacketFactory.createChatPacket(0, message);
            NetworkManager.getInstance().sendPacket(packet);
        }

        setupNetworkHandlers() {
            var network = NetworkManager.getInstance();

            // Init
            LocaleManager.getInstance();

            // Handle channel registration
            network.registerPacket(OpCode.SMSG_JOIN_CHANNEL, (packet) => {
                var channel: ChatChannel = new ChatChannel(packet.channelId, packet.channelName, packet.channelType);
                this._chatChannels[channel.channelId] = channel;
            });

            network.registerPacket(OpCode.SMSG_LEAVE_CHAT_CHANNEL, (packet) => {
                delete this._chatChannels[packet.channelId];
            });

            network.registerPacket(OpCode.SMSG_CHAT_MESSAGE, (packet) => {
                var message = packet.message;
                var id = packet.channelId;
                var sender = packet.sender;

                this.processIncomingMessage(sender, message, id);
            });

            network.registerPacket(OpCode.SMSG_SEND_GAMEMESSAGE, (packet) => {
                var messageType = packet.messageType;
                var args = packet.arguments;

                var message: string = LocaleManager.getInstance().getString(messageType, args);
                this.addMessage(message, "", ChannelType.System);
            });


            // Setup our handlers for doing stuff here; not sure how we'll support more broad commands as of yet
            this.chatCommandHandler.registerCallback(CommandType.Echo, (args: Array<string>) => {
                this.addMessage(args.join(" "), "", ChannelType.System);
            }
                );

        }

        processIncomingMessage(sender: string, message: string, id: number) {
            var chatChannel = this._chatChannels[id];

            if (chatChannel != null) {
                this.addMessage(message, sender + ": ", chatChannel.channelType);
            } else {
                Logger.warn("ChatManager - Failed to find chat channel with id of " + id + ". Was it not registered");
            }
        }

        addMessage(message: string, user: string = "", channel: number = ChannelType.System) {
            $.get("assets/hud/chat/chat_message_line.html", html => {
                var data =
                    {
                        playerName: user,
                        message: message
                    }

                var chatLineHtml = _.template(html, data);
                var chatElement = $(chatLineHtml);
                $(chatElement).css("color", this.channelColorMap[channel]);
                $("#chatlog").append(chatElement);

                // Scroll down
                $("#chatlog").animate({ scrollTop: $("#chatlog")[0].scrollHeight }, 1000);
            });
        }
    }
} 
