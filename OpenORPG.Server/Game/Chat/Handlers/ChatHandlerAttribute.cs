using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Game.Chat.Handlers
{
    public class ChatHandlerAttribute : Attribute
    {
        public ChatHandlerAttribute(string command)
        {
            Command = command;
        }

        public string Command { get; set; }

    }
}
