using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Server.Game.Database;
using Server.Game.Database.Models;
using Server.Game.Network.Packets;
using Server.Game.Zones;
using Server.Infrastructure.Logging;
using Server.Infrastructure.Network.Handlers;
using Server.Infrastructure.World;

namespace Server.Game.Network.Handlers
{
    /// <summary>
    /// Handles hero selection and creation
    /// </summary>
    public class HeroSelectionHandler
    {
        [PacketHandler(OpCodes.CMSG_HERO_SELECT)]
        public static void OnCharacterSelect(GameClient client, ClientHeroSelectPacket packet)
        {
            // Ensure this is a valid action
            bool actionCanBePerformed = client.Account.IsOnline && client.HeroEntity == null;

            if (!actionCanBePerformed)
                return;



            // Get the hero the user wanted
            UserHero hero = client.Account.Heroes.FirstOrDefault();


            if (hero == null)
            {
                var response = new ServerHeroSelectResponsePacket(HeroStatus.Invalid);
                client.Send(response);
            }
            else
            {

                using (var context = new GameDatabaseContext())
                {
                    context.Characters.Attach(hero);
                    context.Entry(hero).Collection(a => a.Skills).Load();
                    context.Entry(hero).Collection(a => a.Inventory).Load();
                }


                // Create an object and assign it the world if everything is okay
                var heroObject = GameObjectFactory.CreateHero(hero, client);
                client.HeroEntity = heroObject;

                var zone = ZoneManager.Instance.FindZone(hero.ZoneId);

                if (zone != null)
                {
                    // Create a response and alert the client that their selection is okay
                    var response = new ServerHeroSelectResponsePacket(HeroStatus.OK);
                    client.Send(response);

                    Thread.Sleep(500);

                    Logger.Instance.Info("{0} has entered the game.", hero.Name);
                    zone.AddEntity(heroObject);
                }
                else
                {
                    var response = new ServerHeroSelectResponsePacket(HeroStatus.Unavailable);
                    client.Send(response);

                    Logger.Instance.Error("{0} attempted to enter the game but could not be loaded.", hero.Name);
                }


            }
        }

        [PacketHandler(OpCodes.CMSG_HERO_CREATE)]
        public static void OnCharacterCreate(GameClient client, ClientHeroCreatePacket packet)
        {
            Logger.Instance.Info("Attempting to create hero with name {0}", packet.Name);
            bool heroExists;

            using (var context = new GameDatabaseContext())
            {
                // We try to load a hero with a similar name to see if they exist
                heroExists = context.Characters.Any(x => x.Name.ToUpper() == packet.Name.ToUpper());
            }

            if (heroExists)
            {
                client.Send(new ServerHeroCreateResponsePacket(HeroStatus.Invalid));
                return;
            }

            // Create a hero and attach it if it dosen't exist yet

            var hero = new UserHero(client.Account, 0, 0, 0, packet.Name);

            // Save our hero into the database
            using (var context = new GameDatabaseContext())
            {
                context.Accounts.Attach(client.Account);
                context.Entry(client.Account).Collection(a => a.Heroes).Load();
                client.Account.Heroes.Add(hero);
                context.SaveChanges();
            }

            client.Send(new ServerHeroCreateResponsePacket(HeroStatus.OK));

            SendHeroList(client);
        }

        public static void SendHeroList(GameClient client)
        {
            var list = new List<HeroInfo>();

            foreach (UserHero hero in client.Account.Heroes)
            {
                var info = new HeroInfo();
                info.HeroId = hero.Id;
                info.Name = hero.Name;
                list.Add(info);
            }

            client.Send(new ServerHeroListPacket(list));
        }
    }
}