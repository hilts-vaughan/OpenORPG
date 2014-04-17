using Server.Game.Database.Models;
using Server.Game.Entities;
using Server.Game.Zones;
using Server.Infrastructure.Logging;
using Server.Infrastructure.Network.Packets;
using Server.Infrastructure.World;

namespace Server
{
    /// <summary>
    ///     A game client represents a single, connected client on the server.
    /// </summary>
    public class GameClient
    {
        public GameClient(Connection connection)
        {
            Connection = connection;
        }

        /// <summary>
        ///     The connection of this client. This is immutable.
        ///     You should never reuse GameClients.
        /// </summary>
        public Connection Connection { get; private set; }


        /// <summary>
        ///     This is the current account of the client if they have logged in, otherwise null.
        /// </summary>
        public UserAccount Account { get; set; }


        /// <summary>
        ///     This is the current zone the GameClient is in
        /// </summary>
        public Zone Zone { get; set; }

        /// <summary>
        ///     This is the game object that is currently assigned to the player in the given Zone
        /// </summary>
        public Player HeroEntity { get; set; }


        /// <summary>
        ///     Sends a packet to the GameClient
        /// </summary>
        /// <param name="packet">The packet to send to the game clients connection</param>
        /// <param name="T">The type of the packet to send</param>
        /// </para>
        public void Send<T>(T packet) where T : IPacket
        {
            Logger.Instance.Debug("GameClient.Send({0})", packet.GetType().Name);
            Connection.Send(packet);
        }
    }
}