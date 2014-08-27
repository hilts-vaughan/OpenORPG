using System;
using System.CodeDom;
using System.Linq;
using Server.Game.AI;
using Server.Game.Combat;
using Server.Game.Database;
using Server.Game.Database.Models;
using Server.Game.Database.Models.ContentTemplates;
using Server.Game.Entities;
using Server.Game.Network.Handlers;
using Server.Game.Network.Packets;
using Server.Game.Zones;
using Server.Infrastructure.World;
using Server.Utils.Math;

namespace Server.Game
{
    /// <summary>
    ///     This factory provides methods for quickly creating objects that are frequently used throughout the server
    ///     This allows quick construction of game objects that are used frequently.
    /// </summary>
    public static class GameObjectFactory
    {


        public static Player CreateHero(UserHero userHero, GameClient gameClient)
        {
            var entity = new Player("male_base", gameClient, userHero);
            entity.Position = new Vector2(userHero.PositionX, userHero.PositionY);
            entity.Name = userHero.Name;

            // Copy the stats from the DB into the player
            CopyStatsFromTemplateToCharacter(userHero, entity);

            return entity;
        }

        /// <summary>
        /// Creates a monster given a reference ID.
        /// </summary>
        /// <param name="id">The ID of the monster to fetch the template for</param>
        /// <returns></returns>
        public static Monster CreateMonster(long id)
        {
            using (var context = new GameDatabaseContext())
            {
                var template = context.MonsterTemplates.FirstOrDefault(x => x.Id == id);

                // Setup actual object
                var monster = new Monster(template);

                // Copy stats into
                CopyStatsFromTemplateToCharacter(template, monster);

                monster.CurrentAi = new WanderAi(monster);
                monster.Skills.Add(new Skill(context.SkillTemplates.First(x => x.Id == 1)));

                return monster;
            }
        }

        public static Npc CreateNpc(long id)
        {
            using (var context = new GameDatabaseContext())
            {
                var template = context.Npcs.FirstOrDefault(x => x.NpcId == id);

                if (template == null)
                    throw new Exception("Creating an NPC with the given Id is invalid.");          

                // Load up the quests this NPC will have
                context.Entry(template).Collection(x => x.Quests).Load();

                foreach (var quest in template.Quests)
                {

                    context.Entry(quest).Collection(x => x.RewardItems).Load();
                    context.Entry(quest).Collection(x => x.QuestSteps).Load();

                    // Load up our set of requirements from the table
                    foreach (var x in quest.QuestSteps)
                        context.Entry(x).Collection(a => a.Requirements).Load();

                }


                var npc = new Npc(template);

                return npc;

            }

        }

        /// <summary>
        /// Copies the template stats given to the <see cref="Character"/> being created from these.
        /// </summary>
        /// <param name="template">The template to copy from</param>
        /// <param name="character">The character being constructed</param>
        private static void CopyStatsFromTemplateToCharacter(IStatTemplate template, Character character)
        {
            character.CharacterStats[(int)StatTypes.Strength].CurrentValue = template.Strength;
            character.CharacterStats[(int)StatTypes.Vitality].CurrentValue = template.Vitality;
            character.CharacterStats[(int)StatTypes.Intelligence].CurrentValue = template.Intelligence;
            character.CharacterStats[(int)StatTypes.Dexterity].CurrentValue = template.Dexterity;
            character.CharacterStats[(int)StatTypes.Luck].CurrentValue = template.Luck;
            character.CharacterStats[StatTypes.Mind].CurrentValue = template.Mind;

            character.CharacterStats[(int)StatTypes.Hitpoints].CurrentValue = template.Hitpoints;
            character.CharacterStats[(int)StatTypes.Hitpoints].MaximumValue = template.MaximumHitpoints;

            character.CharacterStats[(int)StatTypes.SkillResource].CurrentValue = template.SkillResource;
            character.CharacterStats[(int)StatTypes.SkillResource].MaximumValue = template.MaximumSkillResource;         



        }


    }
}