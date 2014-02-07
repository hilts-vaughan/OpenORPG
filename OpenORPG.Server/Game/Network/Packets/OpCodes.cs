namespace Server.Game.Network.Packets
{
    public enum OpCodes : ushort
    {
        CMSG_LOGIN_REQUEST = 0,
        SMSG_LOGIN_RESPONSE = 1,
        SMSG_HERO_LIST = 2,
        CMSG_HERO_SELECT = 3,
        CMSG_HERO_CREATE = 4,

        SMSG_GAME_OBJECT_UPDATE = 5,
        SMSG_HERO_SELECT_RESPONSE = 6,
        SMSG_HERO_CREATE_RESPONSE = 7,
        SMSG_ZONE_CHANGED = 8,
        SMSG_MOB_CREATE = 9,

        SMSG_CHAT_MESSAGE = 10,
        SMSG_JOIN_CHANNEL = 11,
        CMSG_CHAT_MESSAGE = 12,
        SMSG_MOB_DESTROY = 13,
        SMSG_ATTRIBUTES_CHANGED = 14,
        CMSG_MOVEMENT_REQUEST = 15,
        SMSG_MOB_MOVEMENT = 16,
        CMSG_USE_POWER = 17,
        SMSG_MOB_PLAY_ANIMATION = 18,
        SMSG_FLOATING_NUMBER = 19
    }
}