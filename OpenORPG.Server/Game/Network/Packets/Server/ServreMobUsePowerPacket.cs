using Server.Infrastructure.Network.Packets;

namespace Server.Game.Network.Packets
{
    public enum AnimationType
    {
        Idle = 1,
        Walking = 2,
        Interacting = 3,
        //GettingHit = 4,
        UsingSkill = 5,
        Dead = 6
    }

    public struct ServerSkillUseResult : IPacket
    {
        public ServerSkillUseResult(ulong userId, ulong targetId, long damage, long skillId) : this()
        {
            UserId = userId;
            TargetId = targetId;
            Damage = damage;
            SkillId = skillId;
        }

        /// <summary>
        /// The ID of the user of this skill. This is used to broadcast to all entities that
        /// playing the 'finish' skill animation is now okay
        /// </summary>
        public ulong UserId { get; set; }

        /// <summary>
        /// The ID of the user of the result end of this skill. This is used to broadcast  
        /// </summary>
        public ulong TargetId { get; set; }

        /// <summary>
        /// This is the amount of damage the <see cref="TargetId"/> incurred. 
        /// This is used for client display purposes only.
        /// </summary>
        public long Damage { get; set;  }

        /// <summary>
        /// The skill that was performed
        /// </summary>
        public long SkillId { get; set; }

        public OpCodes OpCode
        {
            get { return OpCodes.SMSG_SKILL_USE_RESULT; }
        }
    }
}