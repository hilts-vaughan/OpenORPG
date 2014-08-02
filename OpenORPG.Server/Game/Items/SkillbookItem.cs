using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenORPG.Database.DAL;
using Server.Game.Combat;
using Server.Game.Database;
using Server.Game.Database.Models.ContentTemplates;
using Server.Game.Entities;
using Server.Infrastructure.Logging;

namespace Server.Game.Items
{
    /// <summary>
    /// A <see cref="SkillbookItem"/> is capable of granting a user a skill permanently when used. 
    /// The item will be consumed when used, typically.
    /// </summary>
    public class SkillbookItem : Item
    {
        public SkillbookItem(ItemTemplate itemTemplate)
            : base(itemTemplate)
        {

        }


        public override void UseItemOn(Character character)
        {

            var player = character as Player;

            if (player != null)
            {


                using (var context = new GameDatabaseContext())
                {
                    var skillRepo = new SkillRepository(context);
                    var skillTemplate = skillRepo.Get(ItemTemplate.LearntSkillId);

                    player.AddSkill(new Skill(skillTemplate));
                }

            }

            else
            {
                Logger.Instance.Warn("You cannot use a skillbook on a non-player target.");
            }


        }
    }
}
