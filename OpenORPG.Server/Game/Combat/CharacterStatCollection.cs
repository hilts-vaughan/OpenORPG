
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Game.Entities;

namespace Server.Game.Combat
{
    /// <summary>
    /// A simple collection of character stats that provides a simple indexer API to retrieve and set values.
    /// </summary>
    public class CharacterStatCollection
    {

        public delegate void StatValueChanged(long oldValue, long newValue, StatTypes statType, Character tracking);

        public event StatValueChanged CurrentValueChanged;


        private CharacterStat[] _stats;
        private Character _tracking;

        public CharacterStatCollection()
        {
            Setup();
        }

        private CharacterStat[] Stats
        {
            get { return _stats; }
        }

        public CharacterStatCollection(Character character)
        {
            Setup();
            _tracking = character;
        }


        private void Setup()
        {
            // We initialize the size of our stats here
            var numberOfStats = Enum.GetNames(typeof(StatTypes)).Length;
            _stats = new CharacterStat[numberOfStats];

            foreach (var enumValue in Enum.GetValues(typeof(StatTypes)))
            {
                _stats[(int)enumValue] = new CharacterStat(0, 0, (StatTypes)enumValue);
                this[(StatTypes)enumValue].CurrentValueChanged += OnCurrentValueChanged;
                this[(StatTypes)enumValue].MaximumValueChanged += OnMaximumValueChanged;
            }
        }



        ~CharacterStatCollection()
        {
            foreach (var stat in _stats)
            {
                stat.CurrentValueChanged -= OnCurrentValueChanged;
                stat.MaximumValueChanged -= OnMaximumValueChanged;
            }
        }


        public CharacterStat this[StatTypes i]
        {
            get
            {
                // This indexer is very simple, and just returns or sets 
                // the corresponding element from the internal array. 
                return _stats[(int)i];
            }
            set
            {
                _stats[(int)i] = value;
            }
        }



        public CharacterStat this[int i]
        {
            get
            {
                // This indexer is very simple, and just returns or sets 
                // the corresponding element from the internal array. 
                return _stats[(int)i];
            }
            set
            {
                _stats[(int)i] = value;
            }
        }


        protected virtual void OnCurrentValueChanged(long oldvalue, long newvalue, StatTypes stattype)
        {
            StatValueChanged handler = CurrentValueChanged;
            if (handler != null) handler(oldvalue, newvalue, stattype, _tracking);
        }

        public event StatValueChanged MaximumValueChanged;

        protected virtual void OnMaximumValueChanged(long oldvalue, long newvalue, StatTypes stattype)
        {
            StatValueChanged handler = MaximumValueChanged;
            if (handler != null) handler(oldvalue, newvalue, stattype, _tracking);
        }





        public int Length
        {
            get { return _stats.Length; }
        }

    }
}
