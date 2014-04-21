using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Game.AI;
using Server.Game.Combat;
using Server.Game.Items.Equipment;
using Server.Infrastructure.World;
using Server.Utils.Math;

namespace Server.Game.Entities
{
    public class Character : Entity
    {
        private int _speed;
        private CharacterState _characterState;

        public Character(string sprite)
            : base(sprite)
        {
            //TODO: Be a bit more creative than this
            Speed = 32;

            // We initialize the size of our stats here
            var numberOfStats = Enum.GetNames(typeof(StatTypes)).Length;
            CharacterStats = new CharacterStat[numberOfStats];

            // Allocate just enough room for equipment
            var numberOfEquipmentSlots = Enum.GetNames(typeof(EquipmentSlot)).Length;
            Equipment = new Equipment[numberOfEquipmentSlots];

            Skills = new List<Skill>();
        }


        /// <summary>
        /// An array of stats for this character
        /// </summary>
        public CharacterStat[] CharacterStats { get; set; }
        public Equipment[] Equipment { get; set; }

        public List<Skill> Skills { get; set; }

        public int ZoneId { get; set; }

        /// <summary>
        /// The current AI this Character will be running. 
        /// This is typically only applicable to NPCs
        /// but in some cases AI can be attached to players, too.
        /// 
        /// If this is null, no AI is currently running.
        /// </summary>
        public AiBase CurrentAi { get; set; }

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
                    PropertyCollection.WriteValue("CharacterState", value.ToString());

                _characterState = value;

            }
        }

        public int Speed
        {
            get { return _speed; }
            set { _speed = value; PropertyCollection.WriteValue("Speed", value); }
        }

        protected override void MoveEntity(Vector2 location)
        {
            base.MoveEntity(location);

            // For now, we can simply move the object. 
            //TODO: We should send sync packets to keep things in sync
            _position = location;



        }
    }
}
