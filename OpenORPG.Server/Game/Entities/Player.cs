using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Server.Game.Combat;
using Server.Game.Database;
using Server.Game.Database.Models;
using Server.Game.Storage;
using Server.Infrastructure.Quests;
using Server.Infrastructure.World;

namespace Server.Game.Entities
{
    public class Player : Character
    {
        private UserHero _hero;

        public Player(string sprite, GameClient client, UserHero userHero)
            : base(sprite)
        {
            Client = client;
            Backpack = new ItemStorage();
            Bank = new ItemStorage();
            QuestInfo = new List<UserQuestInfo>();

            using (var context = new GameDatabaseContext())
            {
                // Add the skills this player knows
                foreach (var skillEntry in userHero.Skills)
                {
                    var skillTemplate = context.SkillTemplates.First(x => x.Id == skillEntry.SkillId);
                    var skill = new Skill(skillTemplate);
                    Skills.Add(skill);
                }

                // Add the state of the quest world to the user
                foreach (var questEntry in userHero.QuestInfo)
                {
                    QuestInfo.Add(questEntry);
                }

            }


            // Store the user hero internally
            _hero = userHero;
        }

        public long HomepointZoneId
        {
            get { return _hero.HomepointZoneId; }
            set { _hero.HomepointZoneId = value; }
        }

        public long HomepointZoneX
        {
            get { return _hero.HomepointZoneX; }
            set { _hero.HomepointZoneX = value; }
        }

        public long HomepointZoneY
        {
            get { return _hero.HomepointZoneY; }
            set { _hero.HomepointZoneY = value; }
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

        public List<UserQuestInfo> QuestInfo { get; set; }


    }
}
