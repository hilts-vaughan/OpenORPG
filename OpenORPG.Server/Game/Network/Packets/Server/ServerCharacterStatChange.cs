using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Game.Entities;
using Server.Infrastructure.Network.Packets;

namespace Server.Game.Network.Packets.Server
{
    /// <summary>
    /// 
    /// </summary>
    public class ServerCharacterStatChange : IPacket
    {
        public OpCodes OpCode
        {
            get { return OpCodes.SMSG_STAT_CHANGE; }
        }

        public StatTypes Stat { get; set; }
        public long CurrentValue { get; set; }
        
        public long MaximumValue { get; set; }

        public long CharacterId { get; set; }

        public ServerCharacterStatChange(StatTypes stat, long currentValue, long maximumValue, long characterId)
        {
            Stat = stat;
            CurrentValue = currentValue;
            MaximumValue = maximumValue;
            CharacterId = characterId;
        }

    }
}
