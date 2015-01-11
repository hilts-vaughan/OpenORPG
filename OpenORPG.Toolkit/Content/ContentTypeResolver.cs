using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenORPG.Database.DAL;
using Server.Game.Database;
using Server.Game.Database.Models;
using Server.Game.Database.Models.ContentTemplates;
using Server.Game.Database.Models.Quests;

namespace OpenORPG.Toolkit.Content
{
    /// <summary>
    /// A static utility to resolve template data
    /// </summary>
    public static class ContentTypeResolver
    {
        public static void ForceUpdate<T>(Type t, T template) where T : class, IContentTemplate
        {

            using (var context = new GameDatabaseContext())
            {
                var monsterRepository = new MonsterRepository(context);
                var itemRepo = new ItemRepository(context);
                var skillRepo = new SkillRepository(context);
                var questRepo = new QuestRepository(context);
                var npcRepo = new NpcRepository(context);

                var @switch = new Dictionary<Type, Action>
            {                
                  { typeof (MonsterTemplate), () => monsterRepository.Update(template as MonsterTemplate, template.Id) },
                  { typeof (ItemTemplate), () => itemRepo.Update(template as ItemTemplate, template.Id)},
                  { typeof (SkillTemplate), () => skillRepo.Update(template as SkillTemplate, template.Id) },     
                  { typeof (QuestTemplate), () => questRepo.Update(template as QuestTemplate, template.Id) },
                  { typeof (NpcTemplate), () => npcRepo.Update(template as NpcTemplate, template.Id) }   
            };

                @switch[t]();


            }



        }

        public static void AddContentWithVirtualCategory(Type type, string category)
        {
            category = category.Replace("\\", "/");

            //TODO: This modules logic sucks

            using (var db = new GameDatabaseContext())
            {
                var monsterRepository = new MonsterRepository(db);
                var itemRepo = new ItemRepository(db);
                var skillRepo = new SkillRepository(db);
                var questRepo = new QuestRepository(db);
                var npcRepo = new NpcRepository(db);


                string NewName = "New Content";
                var @switch = new Dictionary<Type, Action>
            {                
                  { typeof (MonsterTemplate), () => monsterRepository.Add(new MonsterTemplate() { VirtualCategory = category, Name = NewName } ) },
                  { typeof (ItemTemplate), () => itemRepo.Add(new ItemTemplate() { VirtualCategory = category, Name = NewName }  ) },
                  { typeof (SkillTemplate), () => skillRepo.Add(new SkillTemplate()  { VirtualCategory = category, Name = NewName } ) },     
                  { typeof (QuestTemplate), () => questRepo.Add(new QuestTemplate()  { VirtualCategory = category, Name = NewName } ) },
                  { typeof (NpcTemplate), () => npcRepo.Add(new NpcTemplate()  { VirtualCategory = category, Name = NewName } ) }   
            };

                @switch[type]();

            }


        }

        public static List<IContentTemplate> GetContentTemplateFromType(Type type)
        {
            List<IContentTemplate> result = null;

            using (var db = new GameDatabaseContext())
            {
                var monsterRepository = new MonsterRepository(db);
                var itemRepo = new ItemRepository(db);
                var skillRepo = new SkillRepository(db);
                var questRepo = new QuestRepository(db);
                var npcRepo = new NpcRepository(db);


                var @switch = new Dictionary<Type, Action>
            {                
                  { typeof (MonsterTemplate), () => result = new List<IContentTemplate>(monsterRepository.GetAll().ToList()) },
                  { typeof (ItemTemplate), () => result = new List<IContentTemplate>(itemRepo.GetAll().ToList()) },
                  { typeof (SkillTemplate), () => result = new List<IContentTemplate>(skillRepo.GetAll().ToList()) },     
                  { typeof (QuestTemplate), () => result = new List<IContentTemplate>(questRepo.GetAll().ToList()) },
                  { typeof (NpcTemplate), () => result = new List<IContentTemplate>(npcRepo.GetAll().ToList()) }   
            };

                @switch[type]();

            }

            return result;
        }




    }
}
