using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Infrastructure.Network.Packets;

namespace Server.Game.Network.Packets.Client
{
    public enum StorageType
    {
        Inventory
    }
  

    public struct ClientStorageMoveSlotPacket : IPacket
    {
        public ClientStorageMoveSlotPacket(long sourceSlot, long destSlot, StorageType storageType) : this()
        {
            SourceSlot = sourceSlot;
            DestSlot = destSlot;
            StorageType = storageType;
        }

        public OpCodes OpCode
        {
            get { return OpCodes.CMSG_STORAGE_MOVE_SLOT; }
        }

        public long SourceSlot { get; set; }
        public long DestSlot { get; set; }

        public StorageType StorageType {  get; set;}

    }
}
