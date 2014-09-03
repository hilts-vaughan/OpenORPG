using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenORPG.Database.Enums;
using Server.Game.Entities;

namespace Server.Game.Combat
{
    /// <summary>
    /// A character stat is a vital part of the makeup of the <see cref="Character"/>.
    /// These usually contain information like maximum and minimums, along with any modifiers.
    /// </summary>
    public class CharacterStat
    {

        public delegate void StatValueChanged(long oldValue, long newValue, StatTypes statType);

        public event StatValueChanged CurrentValueChanged;

        protected virtual void OnCurrentValueChanged(long oldvalue, long newvalue, StatTypes stattype)
        {
            StatValueChanged handler = CurrentValueChanged;
            if (handler != null) handler(oldvalue, newvalue, stattype);
        }

        public event StatValueChanged MaximumValueChanged;

        protected virtual void OnMaximumValueChanged(long oldvalue, long newvalue, StatTypes stattype)
        {
            StatValueChanged handler = MaximumValueChanged;
            if (handler != null) handler(oldvalue, newvalue, stattype);
        }


        private long _maximumValue;
        private long _currentValue;

        public long MaximumValue
        {
            get { return _maximumValue; }
            set
            {
                if (value != _maximumValue)
                {
                    _maximumValue = value;
                    OnMaximumValueChanged(_maximumValue, value, StatType);
                }

            
            }
        }


        public long CurrentValue
        {
            get { return _currentValue; }
            set
            {

                if (value > _maximumValue && _maximumValue != 0)
                    value = _maximumValue;

                if (value == _currentValue)
                    return;

                if (value != _currentValue)
                {
                    _currentValue = value;
                    OnCurrentValueChanged(_currentValue, value, StatType);
                }

              
            }
        }

        public StatTypes StatType { get; private set; }

        public CharacterStat(long maximumValue, long currentValue, StatTypes statType)
        {
            _maximumValue = maximumValue;
            _currentValue = currentValue;
            StatType = statType;
        }
    }
}
