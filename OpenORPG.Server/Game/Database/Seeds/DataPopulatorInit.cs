using System.Data.Entity;
using System.Linq;
using Server.Game.Combat;
using Server.Game.Database.Models;
using Server.Game.Database.Models.ContentTemplates;

namespace Server.Game.Database.Seeds
{
    /// <summary>
    ///     A custom intalizer that populates the game database with mock data useful for testing, clean, good known states.
    /// </summary>
    public class CustomInitializer :  DropCreateDatabaseIfModelChanges<GameDatabaseContext>
    {
        //DropCreateDatabaseAlways<GameDatabaseContext>

        protected override void Seed(GameDatabaseContext context)
        {
            CreateTestSkills(context);

            context.SaveChanges();

            // Add a new user
            for (int i = 0; i < 50; i++)
            {
                var account = new UserAccount("Vaughan" + i, @"Vaughan/", "someone@someone.com");

                var character = new UserHero();


                character.Name = "Vaughan" + i;
                character.Account = account;
                character.ZoneId = 1;
                character.PositionX = 6 * 32;
                character.PositionY = 4 * 32;

                character.Inventory.Add(new UserItem(1, 1));
                
                // Add a basic attack to this character
                character.Skills.Add(new UserSkill(1));

                character.Strength = 5;
                character.Hitpoints = 10;
                character.MaximumHitpoints = 10;

                context.Characters.Add(character);

                context.Accounts.Add(account);
            }

            CreateTestMonsters(context);


            context.SaveChanges();



            base.Seed(context);
        }

        private void CreateTestSkills(GameDatabaseContext context)
        {
            var attack = new SkillTemplate(SkillType.Damage, SkillTargetType.Enemy, SkillActivationType.Immediate, 0, 1, "Slay mighty foes!", 1, "Attack");
            context.SkillTemplates.Add(attack);

            var banish = new SkillTemplate(SkillType.Damage, SkillTargetType.Enemy, SkillActivationType.Immediate, 0,
                long.MaxValue, "Eradicates a foe.", 2, "Banish");
            context.SkillTemplates.Add(banish);
        }

        private void CreateTestMonsters(GameDatabaseContext context)
        {

            // Create a sample 'snake'

            var snake = new MonsterTemplate();
            snake.Strength = 3;
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
            goblinVariant.Name = "Begusk";
            goblinVariant.Sprite = "goblin";
            goblinVariant.Id = 2;
            goblinVariant.Level = 5;

            // Add our test monsters to the game
            context.MonsterTemplates.Add(snake);
            context.MonsterTemplates.Add(goblin);
            context.MonsterTemplates.Add(goblinVariant);

        }

    }
}