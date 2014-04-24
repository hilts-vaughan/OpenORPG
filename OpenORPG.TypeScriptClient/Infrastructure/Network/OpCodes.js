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
//# sourceMappingURL=OpCodes.js.map
