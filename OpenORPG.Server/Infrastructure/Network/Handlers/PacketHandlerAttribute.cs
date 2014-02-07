using System;
using Server.Game.Network.Packets;

namespace Server.Infrastructure.Network.Handlers
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    internal sealed class PacketHandlerAttribute : Attribute
    {
        public PacketHandlerAttribute(OpCodes opCode)
        {
            OpCode = opCode;
        }

        public OpCodes OpCode { get; set; }
    }
}