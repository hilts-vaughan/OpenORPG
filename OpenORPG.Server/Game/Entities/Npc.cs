using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Infrastructure.Quests;
using Server.Infrastructure.World;

namespace Server.Game.Entities
{
    
    /// <summary>
    /// An NPC is an object in the game world that can be 
    /// </summary>
    public class Npc : Entity, IQuestProvider
    {

        public Npc(NpcInfo npcInfo)
        {
            
        }

        public List<Quest> Quests { get; set; }
    }


}
