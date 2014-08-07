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

    [RoutePrefix("api/monsterss")]
    public class MonsterController : ApiController
    {
        [HttpGet]
        [Route("{id:int}")]
        public MonsterTemplate GetMonsterById(int id)
        {

            using (var context = new GameDatabaseContext())
            {
                var repo = new MonsterRepository(context);
                return repo.Get(id);
            }
        }


        [HttpGet]
        [Route("")]
        public IEnumerable<MonsterTemplate> GetMonsters()
        {
            using (var context = new GameDatabaseContext())
            {
                var repo = new MonsterRepository(context);
                return repo.GetAll();
            }
        }

        [HttpPost]
        [Route("{id:int}")]
        public IHttpActionResult SaveMonster(MonsterTemplate monsterTemplate, int id)
        {
            using (var context = new GameDatabaseContext())
            {

                var repo = new MonsterRepository(context);
                repo.Update(monsterTemplate, id);

                return Ok();
            }


        }

        [HttpPut]
        [Route("")]
        public IHttpActionResult NewMonster()
        {
            using (var context = new GameDatabaseContext())
            {

                var repo = new MonsterRepository(context);
                var skill =
                    repo.Add(new MonsterTemplate());

                return Ok(skill);
            }


        }



    }
}
