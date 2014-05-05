using System.Data.Entity;
using System.Linq;
using Server.Game.Database;
using Server.Game.Database.Models;
using Server.Game.Entities;
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


            if (client.HeroEntity == null)
                return;

            PersistPlayer(client.HeroEntity);

            client.HeroEntity.Zone.RemoveEntity(client.HeroEntity);
        }

        private static void PersistPlayer(Player player)
        {
            using (var context = new GameDatabaseContext())
            {
                //TODO: Matching by a name might not be the best, but we'll run with it for now
                var hero = context.Characters.First(x => x.UserHeroId == player.UserId);

                // Load quest info
                context.Entry(hero).Collection(x => x.QuestInfo).Load();

                hero.Name = player.Name;
                hero.PositionX = (int)player.Position.X;
                hero.PositionY = (int)player.Position.Y;
                hero.ZoneId = player.Zone.Id;

                // Persist stats
                hero.Hitpoints = (int)player.CharacterStats[(int)StatTypes.Hitpoints].CurrentValue;
                hero.Strength = (int)player.CharacterStats[(int)StatTypes.Strength].CurrentValue;
                hero.Intelligence = (int)player.CharacterStats[(int)StatTypes.Intelligence].CurrentValue;
                hero.Dexterity = (int)player.CharacterStats[(int)StatTypes.Dexterity].CurrentValue;
                hero.Luck = (int)player.CharacterStats[(int)StatTypes.Luck].CurrentValue;
                hero.Vitality = (int)player.CharacterStats[(int)StatTypes.Vitality].CurrentValue;
                hero.MaximumHitpoints = (int)player.CharacterStats[(int)StatTypes.Hitpoints].MaximumValue;


                //TODO: Need better tracking code here, incrementing the row needlessly here

                hero.QuestInfo.ToList().ForEach(r => context.UserQuestInfo.Remove(r));

                //context.SaveChanges();

                foreach (var questInfo in player.QuestInfo)
                {
                    

                    var item = new UserQuestInfo()
                    {
                        QuestId = questInfo.QuestId,
                        State = questInfo.State,
                        UserQuestInfoId = questInfo.UserQuestInfoId,
                        UserHero = hero

                    };

                    hero.QuestInfo.Add(item);
                  
                }

                context.Entry(hero).State = EntityState.Modified;

                // Flush
                context.SaveChanges();

            }


        }


    }
}