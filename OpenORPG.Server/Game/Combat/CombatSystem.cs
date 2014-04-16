using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Game.Combat.Actions;
using Server.Game.Database;
using Server.Game.Database.Models.ContentTemplates;
using Server.Game.Entities;
using Server.Game.Network.Packets;
using Server.Game.Zones;
using Server.Infrastructure.Logging;
using Server.Infrastructure.World;
using Server.Infrastructure.World.Systems;

namespace Server.Game.Combat
{
    public class CombatSystem : GameSystem
    {

        private ActionGenerator _actionGenerator;

        public CombatSystem(Zone world) : base(world)
        {
            _actionGenerator = new ActionGenerator();
        }



        public override void Update(float frameTime)
        {
            throw new NotImplementedException();
        }

        public override void OnEntityAdded(Entity entity)
        {
            throw new NotImplementedException();
        }

        public override void OnEntityRemoved(Entity entity)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Fetches all combat ready characters within the system.
        /// </summary>
        /// <returns></returns>
        private IEnumerable<Character> GetCombatCharactersInRange()
        {
            // We can implement some filtering here later to determine who is eligible for combat if required
            return Zone.ZoneCharacters;
        }


        /// <summary>
        /// Processes and consumes a request for a skill use on a specific hero.
        /// </summary>
        /// <param name="requestingHero"></param>
        /// <param name="skillRequest"></param>
        public void ProcessCombatRequest(Player requestingHero, ClientUseSkillPacket skillRequest)
        {
            var skillId = skillRequest.SkillId;
            var targetId = skillRequest.TargetId;

            SkillTemplate skill;

            using (var context = new GameDatabaseContext())
            {
                // Fetch our skill from the database
                skill = context.SkillTemplates.FirstOrDefault(x => x.Id == skillId);
            }

            // Checks if the skill requested did not exist
            if (skill == null)
            {
                Logger.Instance.Info("{0} requested the use of the non-existent skill #{1}", requestingHero.Name, skillId);
                return;
            }
                
            // Skill is good, execute!
            var action = _actionGenerator.GenerateActionFromSkill(skill, targetId, requestingHero);

            if (action == null)
                return;

            // Performs the action
            action.PerformAction(GetCombatCharactersInRange());
        }


    }
}
