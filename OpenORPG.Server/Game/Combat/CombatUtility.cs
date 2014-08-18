using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenORPG.Database.Enums;
using Server.Game.Entities;
using Server.Game.Network.Packets;
using Server.Infrastructure.Math;
using Server.Infrastructure.World;

namespace Server.Game.Combat
{
    public static class CombatUtility
    {

        /// <summary>
        /// Computes the damage of a skill given a user, victim and skill. This takes into account affinity
        /// resistance and all potential resistances.
        /// </summary>
        /// <param name="victim"></param>
        /// <param name="skill"></param>
        public static int ComputeDamage(Character user, Character victim, Skill skill)
        {
            var damageModifier = 1d;

            switch (skill.SkillTemplate.SkillType)
            {
                case SkillType.Elemental:
                    damageModifier += GetElementalModifier(user);
                    break;
                case SkillType.Healing:
                    damageModifier += GetHealingModifier(user);
                    damageModifier *= -1;
                    break;
                case SkillType.Physical:
                    damageModifier += GetPhysicalModifier(user);
                    break;                    
            }

            // Grab the damage modifier
            damageModifier = Math.Round(damageModifier, 3, MidpointRounding.AwayFromZero);

            var totalDamage = skill.SkillTemplate.Damage * damageModifier;

            return (int)totalDamage;
        }

        private static double GetPhysicalModifier(Character user)
        {
            var stats = GetCharacterStats(user);
            return stats[StatTypes.Strength].CurrentValue/1.5f;
        }

        private static float GetElementalModifier(Character user)
        {
            var stats = GetCharacterStats(user);
            return stats[StatTypes.Intelligence].CurrentValue / 10f;
        }

        private static float GetHealingModifier(Character user)
        {
            var stats = GetCharacterStats(user);
            return stats[StatTypes.Mind].CurrentValue / 5f;
        }


        private static CharacterStatCollection GetCharacterStats(Character character)
        {
            var statsWithEquipmentMods = new CharacterStatCollection();

            // Include character stats
            statsWithEquipmentMods = CombineStats(statsWithEquipmentMods, character.CharacterStats);

            // Get inclusive equipment stats
            foreach (var equip in character.Equipment)
            {
                if (equip != null)
                    statsWithEquipmentMods = CombineStats(equip.GetEquipmentModifierStats(), statsWithEquipmentMods);
            }

            return statsWithEquipmentMods;
        }



        /// <summary>
        /// A utility method that is used to combine two stat collections together (summing them up)
        /// </summary>
        /// <param name="first">The first stat collection to combine</param>
        /// <param name="second">The second stat collection to combine</param>
        /// <returns></returns>
        private static CharacterStatCollection CombineStats(CharacterStatCollection first, CharacterStatCollection second)
        {
            var stats = new CharacterStatCollection();

            for (int index = 0; index < first.Length; index++)
            {
                var stat = first[index];
                var stat2 = second[index];

                stats[index].CurrentValue = stat.CurrentValue + stat2.CurrentValue;
                stats[index].MaximumValue = stat.MaximumValue + stat2.MaximumValue;
            }

            return stats;
        }


        public static bool CanSee(Entity eye, Entity goal)
        {

            var ray = GetRay(eye);
            var goalRect = goal.Body.GetBodyRectangle();

            if (ray.Intersects(goalRect))
                return true;

            return false;
        }

        public static bool CanSeeInDirection(Entity eye, Entity goal, Direction direction)
        {
            var ray = GetRayInDirection(eye, direction);
            var goalRect = goal.Body.GetBodyRectangle();

            if (ray.Intersects(goalRect))
                return true;

            return false;
        }

        private static Rectangle GetRayInDirection(Entity eye, Direction direction)
        {
            var rect = eye.Body.GetBodyRectangle();
            switch (direction)
            {
                case Direction.North:
                    var y = rect.Y;
                    rect.Y = 0;
                    rect.Height = y;
                    break;
                case Direction.South:
                    rect.Height = 9999999;
                    break;
                case Direction.West:
                    var x = rect.X;
                    rect.X = 0;
                    rect.Width = x;
                    break;
                case Direction.East:
                    rect.Width = 9999999;
                    break;
            }

            return rect;
        }

        private static Rectangle GetRay(Entity eye)
        {
            var rect = eye.Body.GetBodyRectangle();
            switch (eye.Direction)
            {
                case Direction.North: ;
                    var y = rect.Y;
                    rect.Y = 0;
                    rect.Height = y;
                    break;
                case Direction.South:
                    rect.Height = 9999999;
                    break;
                case Direction.West:
                    var x = rect.X;
                    rect.X = 0;
                    rect.Width = x;
                    break;
                case Direction.East:
                    rect.Width = 9999999;
                    break;
            }

            return rect;
        }
    }
}
