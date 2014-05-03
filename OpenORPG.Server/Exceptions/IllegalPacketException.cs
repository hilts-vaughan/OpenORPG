using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Exceptions
{
    /// <summary>
    /// This exception is thrown when a packet is recieved on the server but cannot be parsed for some given reason.
    /// </summary>
    public class IllegalPacketException : Exception
    {
        public IllegalPacketException(string data, Exception innerException)
            : base("The packet could not be parsed described by the data: " + data, innerException)
        {
            PacketData = data;
        }

        public string PacketData { get; set; }
    }
}
