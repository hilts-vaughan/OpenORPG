using Server.Game.Database.Models;

namespace Server.Game.Network.Authentication
{
    /// <summary>
    ///     An interface for defining the base of all authenticators needed to sign into the game
    /// </summary>
    public interface IAuthenticationProvider
    {
        bool Authenticate(string username, string password, out UserAccount account);
    }
}