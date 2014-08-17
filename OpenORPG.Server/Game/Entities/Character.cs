using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using OpenORPG.Database.Enums;
using Server.Game.AI;
using Server.Game.Combat;
using Server.Game.Combat.Effects;
using Server.Game.Items.Equipment;
using Server.Game.Network.Packets.Server;
using Server.Infrastructure.World;
using Server.Utils.Math;

namespace Server.Game.Entities
{

    /// <summary>
    /// A signature for events when a character has died
    /// </summary>
    /// <param name="aggressor">The aggressor is the person whom was killed</param>
    /// <param name="victim"></param>
    public delegate void CharacterKilled(Character aggressor, Character victim);

    public class Character : Entity
    {

        // Events are up here

        /// <summary>
        /// This event is raised when this character dies.
        /// The concept of death occurs when the hitpoints of a character
        /// reaches zero. 
        /// </summary>
        public event CharacterKilled Killed;

        protected virtual void OnKilled(Character aggressor, Character victim)
        {
            CharacterKilled handler = Killed;
            if (handler != null) handler(aggressor, victim);
        }


        private int _speed;
        private CharacterState _characterState;

        public Character(string sprite)
            : base(sprite)
        {
            //TODO: Be a bit more creative than this
            Speed = 130;

            CharacterStats = new CharacterStatCollection(this);


            // Allocate just enough room for equipment
            var numberOfEquipmentSlots = Enum.GetNames(typeof(EquipmentSlot)).Length;
            Equipment = new Equipment[numberOfEquipmentSlots];

            CharacterStats[StatTypes.Hitpoints].CurrentValueChanged += OnHitpointsChanged;

            Skills = new List<Skill>();
            ActiveStatusEffects = new StatusEffectCollection();
        }

        private void OnHitpointsChanged(long oldValue, long newValue, StatTypes statType)
        {
            if (newValue <= 0)
                OnKilled(_lastCharacterToHitMe, this);
        }


        /// <summary>
        /// An array of stats for this character
        /// </summary>
        public CharacterStatCollection CharacterStats { get; set; }

        public StatusEffectCollection ActiveStatusEffects { get; private set; }


        [JsonIgnore]
        public Equipment[] Equipment { get; set; }

        public List<Skill> Skills { get; set; }

        public int ZoneId { get; set; }

        public virtual int Level { get; set; }

        /// <summary>
        /// The current AI this Character will be running. 
        /// This is typically only applicable to NPCs
        /// but in some cases AI can be attached to players, too.
        /// 
        /// If this is null, no AI is currently running.
        /// </summary>
        [JsonIgnore]
        public AiBase CurrentAi { get; set; }

        public ulong TargetId { get; private set; }

        public void SelectTarget(Character target)
        {
            TargetId = target.Id;
        }

        /// <summary>
        /// The current state of this character is stored here
        /// </summary>
        public CharacterState CharacterState
        {
            get { return _characterState; }
            set
            {
                // Write only if value changed
                if (value != _characterState)
                    PropertyCollection.WriteValue("CharacterState", value);

                _characterState = value;

            }
        }

        public int Speed
        {
            get { return _speed; }
            set { _speed = value; PropertyCollection.WriteValue("Speed", value); }
        }

        public bool IsAlive
        {
            get { return CharacterStats[(int)StatTypes.Hitpoints].CurrentValue > 0; }
        }

        private Character _lastCharacterToHitMe;
        private int _level;

        public void ApplyDamage(DamagePayload damagePayload)
        {

            _lastCharacterToHitMe = damagePayload.Aggressor;
      

            // Applies the actual damage to this character
            damagePayload.Apply(this);

            // If this died, tell any interested parties
            if (CharacterStats[(int)StatTypes.Hitpoints].CurrentValue <= 0)
            {
                CharacterStats[(int)StatTypes.Hitpoints].CurrentValue = 0;
            }


        }

        protected override void MoveEntity(Vector2 location)
        {
            base.MoveEntity(location);

            _position = location;

            // Syncs the position to the rest of the clients that can see this
            SyncPosition();

        }

        private void SyncPosition()
        {
            // We only need to bother syncing if we're attached somewhere yet
            if (Zone != null)
            {
                // Send this packet to all interested parties
                var newPacket = new ServerEntityMovementPacket(_position, Direction, this.Id);
                Zone.SendToEntitiesInRangeExcludingSource(newPacket, this);
            }

        }

        public void UseSkill(long skillId, long targetId)
        {
            var combatSystem = Zone.GetGameSystem<CombatSystem>();
            if (combatSystem != null)
                combatSystem.ProcessCombatRequest(this, skillId, (int) targetId);
        }



    }
}
