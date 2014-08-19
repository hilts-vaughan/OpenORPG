using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Game.Combat;
using Server.Game.Entities;
using Server.Infrastructure.Scripting;
using Server.Infrastructure.Scripting.Combat;

namespace OpenORPG.Server.Scripts.Combat.Skills
{
    /// <summary>
    /// A skill that deals lethal damage when used. Bypasses all damage computations and ensures fatality by returning an absurd amount of damage.
    /// </summary>
    [GameScript("Skill_Deathstrike")]
    public class DeathstrikeSkillScript : SkillScript
    {
        public DeathstrikeSkillScript()
        {
        }

        public override int OnComputeDamage(Character attacker, Character victim, int damage)
        {
            return damage * 50000;
        }
    }
}
