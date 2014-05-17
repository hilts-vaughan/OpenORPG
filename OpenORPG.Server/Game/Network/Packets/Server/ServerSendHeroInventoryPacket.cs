using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Game.Network.Packets.Client;
using Server.Game.Storage;
using Server.Infrastructure.Network.Packets;

namespace Server.Game.Network.Packets.Server
{

    public class ServerSendHeroStoragePacket : IPacket
    {
        public OpCodes OpCode
        {
            get { return OpCodes.SMSG_STORAGE_HERO_SEND;  }
        }

        /// <summary>
        /// The storage to be sent to the user
        /// </summary>
        public ItemStorage ItemStorage { get; set; }

        public StorageType StorageType { get; set; }

        public ServerSendHeroStoragePacket(ItemStorage itemStorage, StorageType storageType)
        {
            ItemStorage = itemStorage;
            StorageType = storageType;
        }

    }
}
