using System.Data.Entity;
using Server.Game.Database.Models;

namespace Server.Game.Database.Seeds
{
    /// <summary>
    ///     A custom intalizer that populates the game database with mock data useful for testing, clean, good known states.
    /// </summary>
    public class CustomInitializer : DropCreateDatabaseAlways<GameDatabaseContext>
        // DropCreateDatabaseIfModelChanges<GameDatabaseContext>
    {
        protected override void Seed(GameDatabaseContext context)
        {
            // Add a new user
            {
                var account = new UserAccount("Vaughan", "Vaughan", "someone@someone.com");

                var character = new UserHero();
                character.Name = "Vaughan";
                character.Account = account;
                character.ZoneId = 1;
                character.PositionX = 77 * 32;
                character.PositionY = 207 * 32;

                context.Characters.Add(character);

                context.Accounts.Add(account);
            }

            {
                var account = new UserAccount("test", "test", "someone@someone.com");

                {
                    var character = new UserHero();
                    character.Account = account;

                    character.Name = "Test";
                    character.ZoneId = 0;
                    character.PositionX = 64;
                    character.PositionY = 64;
                    character.Strength = 10;
                    character.Dexterity = 10;
                    character.Intelligence = 10;
                    character.Vitality = 10;
                    character.Gold = 100;
                    character.Hitpoints = 9; //TODO: what to set here?
                    character.Experience = 0;
                    character.Level = 1;

                    context.Characters.Add(character);
                }

                context.Accounts.Add(account);
            }

            context.SaveChanges();


            base.Seed(context);
        }
    }
}