﻿module OpenORPG {

    export enum OpCode {
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
        CMSG_USE_SKILL = 17,
        SMSG_SKILL_USE_RESULT = 18,
        SMSG_FLOATING_NUMBER = 19,

        SMSG_ENTITY_PROPERTY_CHANGE = 20,
        SMSG_ENTITY_MOVE = 21,
        CMMSG_ZONE_CHANGE = 22,
        CMSG_ITEM_USE = 23,

        SMSG_LEAVE_CHAT_CHANNEL = 24,
        CMSG_INTERACT_REQUEST = 25,

        CMSG_QUEST_REQUEST_COMPLETION = 26,
        CMSG_QUEST_ACCEPT = 27,
        SMSG_QUEST_ACCEPT_RESULT = 28,
        SMSG_QUEST_COMPLETE_RESULT = 29,
        SMSG_SERVER_OFFER_QUEST = 30,
        CMSG_STORAGE_MOVE_SLOT = 31,
        SMSG_STORAGE_HERO_SEND = 32,
        CMSG_STORAGE_DROP = 33,
        SMSG_SEND_GAMEMESSAGE = 34,
        CMSG_UNEQUIP_ITEM = 35,
        SMSG_EQUIPMENT_UPDATE = 36,
        SMSG_STAT_CHANGE = 37,
        SMSG_SKILL_CHANGE = 38,
        SMSG_QUEST_SEND_LIST = 39,
        CMSG_ENTITY_TARGET = 40,
        CMSG_GAME_LOADED = 41,
        SMSG_QUEST_PROGRESS_UPDATE = 42,
        SMSG_DIALOG_PRESENT = 43,
        CMSG_DIALOG_LINK_SELECTION = 44,
        CMSG_CLICK_WARP_REQUEST = 45,
        SMSG_ENTITY_TELEPORT = 46
    }

} 