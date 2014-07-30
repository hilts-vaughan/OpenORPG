namespace Server.Infrastructure.Configuration
{
    /// <summary>
    ///     Holds various configuration settings for the server.
    /// </summary>
    public class ServerSettings
    {

        /// <summary>
        /// A simple string that is broadcasted to all clients when they connect to the game world for the first time.
        /// </summary>
        public string MessageOfTheDay { get; set; }

    }
}