module OpenORPG {

    // This manager is used for maintaing and hooking into chat related functionality in the game
    // If it has to do with talking in the game world, you'll find it here.
    export class ChatManager {

        private _chatChannels: Array<ChatChannel> = new Array<ChatChannel>();
        private _chatLogElement: string = "chatlog";

        constructor() {

            // Hook into the DOM
            $("#chatmessage").on('keypress', (event: JQueryEventObject) => {
                if (event.which == 13) {

                    var packet = PacketFactory.createChatPacket(0, $("#chatmessage").val());
                    NetworkManager.getInstance().sendPacket(packet);
                    $("#chatmessage").val("");
                }
            });


            this.setupNetworkHandlers();
        }

        setupNetworkHandlers() {
            var network = NetworkManager.getInstance();

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

                this.processIncomingMessage(message, id);
            });
        }

        processIncomingMessage(message: string, id: number) {
            var chatChannel = this._chatChannels[id];

            if (chatChannel != null) {
                var chatLog = $("#chatlog");
                chatLog.val(chatLog.val() + message + "\n");

            }

        }



    }


} 