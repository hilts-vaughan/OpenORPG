using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Game.Combat.Effects
{
    /// <summary>
    /// A collection of status effects that is capable of
    /// </summary>
    public class StatusEffectCollection : IEnumerable<StatusEffect>
    {

        public delegate void StatusEffectEvent(StatusEffect effect);

        public event StatusEffectEvent EffectAdded;

        protected virtual void OnEffectAdded(StatusEffect effect)
        {
            StatusEffectEvent handler = EffectAdded;
            if (handler != null) handler(effect);
        }

        public event StatusEffectEvent EffectRemoved;

        protected virtual void OnEffectRemoved(StatusEffect effect)
        {
            StatusEffectEvent handler = EffectRemoved;
            if (handler != null) handler(effect);
        }

        private List<StatusEffect> _effects = new List<StatusEffect>();

        public StatusEffectCollection()
        {
        }

        public void AddStatusEffect(StatusEffect effect)
        {
            _effects.Add(effect);
            OnEffectAdded(effect);
        }

        public void RemoveStatusEffect(StatusEffect effect)
        {
            _effects.Remove(effect);
            OnEffectRemoved(effect);
        }

        

        public IEnumerator<StatusEffect> GetEnumerator()
        {
            return _effects.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
