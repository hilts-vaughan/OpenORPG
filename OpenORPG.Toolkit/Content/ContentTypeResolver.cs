using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenORPG.Database.DAL;
using Server.Game.Database;
using Server.Game.Database.Models.ContentTemplates;

namespace OpenORPG.Toolkit.Content
{
    /// <summary>
    /// A static utility to resolve template data
    /// </summary>
    public static class ContentTypeResolver
    {

        public static List<IContentTemplate> GetContentTemplateFromType(Type type)
        {
            List<IContentTemplate> result = null;

            using (var db = new GameDatabaseContext())
            {
                var monsterRepository = new MonsterRepository(db);
                var itemRepo = new ItemRepository(db);

                var @switch = new Dictionary<Type, Action>
            {                
                  { typeof (MonsterTemplate), () => result = new List<IContentTemplate>(monsterRepository.GetAll().ToList()) },
                  { typeof (ItemTemplate), () => result = new List<IContentTemplate>(itemRepo.GetAll().ToList()) }     
            };

                @switch[type]();

            }

            return result;
        }




    }
}
