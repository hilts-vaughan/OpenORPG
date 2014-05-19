using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using Inspire.Shared.Models.Enums;
using Server.Game.Combat;
using Server.Game.Database.Models;
using Server.Game.Database.Models.ContentTemplates;
using Server.Game.Database.Models.Quests;
using Server.Game.Items.Equipment;
using Server.Infrastructure.Quests;

namespace Server.Game.Database.Seeds
{
    /// <summary>
    ///     A custom intalizer that populates the game database with mock data useful for testing, clean, good known states.
    /// </summary>
    public class CustomInitializer : DropCreateDatabaseAlways<GameDatabaseContext>   //DropCreateDatabaseIfModelChanges<GameDatabaseContext>  // 
    {



        protected override void Seed(GameDatabaseContext context)
        {
            CreateTestSkills(context);

            context.SaveChanges();

            // Add a new user
            for (int i = 0; i < 50; i++)
            {
                var account = new UserAccount("Vaughan" + i, @"Vaughan/", "someone@someone.com");

                var character = new UserHero();


                character.Name = GetRandomName();
                character.Account = account;

                character.ZoneId = 1;
                character.PositionX = 6 * 32;
                character.PositionY = 4 * 32;

                character.HomepointZoneId = character.ZoneId;
                character.HomepointZoneX = character.PositionX;
                character.HomepointZoneY = character.PositionY;

                character.Inventory.Add(new UserItem(1, 1, 0));
                character.Inventory.Add(new UserItem(2, 1, 1));
                character.Inventory.Add(new UserItem(3, 1, 2));

                // Add a basic attack to this character
                character.Skills.Add(new UserSkill(1));

                character.Strength = 5;
                character.Hitpoints = 100;
                character.MaximumHitpoints = 100;
                character.Level = 1;
                character.Experience = 0;

                context.Characters.Add(character);

                context.Accounts.Add(account);
            }

            CreateTestMonsters(context);
            CreateTestQuests(context);
            CreateTestItems(context);

            context.SaveChanges();
            context.SaveChanges();



            base.Seed(context);
        }

        private void CreateTestItems(GameDatabaseContext context)
        {
            var itemTemplate = new ItemTemplate(0, "A simple test item", "Something special indeed", ItemType.FieldItem,
                500, true, 0);
            context.ItemTemplates.Add(itemTemplate);

            // NOTICE: Creating test equipment here
            var testItem = new ItemTemplate(0, "Pendragon",
                "A weapon with an immense amount of magic radiating from the hilt. Designed as a weapon of war for the 'Liberators', this weapon leaves nothing behind when faced with a foe.",
                ItemType.Equipment, 0, false, 0);

            testItem.EquipmentSlot = EquipmentSlot.Weapon;
            testItem.StrengthModifier = 999;

            var testPlate = new ItemTemplate(0, "Astral Body",
                "Providing almost perfect defense, this body piece is said to be almost unbreakable by any foe. Designed for the 'Liberators', this " +
                "piece was designed for war with gods.", ItemType.Equipment, 0, false, 0);
            testPlate.EquipmentSlot = EquipmentSlot.Body;
            testPlate.VitalityModifier = 999;

            context.ItemTemplates.Add(testItem);
            context.ItemTemplates.Add(testPlate);


        }

        private string GetRandomName()
        {
            var names = new List<string>()
            {
                "Touma",
                "Haruki",
                "Setsuna",
                "Nymph",
                "Zeta",
                "Holo",
                "Haruhi",
                "Blank",
                "Himeragi"
            };

            return names.OrderBy(s => Guid.NewGuid()).First();

        }

        private void CreateTestSkills(GameDatabaseContext context)
        {
            var attack = new SkillTemplate(SkillType.Damage, SkillTargetType.Enemy, SkillActivationType.Immediate, 0, 1, "Slay mighty foes!", 1, "Attack");
            context.SkillTemplates.Add(attack);

            var banish = new SkillTemplate(SkillType.Damage, SkillTargetType.Enemy, SkillActivationType.Immediate, 0,
                long.MaxValue, "Eradicates a foe.", 2, "Banish");
            context.SkillTemplates.Add(banish);
        }

        private void CreateTestQuests(GameDatabaseContext context)
        {
            var items = context.ItemTemplates.Where(x => x.Id == 1).ToList();
            var monsterReq = new QuestMonsterRequirementTable()
            {
                QuestTableId = 1,
                MonsterId = 2,
                MonsterAmount = 5
            };

            var quest = new QuestTable()
            {
                Name = "Overthrow the Goblins",
                Description = "We've got a goblin problem going around. Think you can help us out? I reckon knocking out at least 5 of them should give them a spook.",
                RewardCurrency = 0,
                RewardExp = 100,
                QuestTableId = 1,
                EndMonsterRequirements = monsterReq
            };

            context.Quests.Add(quest);

            context.SaveChanges();


            var npc = new NpcTemplate();
            npc.Name = "Forest Explorer";
            npc.Sprite = "forestnpc";
            npc.Quests = new List<QuestTable>();
            npc.Quests.Add(quest);

            context.Npcs.Add(npc);

        }

        private void CreateTestMonsters(GameDatabaseContext context)
        {

            // Create a sample 'snake'

            var snake = new MonsterTemplate();
            snake.Strength = 1;
            snake.Hitpoints = 10;
            snake.Vitality = 1;
            snake.Name = "Snake";
            snake.Sprite = "snake";
            snake.Id = 0;
            snake.Level = 1;

            var goblin = new MonsterTemplate();
            goblin.Strength = 5;
            goblin.Hitpoints = 25;
            goblin.Vitality = 5;
            goblin.Name = "Goblin";
            goblin.Sprite = "goblin";
            goblin.Id = 1;
            goblin.Level = 3;

            var goblinVariant = new MonsterTemplate();
            goblinVariant.Strength = 7;
            goblinVariant.Hitpoints = 75;
            goblinVariant.Vitality = 6;
            goblinVariant.Name = "Fire Elemental";
            goblinVariant.Sprite = "elemental_fire";
            goblinVariant.Id = 2;
            goblinVariant.Level = 5;

            // Add our test monsters to the game
            context.MonsterTemplates.Add(snake);
            context.MonsterTemplates.Add(goblin);
            context.MonsterTemplates.Add(goblinVariant);

        }

    }
}