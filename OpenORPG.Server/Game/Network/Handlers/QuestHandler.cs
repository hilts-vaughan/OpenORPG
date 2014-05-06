using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Game.Network.Packets;
using Server.Game.Network.Packets.Client;
using Server.Game.Network.Packets.Server;
using Server.Game.Quests;
using Server.Game.Zones;
using Server.Infrastructure.Logging;
using Server.Infrastructure.Network.Handlers;
using Server.Infrastructure.Quests;

namespace Server.Game.Network.Handlers
{
    /// <summary>
    /// This class is responsible for handling all network requests involving quests. 
    /// </summary>
    public static class QuestHandler
    {

        [PacketHandler(OpCodes.CMSG_QUEST_REQUEST_COMPLETION)]
        public static void OnPlayerRequestQuestCompletion(GameClient client, ClientQuestRequestCompletionPacket packet)
        {
            var player = client.HeroEntity;
            var result = new ServerQuestCompleteResultPacket(QuestCompleteResult.CannotComplete);

            var quest = FindQuestInZoneOrReturnNull(player.Zone, packet.QuestId);

            // If this quests exists somewhere in this zone for completion
            if (quest != null)
            {
                bool didComplete = quest.TryCompleteQuest(player);

                if (didComplete)
                {
                    Logger.Instance.Info("{0} has completed the quest {1} [#{2}.", player.Name, quest.Name,
                        quest.QuestId);
                    result.Result = QuestCompleteResult.Success;
                }
                else
                    result.Result = QuestCompleteResult.CannotGetReward;

            }
        }

        /// <summary>
        /// Handles a request to accept a quest from a client
        /// </summary>
        /// <param name="client"></param>
        /// <param name="packet"></param>
        [PacketHandler(OpCodes.CMSG_QUEST_ACCEPT)]
        public static void OnPlayerAcceptQuest(GameClient client, ClientAcceptQuestPacket packet)
        {
            var player = client.HeroEntity;
            var result = new ServerQuestAcceptResultPacket(false);

            // We need to query the surrounding NPCs to see if they can give us the quest we need
            var quest = FindQuestInZoneOrReturnNull(player.Zone, packet.QuestId);

            if (quest != null)
            {
                // Give the player the quest if it's at all possible
                if (QuestManager.Instance.CanPlayerGetQuest(quest, player))
                {
                    QuestManager.Instance.GivePlayerQuest(quest, player);
                    result.Succeeded = true;
                }
            }

            // Notify the client
            client.Send(result);
        }




        private static Quest FindQuestInZoneOrReturnNull(Zone zone, long questId)
        {
            foreach (var npc in zone.Npcs)
            {
                foreach (var quest in npc.Quests)
                {
                    if (quest.QuestId == questId)
                        return quest;
                }
            }

            return null;
        }


    }
}
