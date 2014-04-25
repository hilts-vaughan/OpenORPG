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

        /// <summary>
        /// A list of pending actions for this system to perform.
        /// </summary>
        private readonly List<ICombatAction> _pendingActions = new List<ICombatAction>();

        public CombatSystem(Zone world)
            : base(world)
        {
            _actionGenerator = new ActionGenerator();
        }



        public override void Update(float frameTime)
        {
            var toRemove = new List<ICombatAction>();

            foreach (var pendingAction in _pendingActions)
            {
                // Decrement time remaining
                pendingAction.ExecutionTime -= frameTime;

                if (pendingAction.ExecutionTime < 0f)
                {
                    var result = pendingAction.PerformAction(GetCombatCharactersInRange());
                    toRemove.Add(pendingAction);
                    pendingAction.ExecutingCharacter.CharacterState = CharacterState.Idle;

                    // If success 
                    if (result.TargetId != 0 && result.Damage != 0)
                    {
                        var packet = new ServerSkillUseResult(pendingAction.ExecutingCharacter.Id, (ulong) result.TargetId, result.Damage);
                        Zone.SendToEntitiesInRange(packet, pendingAction.ExecutingCharacter);
                    }

                    // Force skill onto cooldown
                    pendingAction.Skill.EnableCooldown();
                }

            }

            // Remove skill
            toRemove.ForEach(x => _pendingActions.Remove(x));

            foreach (var c in Zone.ZoneCharacters)
            {
                foreach (var skill in c.Skills)
                    skill.Cooldown -= frameTime;

                if (c.CharacterStats[(int)StatTypes.Hitpoints].CurrentValue <= 0 && c is Monster)
                    Zone.RemoveEntity(c);
            }


        }

        public override void OnEntityAdded(Entity entity)
        {

        }

        public override void OnEntityRemoved(Entity entity)
        {

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


            // You may only use a skill if you are idle
            if (requestingHero.CharacterState != CharacterState.Idle)
                return;

            // Fetch skill
            Skill skill = requestingHero.Skills.Find(x => x.SkillTemplate.Id == skillRequest.SkillId);

            // Checks if the skill requested did not exist
            if (skill == null)
            {
                Logger.Instance.Info("{0} requested the use of the non-existent skill #{1}", requestingHero.Name, skillId);
                return;
            }

            // Check that this skill is usable
            if (!skill.CanUse())
                return;

            // Skill is good, execute!
            var action = _actionGenerator.GenerateActionFromSkill(skill, targetId, requestingHero);

            if (action == null)
                return;

            // Set the execution time and add it in
            action.ExecutionTime = skill.SkillTemplate.CastTime;

            // Add the action to the queue to be executed 
            _pendingActions.Add(action);

            Logger.Instance.Debug("Preparing for {0} to perform skill #{1}", requestingHero.Name, skillId);

            // Character is casting now
            requestingHero.CharacterState = CharacterState.UsingSkill;


        }


    }
}
