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
            for(int i = 0; i < 50; i++)
            {
                var account = new UserAccount("Vaughan" + i, "Vaughan", "someone@someone.com");

                var character = new UserHero();
                character.Name = "Vaughan" + i;
                character.Account = account;
                character.ZoneId = 1;
                character.PositionX = 77 * 32;
                character.PositionY = (207 + i) * 32;

                context.Characters.Add(character);

                context.Accounts.Add(account);
            }

            context.SaveChanges();


            base.Seed(context);
        }
    }
}