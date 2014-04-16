using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Game.Combat.Payloads;
using Server.Game.Entities;

namespace Server.Game.Combat
{
    /// <summary>
    /// An attack payload is an attack that is ready to be applied to any entity.
    /// A payload does all the computations for damage and application of status effects.
    /// 
    /// Any combat damage should be applied via a payload so subsystems can track this.
    /// </summary>
    public class DamagePayload : Payload
    {
        public DamagePayload(Character aggressor) : base(aggressor)
        {

        }

        public override void Apply(Character victim)
        {
            // Get the final stats from both our targets
            var aggressorFinalStats = GetCharacterStats(Aggressor);
            var victimFinalStats = GetCharacterStats(victim);

            // Compute the damage using a basic formula for now
            var damageToDeal = aggressorFinalStats[(int)StatTypes.Strength].CurrentValue * 2 - victimFinalStats[(int) StatTypes.Vitality].CurrentValue;

            victim.CharacterStats[(int) StatTypes.Hitpoints].CurrentValue -= damageToDeal;
        }

 

        private CharacterStat[] GetCharacterStats(Character character)
        {
            var statsWithEquipmentMods = new CharacterStat[character.Equipment.Length];

            // Include character stats
            statsWithEquipmentMods = CombineStats(statsWithEquipmentMods, character.CharacterStats);

            // Get inclusive equipment stats
            statsWithEquipmentMods = character.Equipment.Aggregate(statsWithEquipmentMods,
                (current, equip) => CombineStats(current, equip.GetEquipmentModifierStats()));
            return statsWithEquipmentMods;
        }



        private CharacterStat[] CombineStats(CharacterStat[] first, CharacterStat[] second)
        {
            var stats = new CharacterStat[first.Length];

            for (int index = 0; index < first.Length; index++)
            {
                var stat = first[index];
                var stat2 = second[index];

                stats[index].CurrentValue = stat.CurrentValue + stat2.MaximumValue;
                stats[index].MaximumValue = stat.MaximumValue + stat2.MaximumValue;
            }

            return stats;
        }


    }
}
