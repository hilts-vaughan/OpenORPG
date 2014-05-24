using System.Collections.Generic;
using System.Linq;
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


    }
}
