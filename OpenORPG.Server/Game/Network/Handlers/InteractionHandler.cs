using System.Linq;
using Server.Game.Entities;
using Server.Game.Network.Packets;
using Server.Game.Network.Packets.Client;
using Server.Game.Network.Packets.Server;
using Server.Game.Quests;
using Server.Game.Zones;
using Server.Infrastructure.Logging;
using Server.Infrastructure.Network.Handlers;
using Server.Utils.Math;

namespace Server.Game.Network.Handlers
{
    public static class InteractionHandler
    {

        /// <summary>
        /// This handler is invoked when a client makes a request with the action key to interact with
        /// an entity in the game world. Typically, this is an <see cref="Npc"/> but not always.
        /// 
        /// In the case there is nothing to interact with, the client will get nothing back. Interactions
        /// will typically be stateless unless otherwise needed.
        /// </summary>
        /// <param name="client">This is the game client that initiated the request</param>
        /// <param name="packet">This is the packet that was sent</param>
        [PacketHandler(OpCodes.CMSG_INTERACT_REQUEST)]
        public static void OnPlayerInteract(GameClient client, ClientRequestInteractionPacket packet)
        {
            var hero = client.HeroEntity;

            // It's only possible to interact with things standing still
            if (hero == null && hero.CharacterState == CharacterState.Idle)
                return;

            // Get who to interact with
            var interactWith = GetNearestInteractable(hero);

            if (interactWith != null)
            {

                Logger.Instance.Debug("{0} is interacting with {1}.", hero.Name, interactWith.Name);

                // Get the first quest
                var quest = interactWith.Quests[0];

                if (QuestManager.Instance.CanPlayerGetQuest(quest, hero))
                {
                    var p = new ServerSendQuestOfferPacket(quest.QuestId);
                    hero.Client.Send(p);
                }


            }


        }


        /// <summary>
        /// Retrieves the nearest interactable Npc to the player.
        /// </summary>
        /// <param name="player">The player to check against</param>
        /// <returns></returns>
        private static Npc GetNearestInteractable(Player player)
        {
            float highestDistance = float.MaxValue;
            Npc target = null;

            foreach (var character in player.Zone.Npcs)
            {
                var distance = Vector2.Distance(character.Position, player.Position);

                if (distance < highestDistance && distance < 64)
                {
                    highestDistance = distance;
                    target = character;
                }
            }
            return target;
        }


    }
}
