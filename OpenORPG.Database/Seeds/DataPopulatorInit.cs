﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using Inspire.Shared.Models.Enums;
using OpenORPG.Database.Enums;
using OpenORPG.Database.Models.ContentTemplates;
using OpenORPG.Database.Models.Quests;
using OpenORPG.Database.Models.Quests.Rewards;
using Server.Game.Database.Models;
using Server.Game.Database.Models.ContentTemplates;
using Server.Game.Database.Models.Quests;


namespace Server.Game.Database.Seeds
{
    /// <summary>
    ///     A custom initializer that populates the game database with mock data useful for testing, clean, good known states.
    /// </summary>
    public class CustomInitializer : DropCreateDatabaseAlways<GameDatabaseContext> // DropCreateDatabaseIfModelChanges<GameDatabaseContext>   
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
                character.Inventory.Add(new UserItem(4, 1, 3));

                // Add a basic attack to this character
                character.Skills.Add(new UserSkill(1));
                character.Skills.Add(new UserSkill(2));
                character.Skills.Add(new UserSkill(3));
                character.Skills.Add(new UserSkill(4));

                character.Strength = 5;
                character.Hitpoints = 100;
                character.MaximumHitpoints = 100;

                character.SkillResource = 100;
                character.MaximumSkillResource = 100;

                character.Level = 1;
                character.Experience = 0;

                context.Characters.Add(character);

