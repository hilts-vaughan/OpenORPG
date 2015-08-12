using System;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Core.Common;
using System.Data.Entity.Migrations.History;
using System.Reflection;
using OpenORPG.Database.Models.ContentTemplates;
using OpenORPG.Database.Models.Quests;
using Server.Game.Database.Maps;
using Server.Game.Database.Models;
using Server.Game.Database.Models.ContentTemplates;
using Server.Game.Database.Models.Quests;
using Server.Game.Database.Seeds;

namespace Server.Game.Database
{
    public class MySqlConfiguration : DbConfiguration
    {
        public MySqlConfiguration()
        {
            SetHistoryContext(
            "MySql.Data.MySqlClient", (conn, schema) => new MySqlHistoryContext(conn, schema));
        }
    }
    /// <summary>
    ///     The game database context contains utility for interfacting with the game database.
    ///     A context should be opened up for each unit of work that is requested.
    /// </summary>

    public class MySqlHistoryContext : HistoryContext
    {
        public MySqlHistoryContext(
          DbConnection existingConnection,
          string defaultSchema)
            : base(existingConnection, defaultSchema)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<HistoryRow>().Property(h => h.MigrationId).HasMaxLength(100).IsRequired();
            modelBuilder.Entity<HistoryRow>().Property(h => h.ContextKey).HasMaxLength(200).IsRequired();
        }
    }


    public class GameDatabaseContext : DbContext
    {
        static GameDatabaseContext()
        {
            System.Data.Entity.Database.SetInitializer(new CustomInitializer());


            var x = typeof(System.Data.Entity.SqlServer.SqlProviderServices);

            //System.Data.Entity.Database.SetInitializer(new CreateDatabaseIfNotExists<GameDatabaseContext>());
            //System.Data.Entity.Database.SetInitializer(new DropCreateDatabaseIfModelChanges<GameDatabaseContext>());
        }

        public GameDatabaseContext()
            : base("MyContext")
        {
            Configuration.LazyLoadingEnabled = false;
            Configuration.ProxyCreationEnabled = false;
        }

        public DbSet<UserAccount> Accounts { get; set; }
        public DbSet<UserHero> Characters { get; set; }

        public DbSet<MonsterTemplate> MonsterTemplates { get; set; }
        public DbSet<SkillTemplate> SkillTemplates { get; set; }
        public DbSet<ItemTemplate> ItemTemplates { get; set; }

        public DbSet<DialogTemplate> DialogTemplates { get; set; }


        // quest related stuff

        public DbSet<QuestTemplate> Quests { get; set; }
                


        public DbSet<NpcTemplate> Npcs { get; set; }

        public DbSet<UserQuestInfo> UserQuestInfo { get; set; }
        public DbSet<UserItem> UserItems { get; set; }
        public DbSet<UserEquipment> UserEquipments { get; set; }


        

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
   

            modelBuilder.Configurations.Add(new SkillTemplateMap());

            modelBuilder.Configurations.Add(new UserAccounConfiguration());
            modelBuilder.Configurations.Add(new UserHeroMap());
            modelBuilder.Configurations.Add(new MonsterTemplateMap());
            modelBuilder.Configurations.Add(new UserStorageMap());
            modelBuilder.Configurations.Add(new UserSkillMap());
            modelBuilder.Configurations.Add(new UserEquipmentMap());

;            modelBuilder.Configurations.Add(new ItemTemplateMap());

            modelBuilder.Entity<QuestTemplate>().HasKey(x => x.Id);


  




        }
    }
}