using System.Collections.Generic;
using Server.Infrastructure.Network.Packets;

namespace Server.Game.Network.Packets
{
    public struct AttributeKeyValue
    {
        public AttributeName Name;
        public float Value;

        public AttributeKeyValue(AttributeName name, float value)
            : this()
        {
            Name = name;
            Value = value;
        }
    }

    /// <summary>
    ///     A packet sent to notify clients of a changed attribute on a given entity
    ///     This packet is sent once per entity that has had attributes changed
    ///     This packet is sent by the <see cref="AttributeSyncSystem" /> and should only
    ///     be sent by it.
    /// </summary>
    public struct ServerAttributesChangedPacket : IPacket
    {
        public List<AttributeKeyValue> Attributes;

        /// <summary>
        ///     The ID of the object that had attributes changed
        /// </summary>
        public ulong ObjectId;


        public ServerAttributesChangedPacket(List<AttributeKeyValue> attributes, ulong objectId)
        {
            ObjectId = objectId;
            Attributes = attributes;
        }


        public OpCodes OpCode
        {
            get { return OpCodes.SMSG_ATTRIBUTES_CHANGED; }
        }
    }
}