using Server.Infrastructure.Network.Packets;

namespace Server.Game.Network.Packets
{
    public struct ClientLoginRequestPacket : IPacket
    {
        public string Password; //TODO: send something more secure
        public string Username;

        public OpCodes OpCode
        {
            get { return OpCodes.CMSG_LOGIN_REQUEST; }
        }
    }
}