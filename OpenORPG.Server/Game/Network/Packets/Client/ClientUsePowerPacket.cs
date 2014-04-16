using Server.Infrastructure.Network.Packets;

namespace Server.Game.Network.Packets
{
    public struct ClientUseSkillPacket : IPacket
    {
        public long SkillId { get; set; }

        public ulong TargetId { get; set; }

        public ClientUseSkillPacket(long skillId, ulong targetId) : this()
        {
            SkillId = skillId;
            TargetId = targetId;
        }

        public OpCodes OpCode
        {
            get { return OpCodes.CMSG_USE_SKILL; }
        }

    }
}