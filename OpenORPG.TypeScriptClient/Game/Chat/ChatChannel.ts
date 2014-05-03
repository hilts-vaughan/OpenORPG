module OpenORPG {
    
    export class ChatChannel {
        
        public channelId: number;
        public channelName: string;
        public channelType: ChannelType;

        constructor(channelId: number, channelName: string, chatType : ChannelType) {
            this.channelId = channelId;
            this.channelName = channelName;
            this.channelType = chatType;
        }
    }

    export enum ChannelType {
        Global,
        Zone,
        Administration,
        Trade,
        Party,
        Guild,
        Custom,
    }


} 