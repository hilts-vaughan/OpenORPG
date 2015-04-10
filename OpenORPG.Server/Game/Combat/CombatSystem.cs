using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenORPG.Database.Enums;
using Server.Game.Combat.Actions;
using Server.Game.Combat.Validators.Skills;
using Server.Game.Database;
using Server.Game.Database.Models.ContentTemplates;
using Server.Game.Entities;
using Server.Game.Network.Packets;
using Server.Game.Zones;
using Server.Infrastructure.Logging;
using Server.Infrastructure.World;
using Server.Infrastructure.World.Systems;
using Server.Utils.Math;

namespace Server.Game.Combat
{
    public class CombatSystem : GameSystem
    {

        private ActionGenerator _actionGenerator;

        /// <summary>
        /// A list of pending actions for this system to perform.
        /// </summary>
        private readonly List<CombatAction> _pendingActions = new List<CombatAction>();

        public CombatSystem(Zone world)
            : base(world)
        {
            _actionGenerator = new ActionGenerator();
        }



        public override void Update(double frameTime)
        {

            // Perform any skills that may be pending in the queue
            PerformPendingSkills(frameTime);

            // Decrease cooldowns as required
            foreach (var c in Zone.ZoneCharacters)
            {
                DecreaseCooldowns(frameTime, c);
            }

        }

        private static void DecreaseCooldowns(double frameTime, Character character)
        {
            foreach (var skill in character.Skills)
                skill.Cooldown -= frameTime;
        }

        private void PerformPendingSkills(double frameTime)
        {
            var toRemove = new List<CombatAction>();

            foreach (var pendingAction in _pendingActions)
            {
                // Decrement time remaining
                pendingAction.ExecutionTime -= frameTime;

                if (pendingAction.ExecutionTime < 0f)
                {
                    toRemove.Add(pendingAction);
                    ActivateSkill(pendingAction);
                }
            }

            // Remove all pending skills
            toRemove.ForEach(x => _pendingActions.Remove(x));
        }

        private void ActivateSkill(CombatAction pendingAction)
        {
            // Execute the skill and reset the character state
            var results = pendingAction.PerformAction(GetCombatCharactersInRange());
            pendingAction.ExecutingCharacter.CharacterState = CharacterState.Idle;

            SendActionResult(pendingAction, results);

            CalculateAggression(pendingAction, results);

            // Force skill onto cooldown
            pendingAction.Skill.ResetCooldown();
        }

        private void CalculateAggression(CombatAction pendingAction, List<CombatActionResult> results)
        {
            // Increase aggro as required
            foreach (var result in results)
            {


                var victim = Zone.ZoneCharacters.FirstOrDefault(x => x.Id == (ulong)result.TargetId);
                if (victim != null && victim.CurrentAi != null)
                    victim.CurrentAi.AgressionTracker.IncreaseAgression(pendingAction.ExecutingCharacter, 1);
            }

        }

        private void SendActionResult(CombatAction action, List<CombatActionResult> results)
        {

            foreach (var result in results)
            {
                if (result.TargetId != -1)
                {
                    var packet = new ServerSkillUseResult(action.ExecutingCharacter.Id, (ulong)result.TargetId,
                        result.Damage, action.Skill.Id);
                    Zone.SendToEntitiesInRange(packet, action.ExecutingCharacter);
                }

            }

        }


       
        private void CharacterOnKilled(Character aggressor, Character victim)
        {
            if (victim is Player)
            {
                // Restore hitpoints
                victim.CharacterStats[(int)StatTypes.Hitpoints].CurrentValue =
                    victim.CharacterStats[(int)StatTypes.Hitpoints].MaximumValue;

                var player = victim as Player;
                var homePoint = ZoneManager.Instance.FindZone(player.HomepointZoneId);

                if (homePoint != null)
                    ZoneManager.Instance.SwitchToZoneAndPosition(player, homePoint, new Vector2(player.HomepointZoneX, player.HomepointZoneY));
            }

            if (victim is Monster)
            {
                victim.Zone.ChatChannel.SendMessage(GameMessage.MonsterDies, new List<string>()
                {
                    victim.Name
                });

                Zone.RemoveEntity(victim);


                // Aware EXP if needed
                var player = aggressor as Player;
                if (player != null)
                    player.Experience += GetExperienceRelative(player, victim);

            }

        }

        private int GetExperienceRelative(Character killer, Character victim)
        {
            var levelDifference = killer.Level - victim.Level;
            var expPenalty = levelDifference * 25;
            return Math.Max(200 - expPenalty, 0);
        }

        private void OnCharacterRemoved(Character character)
        {
            character.Killed -= CharacterOnKilled;
        }


        private void OnCharacterAdded(Character character)
        {
            character.Killed += CharacterOnKilled;
        }


        public override void OnEntityAdded(Entity entity)
        {
            if (entity is Character)
                OnCharacterAdded(entity as Character);
        }

        public override void OnEntityRemoved(Entity entity)
        {
            if (entity is Character)
                OnCharacterRemoved(entity as Character);
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
        public void ProcessCombatRequest(Character requestingHero, long skillId, int targetId)
        {


            // You may only use a skill if you are idle
            if (requestingHero.CharacterState != CharacterState.Idle)
                return;

            // Fetch skill
            Skill skill = requestingHero.Skills.Find(x => x.SkillTemplate.Id == skillId);

            // Checks if the skill requested did not exist
            if (skill == null)
            {
                Logger.Instance.Info("{0} requested the use of the non-existent skill #{1}", requestingHero.Name, skillId);
                return;
            }


            var target = Zone.ZoneCharacters.FirstOrDefault(x => (int)x.Id == targetId);

            if (!SkillValidationUtility.CanPerformSkill(requestingHero, target, skill))
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
