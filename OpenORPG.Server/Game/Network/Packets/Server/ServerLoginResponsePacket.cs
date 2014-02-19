using Server.Infrastructure.Network.Packets;

namespace Server.Game.Network.Packets
{
    public enum LoginStatus
    {
        OK = 1,
        InvalidCredentials = 2,
        AlreadyLoggedIn = 3,
        ServerLocked = 4,
        AccountLocked = 5
    }

    public struct ServerLoginResponsePacket : IPacket
    {
        public LoginStatus Status;

        public ServerLoginResponsePacket(LoginStatus loginStatus)
        {
            Status = loginStatus;
        }

        public OpCodes OpCode
        {
            get { return OpCodes.SMSG_LOGIN_RESPONSE; }
        }
    }
}