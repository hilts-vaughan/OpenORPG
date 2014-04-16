using System.Data.Entity;
using Server.Game.Database.Maps;
using Server.Game.Database.Models;
using Server.Game.Database.Models.ContentTemplates;
using Server.Game.Database.Seeds;

namespace Server.Game.Database
{
    /// <summary>
    ///     The game database context contains utility for interfacting with the game database.
    ///     A context should be opened up for each unit of work that is requested.
    /// </summary>
    public class GameDatabaseContext : DbContext
    {
        static GameDatabaseContext()
        {
            System.Data.Entity.Database.SetInitializer(new CustomInitializer());


            //System.Data.Entity.Database.SetInitializer(new CreateDatabaseIfNotExists<GameDatabaseContext>());
            //System.Data.Entity.Database.SetInitializer(new DropCreateDatabaseIfModelChanges<GameDatabaseContext>());
        }

        public GameDatabaseContext()
        {
            Configuration.LazyLoadingEnabled = false;
            Configuration.ProxyCreationEnabled = false;
        }

        public DbSet<UserAccount> Accounts { get; set; }
        public DbSet<UserHero> Characters { get; set; }
        public DbSet<MonsterTemplate> MonsterTemplates { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new UserAccounConfiguration());
            modelBuilder.Configurations.Add(new UserHeroMap());
            modelBuilder.Configurations.Add(new MonsterTemplateMap());
        }
    }
}