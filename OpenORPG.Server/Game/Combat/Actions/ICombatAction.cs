using Server.Game.Entities;

namespace Server.Game.Combat.Actions
{
    /// <summary>
    /// Combat actions are the basis of interacting with other <see cref="Character"/>s in combat.
    /// Every action that is taken against another <see cref="Character"/> is a skill.
    /// 
    /// This includes attacking, healing, magic and items that are used.
    /// </summary>
    public interface ICombatAction
    {
        void PerformAction();
    }
}
