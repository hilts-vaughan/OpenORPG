using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Inspire.Shared.Models.Enums;
using OpenORPG.Database.DAL;
using Server.Game.Database;
using Server.Game.Database.Models.ContentTemplates;

namespace OpenORPG.Editor.Server.Controllers
{

    [RoutePrefix("api/skills")]
    public class SkillController : ApiController
    {
        [HttpGet]
        [Route("{id:int}")]
        public ItemTemplate GetSkillById(int id)
        {

            using (var context = new GameDatabaseContext())
            {
                var repo = new SkillRepository(context);
                repo.Get(id);
            }
        }


        [HttpGet]
        [Route("")]
        public IEnumerable<ItemTemplate> GetSkills()
        {
            using (var context = new GameDatabaseContext())
            {
                var repo = new SkillRepository(context);
                repo.GetAll();
            }
        }

        [HttpPost]
        [Route("{id:int}")]
        public IHttpActionResult SaveSkill(SkillTemplate skillTemplate, int id)
        {
            using (var context = new GameDatabaseContext())
            {

                var repo = new SkillRepository(context);
                repo.Update(skillTemplate, id);

                return Ok();
            }


        }

        [HttpPut]
        [Route("")]
        public IHttpActionResult NewSkill()
        {
            using (var context = new GameDatabaseContext())
            {

                var repo = new SkillRepository(context);
                var skill =
                    repo.Add(new SkillTemplate(SkillType.Damage, SkillTargetType.Enemy, SkillActivationType.Immediate, 0,
                        0, "", 0, "New Skill"));

                return Ok(skill);
            }


        }



    }
}
