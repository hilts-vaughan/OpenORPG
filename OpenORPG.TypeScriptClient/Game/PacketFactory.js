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
})(PacketFactory || (PacketFactory = {}));
//# sourceMappingURL=PacketFactory.js.map
