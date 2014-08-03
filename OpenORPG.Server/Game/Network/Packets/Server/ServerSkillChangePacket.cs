using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Game.Combat;
using Server.Infrastructure.Network.Packets;

namespace Server.Game.Network.Packets.Server
{

    /// <summary>
    /// This packet is sent when a player requires knowledge of a new skill that was learned.
    /// </summary>
    public class ServerSkillChangePacket : IPacket
    {

        public List<Skill>  Skills { get; set; }

        public ServerSkillChangePacket(List<Skill> skills)
        {
            Skills = skills;
        }

        public OpCodes OpCode
        {
            get { return OpCodes.SMSG_SKILL_CHANGE; }
        }
    }
}