                context.Accounts.Add(account);
            }

            // Create test dialog
            DialogTemplate dialogTemplate = new DialogTemplate();
            dialogTemplate.Name = "Introduction to World";
            dialogTemplate.JsonPayload =
                "{\"$id\":\"1\",\"Name\":\"Root\",\"Text\":\"Welcome to Port Windurst! If I can help you out with anything, please ask.\",\"Links\":[{\"$id\":\"2\",\"NextNode\":{\"$id\":\"3\",\"Name\":\"Reply\",\"Text\":\"I\'ve restored your vitals completely.\",\"Links\":[]},\"Script\":null,\"DialogConditions\":[{\"$id\":\"4\",\"$type\":\"OpenORPG.Common.Dialog.Conditions.MinimumLevelDialogCondition, OpenORPG.Common\",\"Level\":9}],\"Name\":\"Healing\",\"Text\":\"I need healing.\"},{\"$id\":\"5\",\"NextNode\":{\"$id\":\"6\",\"Name\":\"Reply\",\"Text\":\"Actually, maybe... What would you like to know?\",\"Links\":[{\"$id\":\"7\",\"NextNode\":null,\"Script\":null,\"DialogConditions\":[],\"Name\":\"Ask\",\"Text\":\"Ask about Lancaster.\"},{\"$id\":\"8\",\"NextNode\":{\"$id\":\"9\",\"Name\":\"Reply\",\"Text\":\"Oh, the minister? Yeah, I heard some odd stuff about him... they say he\'s been keeping some secrets, that one.\",\"Links\":[]},\"Script\":null,\"DialogConditions\":[],\"Name\":\"Ask\",\"Text\":\"Ask about Minister Winrix\"}]},\"Script\":null,\"DialogConditions\":[],\"Name\":\"Rumours:\",\"Text\":\"Have you heard any rumours?\"},{\"$id\":\"10\",\"NextNode\":null,\"Script\":null,\"DialogConditions\":[],\"Name\":\"Leave\",\"Text\":\"Thanks for stopping by, have a good day!\"}]}";
            dialogTemplate.VirtualCategory = "Beginner";

            context.DialogTemplates.Add(dialogTemplate);

            context.SaveChanges();

            CreateTestMonsters(context);
            CreateTestQuests(context);
            CreateTestItems(context);

 
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
            testItem.Str = 999;

            var testPlate = new ItemTemplate(0, "Astral Body",
                "Providing almost perfect defense, this body piece is said to be almost unbreakable by any foe. Designed for the 'Liberators', this " +
                "piece was designed for war with gods.", ItemType.Equipment, 0, false, 0);
            testPlate.EquipmentSlot = EquipmentSlot.Body;
            testPlate.Vit = 999;

            context.ItemTemplates.Add(testItem);
            context.ItemTemplates.Add(testPlate);




            // Create a bunch of test equipment for the sample game
            var bronzeSword = new ItemTemplate(0, "Bronze Sword",
                "A basic bronze sword with just enough strength to bang up some monsters.", ItemType.Equipment, 0, false,
                300);

            bronzeSword.EquipmentSlot = EquipmentSlot.Weapon;
            bronzeSword.Damage = 6;
            bronzeSword.IconId = 43;            

            context.ItemTemplates.Add(bronzeSword);


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
            var attack = new SkillTemplate(SkillType.Physical, SkillTargetType.Enemy, SkillActivationType.Immediate, 0, 1, "Slay mighty foes!", 1, "Attack");
            context.SkillTemplates.Add(attack);

            var banish = new SkillTemplate(SkillType.Elemental, SkillTargetType.Enemy, SkillActivationType.Target, 0,
                99999, "Eradicates a foe.", 2, "Banish");
            banish.IconId = 112;
            banish.CooldownTime = 10;
            context.SkillTemplates.Add(banish);

            var fire = new SkillTemplate(SkillType.Elemental, SkillTargetType.Enemy, SkillActivationType.Target, 3, 5,
                "Deals a small amount of fire damage to a foe.", 3, "Fire");
            fire.CooldownTime = 5;
            fire.IconId = 45;
            fire.Script = "Skill_Deathstrike";
            context.SkillTemplates.Add(fire);

            var heal = new SkillTemplate(SkillType.Healing, SkillTargetType.Self | SkillTargetType.Ally, SkillActivationType.Target, 1, 10,
                "Restores a small amount of HP to a character.", 4, "Cure");
            heal.CooldownTime = 9;
            heal.IconId = 99;
            context.SkillTemplates.Add(heal);

        }

        private void CreateTestQuests(GameDatabaseContext context)
        {
            var items = context.ItemTemplates.Where(x => x.Id == 1).ToList();
            var monsterReq = new QuestMonsterRequirementTable()
            {
                MonsterId = 2,
                MonsterAmount = 2
            };

            var quest = new QuestTemplate()
            {
                Name = "Overthrow the Goblins",
                Description = "We've got a goblin problem going around. Think you can help us out? I reckon knocking out at least 5 of them should give them a spook.",
                Id = 1,
                QuestSteps = new List<QuestStepsTable>(),
                Rewards = new List<QuestReward>()
            };

            var step = new QuestStepsTable();
            step.Requirements.Add(monsterReq);

            quest.QuestSteps.Add(step);

            // Setup a reward
            var reward = new QuestRewardExperience() {Amount = 5000};
            quest.Rewards.Add(reward);


            context.Quests.Add(quest);

          //  context.SaveChanges();


            var npc = new NpcTemplate();
            npc.Name = "Forest Explorer";
            npc.Sprite = "forestnpc";
            npc.Quests = new List<QuestTemplate>();
            npc.Quests.Add(quest);

            context.Npcs.Add(npc);

            // Generate someone for conversations where possible
            var guideNpc = new NpcTemplate();
            guideNpc.Name = "Kasaroro";
            guideNpc.Sprite = "villagegirl";
            guideNpc.ConversationAvailableTemplate = context.DialogTemplates.First(x => x.Id == 1);

            var guard_generic = new NpcTemplate();
            guard_generic.Name = "City Guard";
            guard_generic.Sprite = "forestnpc";
            guard_generic.ConversationAvailableTemplate = context.DialogTemplates.First(x => x.Id == 1);


            context.Npcs.Add(guard_generic);
            context.Npcs.Add(guideNpc);


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