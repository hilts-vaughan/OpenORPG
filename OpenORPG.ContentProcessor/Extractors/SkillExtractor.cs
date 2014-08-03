using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using OpenORPG.ContentProcessor.Persistence;
using OpenORPG.Database.DAL;
using Server.Game.Database;
using Server.Game.Database.Models.ContentTemplates;
using ServiceStack;

namespace OpenORPG.ContentProcessor.Extractors
{
    public class SkillExtractor : IContentExtractor
    {

        public void ProcessContent(IContentPersister persister)
        {


            using (var db = new GameDatabaseContext())
            {
                var repo = new SkillRepository(db);
                var skills = repo.GetAll();

                foreach (var skill in skills)
                {

                    // Save out properties we want to a new object and then persist
                    dynamic persistable = new ExpandoObject();

                    Console.WriteLine("Processing skill with ID {0}", skill.Id);

                    persistable.id = skill.Id;
                    persistable.name = skill.Name;
                    persistable.castTime = skill.CastTime;
                    persistable.cooldownTime = skill.CooldownTime;
                    persistable.description = skill.Description;
                    persistable.damage = skill.Damage;
                    persistable.type = skill.SkillType;

                    persister.Persist(persistable, "\\skills\\{0}.json".FormatWith(skill.Id));

                }

            }

        }


    }
}
