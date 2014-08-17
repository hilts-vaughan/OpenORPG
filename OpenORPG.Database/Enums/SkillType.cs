using System;

namespace OpenORPG.Database.Enums
{
    public enum SkillAttribute
    {
        Fire,
        Ice,
        Wind,
        Earth,
        Thunder,
        Water,
        Light, 
        Dark
    }

    /// <summary>
    /// Defines a skill type
    /// </summary>
    public enum SkillType
    {
        None,
        Elemental,
        Enfeebling,
        Enhancing,
        Healing,
        Divine,
        Physical
    }

    /// <summary>
    /// An enumeration of categories of skills available.
    /// </summary>
    public enum SkillCategory
    {
        Weapon,
        BlackMagic,
        WhiteMagic,
        Ability,
        Ninjutsu
    }


    [Flags]
    public enum SkillTargetType
    {
        /// <summary>
        /// The player flags is any player, regardless of being an ally or not
        /// </summary>
        Self = 1,

        /// <summary>
        /// The ally flag is for any ally member
        /// </summary>
        Ally = 2,

        /// <summary>
        /// The enemy flag is for any enemy
        /// </summary>
        Enemy = 4,

        /// <summary>
        /// The anyone flag is for any passerbyer
        /// </summary>
        Anyone = 8
    }

    public enum SkillActivationType
    {
        Immediate,
        Target,
        Projectile
    }

    /// <summary>
    /// This represents any possible states that a quest can potentially be in.
    /// </summary>
    public enum QuestState
    {
        Available,
        InProgress,
        Finished

    }

    /// <summary>
    /// An enumeration of equipment slots that are available for <see cref="Character"/>s.
    /// These are flags as combining them is possible
    /// </summary>
    [Flags]
    public enum EquipmentSlot
    {
        Weapon,
        Head,
        Body,
        Back,
        Feet,
        Hands
    }

    public interface IQuestRequirementTable
    {

        /// <summary>
        /// The quest Id that this entry represents a requirement for.
        /// </summary>
        long QuestTableId { get; set; }

    }
}