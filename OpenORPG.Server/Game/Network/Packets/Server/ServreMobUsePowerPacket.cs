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

    public struct ServerMobPlayAnimation : IPacket
    {
        public string AnimationName;
        public float Duration;
        public ulong MobId;
        public AnimationType Type;

        public ServerMobPlayAnimation(ulong mobId, AnimationType animationType, string animationName, float duration)
        {
            MobId = mobId;
            Type = animationType;
            AnimationName = animationName;
            Duration = duration;
        }

        public OpCodes OpCode
        {
            get { return OpCodes.SMSG_MOB_PLAY_ANIMATION; }
        }
    }
}