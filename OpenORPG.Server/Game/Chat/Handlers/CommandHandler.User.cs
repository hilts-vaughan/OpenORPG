using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Game.Entities;

namespace Server.Game.Chat.Handlers
{
    /// <summary>
    /// Parses some commands and then allows the <see cref="ChatService"/>
    /// </summary>
    public static class ChatCommandHandler
    {

        [ChatHandler("Logout")]
        public static void LogoutCommandHandler(Player player, List<string> arguments)
        {
                player.Client.Connection.Disconnect("The player logged out forcefully.");            
        }

        [ChatHandler("Time")]
        public static void ServerTimeHandler(Player player, List<string> arguments)
        {
           
        }

    }
}
