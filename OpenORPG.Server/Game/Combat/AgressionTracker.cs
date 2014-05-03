using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Server.Game.Entities;

namespace Server.Game.Combat
{
    /// <summary>
    /// Keeps track of aggression levels for a <see cref="Character"/> that is engaged in battle.
    /// 
    /// </summary>
    public class AgressionTracker
    {
        // A dictionary that keeps track of current agression levels
        private Dictionary<ulong, int> _agressionLevels = new Dictionary<ulong, int>();

        /// <summary>
        /// Clears the agression track and wipes all agression.
        /// </summary>
        public void ClearAgression()
        {
            _agressionLevels.Clear();
        }

        public void IncreaseAgression(Character character, int amount)
        {
            if (_agressionLevels.ContainsKey(character.Id))
                _agressionLevels[character.Id] += amount;
            else
                _agressionLevels.Add(character.Id, amount);
        }

        public int GetAgressionLevel(Character character)
        {
            if (_agressionLevels.ContainsKey(character.Id))
                return _agressionLevels[character.Id];
            return 0;
        }

        public bool HasAgression()
        {
            // Returns whether or not anyone has agression greater than zero
            return _agressionLevels.Any(x => x.Value > 0);
        }



        public ulong GetCharacterIdWithMostAgression()
        {
            int n = int.MinValue;
            ulong z = 0;

            foreach (var x in _agressionLevels)
            {
                if (x.Value > n)
                {
                    n = x.Value;
                    z = x.Key;
                }
            }

            return z;
        }

        public void DecayAgression(Character character)
        {

        }


    }
}
