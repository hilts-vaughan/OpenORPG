using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Game.Entities;
using Server.Game.Network.Packets;
using Server.Game.Zones;
using Server.Infrastructure.World;
using Server.Infrastructure.World.Systems;

namespace Server.Game.Chat
{
    /// <summary>
    /// A chat service is responsible for creating and parsing specific requests for a very specific zone.
    /// </summary>
    public class ChatService : GameSystem
    {
        // Creates a new chat command parser for usage
        private readonly ChatCommandParser _chatCommandParser = new ChatCommandParser();

        public ChatService(Zone world) : base(world)
        {
        }

        public override void Update(float frameTime)
        {

        }

        public override void OnEntityAdded(Entity entity)
        {
            if (entity is Player)
                OnPlayerAdded(entity as Player);

        }


        public override void OnEntityRemoved(Entity entity)
        {
            if (entity is Player)
                OnPlayerRemoved(entity as Player);
        }

        private void OnPlayerAdded(Player player)
        {

        }

        private void OnPlayerRemoved(Player player)
        {

        }


        /// <summary>
        /// Handles a message from an external service, a bit of a dirty hack but it works.
        /// 
        /// Returns whether or not a command was parsed
        /// //TODO: Refactor me to be more decoupled from the actual handlers and be more resilent
        /// </summary>
        /// <param name="player"></param>
        /// <param name="packet"></param>
        public bool HandleMessage(Player player, ClientChatMessagePacket packet)
        {
            return _chatCommandParser.ParseAndHandleMessage(player, packet.Message);
        }


    }
}
