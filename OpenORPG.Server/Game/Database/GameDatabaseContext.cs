using System.Data.Entity;
using Server.Game.Database.Maps;
using Server.Game.Database.Models;
using Server.Game.Database.Models.ContentTemplates;
using Server.Game.Database.Models.Quests;
using Server.Game.Database.Seeds;
using Server.Infrastructure.Quests;

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
        public DbSet<SkillTemplate> SkillTemplates { get; set; }
        public DbSet<ItemTemplate> ItemTemplates { get; set; }
        public DbSet<QuestTable> Quests { get; set; }

        public DbSet<NpcTemplate> Npcs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new SkillTemplateMap());

            modelBuilder.Configurations.Add(new UserAccounConfiguration());
            modelBuilder.Configurations.Add(new UserHeroMap());
            modelBuilder.Configurations.Add(new MonsterTemplateMap());
            modelBuilder.Configurations.Add(new UserStorageMap());
            modelBuilder.Configurations.Add(new UserSkillMap());
            modelBuilder.Configurations.Add(new ItemTemplateMap());

            modelBuilder.Entity<QuestTable>().HasKey(x => x.QuestTableId);


            modelBuilder.Entity<QuestItemRequirementTable>().HasKey(x => x.QuestTableId).HasRequired(x => x.Quest).WithOptional(x => x.EndItemRequirements).WillCascadeOnDelete(true);
            modelBuilder.Entity<QuestMonsterRequirementTable>().HasKey(x => x.QuestTableId).HasRequired(x => x.Quest).WithOptional(x => x.EndMonsterRequirements).WillCascadeOnDelete(true);


        }
    }
}