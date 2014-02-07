using Server.Infrastructure.Network.Packets;

namespace Server.Game.Network.Packets
{
    public enum FloatingNumberType
    {
        Damage,
        Healing,
        Gold,
        Exp,
        Miss,
        Dodge,
        Parry,
    }

    public struct ServerFloatingNumberPacket : IPacket
    {
        public ulong MobId;
        public int Number;
        //public Vector2 Position;
        public FloatingNumberType Type;

        public ServerFloatingNumberPacket(ulong mobId, int number, FloatingNumberType type)
        {
            MobId = mobId;
            Number = number;
            Type = type;
        }

        public OpCodes OpCode
        {
            get { return OpCodes.SMSG_FLOATING_NUMBER; }
        }
    }
}