using System.Collections.Generic;
using Server.Infrastructure.Network.Packets;
using Server.Utils.Math;

namespace Server.Game.Network.Packets
{
    public struct ServerMobMovementPacket : IPacket
    {
        public Vector2 Goal;
        public ulong MobId;

        public ServerMobMovementPacket(ulong mobId, Vector2 goal)
        {
            MobId = mobId;
            Goal = goal;
        }

        public OpCodes OpCode
        {
            get { return OpCodes.SMSG_MOB_MOVEMENT; }
        }
    }

    public struct ServerMobCreatePacket : IPacket
    {
        public List<AttributeKeyValue> Attributes;
        public ulong Id;

        public string Name;
        public Vector2 Position;
        public string Sprite;
        //public Attributes;


        public OpCodes OpCode
        {
            get { return OpCodes.SMSG_MOB_CREATE; }
        }
    }
}