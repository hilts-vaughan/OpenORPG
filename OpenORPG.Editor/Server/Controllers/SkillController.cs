﻿using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Inspire.Shared.Models.Enums;
using OpenORPG.Database.DAL;
using OpenORPG.Database.Enums;
using Server.Game.Database;
using Server.Game.Database.Models.ContentTemplates;

namespace OpenORPG.Editor.Server.Controllers
{

    [RoutePrefix("api/skills")]
    public class SkillController : ApiController
    {
        [HttpGet]
        [Route("{id:int}")]
        public SkillTemplate GetSkillById(int id)
        {

            using (var context = new GameDatabaseContext())
            {
                var repo = new SkillRepository(context);
                return repo.Get(id);
            }
        }


        [HttpGet]
        [Route("")]
        public IEnumerable<SkillTemplate> GetSkills()
        {
            using (var context = new GameDatabaseContext())
            {
                var repo = new SkillRepository(context);
                return repo.GetAll();
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
                    repo.Add(new SkillTemplate(SkillType.Elemental, SkillTargetType.Enemy, SkillActivationType.Immediate, 0,
                        0, "", 0, "New Skill"));

                return Ok(skill);
            }


        }



    }
}
