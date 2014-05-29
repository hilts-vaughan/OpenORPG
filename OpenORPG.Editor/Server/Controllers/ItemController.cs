using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Server.Game.Database;
using Server.Game.Database.Models.ContentTemplates;

namespace OpenORPG.Editor.Server.Controllers
{

    [RoutePrefix("api/items")]
    public class ItemController : ApiController
    {
        [HttpGet]
        [Route("{id:int}")]
        public ItemTemplate GetItemById(int id)
        {
   
            using (var context = new GameDatabaseContext())
            {
                return context.ItemTemplates.FirstOrDefault(x => x.Id == id);
            }
        }


        [HttpGet]
        [Route("")]
        public IEnumerable<ItemTemplate> GetItemsWithNameStartingWith()
        {
            using (var context = new GameDatabaseContext())
            {
                return context.ItemTemplates.ToList();
            }
        }

        [HttpDelete]
        [Route("{id:int}")]
        public IHttpActionResult DeleteItem(int id)
        {
            using (var context = new GameDatabaseContext())
            {
                var item = context.ItemTemplates.FirstOrDefault(x => x.Id == id);

                if (item != null)
                {
                    var entry = context.Entry(item);
                    entry.State = EntityState.Deleted;
                    context.SaveChanges();
                    return Ok();
                }

                return NotFound();
            }
        }


    }
}
