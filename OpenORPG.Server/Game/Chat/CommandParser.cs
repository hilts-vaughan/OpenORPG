using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Server.Game.Chat.Handlers;
using Server.Game.Entities;
using Server.Game.Network.Packets;
using Server.Infrastructure.Network;
using Server.Infrastructure.Network.Handlers;

namespace Server.Game.Chat
{


    /// <summary>
    /// Responsible for parsing the commands sent by the client.
    /// </summary>
    public class ChatCommandParser
    {
        private readonly Dictionary<string, ChatCommandHandlerDelegate> _packetHandlers = new Dictionary<string, ChatCommandHandlerDelegate>();

        /// <summary>
        /// A signature template for methods that plan on handling a command
        /// </summary>
        /// <param name="player"></param>
        /// <param name="arguments"></param>
        public delegate void ChatCommandHandlerDelegate(Player player, List<string> arguments);

        public ChatCommandParser()
        {
            // Generate a map of delegates for the handlers to be processed against
            ReflectionHelper.GetMethodsWithAttritube<ChatHandlerAttribute>((method, attribute) =>
            {
                string command = attribute.Command.ToLower();
                var del =
                    (ChatCommandHandlerDelegate)Delegate.CreateDelegate(typeof(ChatCommandHandlerDelegate), method);
                _packetHandlers.Add(command, del);
            });

        }

        /// <summary>
        /// Consumes a specific message
        /// </summary>
        /// <param name="player"></param>
        /// <param name="message"></param>
        public bool ParseAndHandleMessage(Player player, string message)
        {
            if (message.Length > 2)
            {
                var command = message.Split(" ".ToCharArray())[0].ToLower().Substring(1);
                var arguments = GetArgumentList(message);

                // Fire off the handler now
                if (_packetHandlers.ContainsKey(command))
                {
                    _packetHandlers[command](player, arguments);
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Given a message, spits out a parameter list of strings for the.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        private List<string> GetArgumentList(string message)
        {

            var arguments = Regex
                .Matches(message, @"(?<match>\w+)|\""(?<match>[\w\s]*)""")
                .Cast<Match>()
                .Select(m => m.Groups["match"].Value)
                .ToList();
            if (arguments.Count > 0)
                arguments.RemoveAt(0);
            return arguments;
        }
    }
}
