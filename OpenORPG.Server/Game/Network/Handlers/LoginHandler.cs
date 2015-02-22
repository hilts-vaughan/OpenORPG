using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using OpenORPG.Database.Enums;
using OpenORPG.Database.Models.Quests;
using OpenORPG.Database.Models.Quests.Rewards;
using Server.Game.Database;
using Server.Game.Database.Models;
using Server.Game.Entities;
using Server.Game.Items;
using Server.Game.Network.Authentication;
using Server.Game.Network.Authentication.Providers;
using Server.Game.Network.Packets;
using Server.Infrastructure.Logging;
using Server.Infrastructure.Network.Handlers;
using Server.Infrastructure.Quests;

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

                foreach (var x in hero.QuestInfo)
                {
                    context.Entry(x).Collection(a => a.RequirementProgress).Load();

                    var l = x.RequirementProgress.ToList();
                    foreach (var n in l)
                        context.Entry(n).State = EntityState.Deleted;

                }

                context.Entry(hero).Collection(x => x.Inventory).Load();
                context.Entry(hero).Collection(x => x.Equipment).Load();
                context.Entry(hero).Collection(x => x.Flags).Load();

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
                hero.Mind = (int)player.CharacterStats[StatTypes.Mind].CurrentValue;

                hero.SkillResource = (int)player.CharacterStats[StatTypes.SkillResource].CurrentValue;
                hero.MaximumSkillResource = (int)player.CharacterStats[StatTypes.SkillResource].MaximumValue;

                hero.Experience = player.Experience;
                hero.Level = player.Level;

                hero.Flags = player.Flags;

                //TODO: Need better tracking code here, incrementing the row needlessly here




                // For now though, we don't care...
                hero.QuestInfo.ToList().ForEach(r => context.UserQuestInfo.Remove(r));

                context.SaveChanges();
                //context.SaveChanges();

                foreach (var entry in player.QuestLog)
                {




                    var temp = new List<UserQuestRequirements>();

                    for (int index = 0; index < entry.Progress.Count; index++)
                    {
                        var x = entry.Progress[index];

                        var questRequirementProgress = new UserQuestRequirements()
                        {
                            Progress = x
                        };

                        if (entry.QuestInfo.RequirementProgress.Count - 1 >= index)
                            questRequirementProgress.UserQuestRequirementId =
                                entry.QuestInfo.RequirementProgress[index].QuestInfo.UserQuestInfoId;

                        temp.Add(questRequirementProgress);
                    }


                    var quest = new UserQuestInfo()
                   {
                       QuestId = entry.QuestInfo.QuestId,
                       State = entry.State,
                       UserQuestInfoId = entry.QuestInfo.UserQuestInfoId,
                       UserHero = hero,
                       RequirementProgress = temp,
                       QuestProgress = entry.QuestInfo.QuestProgress
                   };



                    hero.QuestInfo.Add(quest);

                }


                hero.Inventory.ToList().ForEach(r => context.UserItems.Remove(r));

                foreach (var inventorySlot in player.Backpack.Storage)
                {
                    var x = new UserItem(inventorySlot.Value.Item.Id, inventorySlot.Value.Amount, inventorySlot.Key);
                    hero.Inventory.Add(x);
                }

                hero.Equipment.ToList().ForEach(r => context.UserEquipments.Remove(r));

                foreach (var equipmentPiece in player.Equipment)
                {
                    if (equipmentPiece == null)
                        continue;

                    var x = new UserEquipment()
                    {
                        ItemId = equipmentPiece.Id,
                        Slot = equipmentPiece.Slot,
                        UserEquipmentId = 0
                    };

                    hero.Equipment.Add(x);
                }


                context.Entry(hero).State = EntityState.Modified;

                // Flush
                context.SaveChanges();

            }


        }


    }
}