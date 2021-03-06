﻿using System.Collections.Generic;
using Server.Infrastructure.Network.Packets;
using Server.Utils.Math;

namespace Server.Game.Network.Packets
{
    /// <summary>
    /// The client sends this when a request to be moved is made.
    /// </summary>
    public struct ClientMovementRequestPacket : IPacket
    {
        /// <summary>
        /// This is the current position that the client claims to be at
        /// </summary>
        public Vector2 CurrentPosition { get; set; }

        /// <summary>
        /// A flag that is set when a player has terminated their movement
        /// </summary>
        public bool Terminates { get; set; }

        /// <summary>
        /// This is the current direction the client claims to be willing to head towards
        /// </summary>
        public Direction Direction { get; set; }

        public OpCodes OpCode
        {
            get { return OpCodes.CMSG_MOVEMENT_REQUEST; }
        }
    }
}