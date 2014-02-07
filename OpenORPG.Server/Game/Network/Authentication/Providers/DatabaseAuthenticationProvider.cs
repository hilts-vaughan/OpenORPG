using System.Linq;
using Server.Game.Database;
using Server.Game.Database.Models;

namespace Server.Game.Network.Authentication.Providers
{
    /// <summary>
    ///     The database authentication provider is a concrete implementation of an <see cref="IAuthenticationProvider" />
    ///     which provides a means to login via a hosted database.
    /// </summary>
    public class DatabaseAuthenticationProvider : IAuthenticationProvider
    {
        public bool Authenticate(string username, string password, out UserAccount account)
        {
            using (var context = new GameDatabaseContext())
            {
                account = context.Accounts.FirstOrDefault((a) => a.Username == username);

                if (account == null)
                    return false;
                return account.Password == password;
            }
        }
    }
}