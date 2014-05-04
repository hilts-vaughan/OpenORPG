using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Game.Database.Models;
using Server.Game.Network.Packets.Server;
using Server.Infrastructure.Quests;
using Server.Infrastructure.World;
using Server.Utils.Math;

namespace Server.Game.Entities
{

    /// <summary>
    /// An NPC is an object in the game world that can be 
    /// </summary>
    public class Npc : Entity, IQuestProvider
    {

        public Npc(NpcTemplate npcTemplate)
            : base(npcTemplate.Sprite)
        {
            Name = npcTemplate.Name;


            Quests = new List<Quest>();
            foreach (var questEntry in npcTemplate.Quests)
            {
                var quest = new Quest(questEntry);
                Quests.Add(quest);
            }

        }

        public List<Quest> Quests { get; set; }

        protected override void MoveEntity(Vector2 location)
        {
            base.MoveEntity(location);

            _position = location;

            // Syncs the position to the rest of the clients that can see this
            SyncPosition();

        }

        private void SyncPosition()
        {
            // We only need to bother syncing if we're attached somewhere yet
            if (Zone != null)
            {
                // Send this packet to all interested parties
                var newPacket = new ServerEntityMovementPacket(_position, Direction, this.Id);
                Zone.SendToEntitiesInRangeExcludingSource(newPacket, this);
            }

        }



    }


}
