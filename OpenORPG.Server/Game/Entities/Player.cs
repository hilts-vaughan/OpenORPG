using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Server.Game.Combat;
using Server.Game.Database.Models;
using Server.Game.Storage;
using Server.Infrastructure.World;

namespace Server.Game.Entities
{
    public class Player : Character
    {
        public Player(string sprite, GameClient client, UserHero userHero)
            : base(sprite)
        {
            Client = client;
            Backpack = new ItemStorage();
            Bank = new ItemStorage();

            // Add the skills this player knows
            foreach (var skillTemplate in userHero.Skills)
            {
                var skill = new Skill(skillTemplate);
                Skills.Add(skill);
            }
        }

        /// <summary>   
        /// The client attached to this player. 
        /// </summary>
        [JsonIgnore]
        public GameClient Client { get; set; }


        /// <summary>
        /// The backpack this player is currently holding
        /// </summary>
        public ItemStorage Backpack { get; set; }

        /// <summary>
        /// A bank is a players storage, usually it can hold a lot more than the standard backpack.
        /// </summary>
        public ItemStorage Bank { get; set; }

    }
}
