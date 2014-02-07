using Server.Game.Database;
using Server.Game.Database.Models;
using Server.Game.Network.Authentication;
using Server.Game.Network.Authentication.Providers;
using Server.Game.Network.Packets;
using Server.Infrastructure.Logging;
using Server.Infrastructure.Network.Handlers;

namespace Server.Game.Network.Handlers
{
    public class LoginHandler
    {
        private static readonly IAuthenticationProvider AuthenticationProvider = new DatabaseAuthenticationProvider();

        //[PacketHandler(OpCodes.CMSG_HERO_SELECT)]        
        //public static void OnHeroSelect(GameClient client, ClientHeroSelectPacket packet)
        //{

        //}

        [PacketHandler(OpCodes.CMSG_LOGIN_REQUEST)]
        public static void OnLoginRequest(GameClient client, ClientLoginRequestPacket packet)
        {
            Logger.Instance.Info("LoginHandler.OnLoginRequest", client, packet);

            UserAccount account;

            bool loginSuccessful = AuthenticationProvider.Authenticate(packet.Username, packet.Password, out account);

            if (!loginSuccessful)
            {
                client.Send(new ServerLoginResponsePacket(LoginStatus.InvalidCredentials));
                return;
            }

            if (account.IsOnline)
            {
                client.Send(new ServerLoginResponsePacket(LoginStatus.AlreadyLoggedIn));
                return;
            }

            client.Account = account;

            using (var context = new GameDatabaseContext())
            {
                context.Accounts.Attach(account);

                account.IsOnline = true;

                context.SaveChanges();

                // Load heroes
                context.Entry(account).Collection(a => a.Heroes).Load();
            }


            client.Send(new ServerLoginResponsePacket(LoginStatus.OK));


            HeroSelectionHandler.SendHeroList(client);
        }


        public static void Logout(GameClient client)
        {
            if (client.Account == null)
                return;

            UserAccount account = client.Account;

            using (var context = new GameDatabaseContext())
            {
                context.Accounts.Attach(account);

                account.IsOnline = false;

                context.SaveChanges();
            }

            client.Account = null;


            if (client.Zone == null)
                return;

            if (client.HeroEntity == null)
                return;

            client.Zone.OnClientLeave(client);
        }
    }
}