using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Infrastructure.World;
using Server.Utils.Math;

namespace Server.Game.Entities
{
    public class Character : Entity
    {
        private int _speed;

        public Character()
        {
            //TODO: Be a bit more creative than this
            Sprite = "male_base";
            Speed = 32;
        }

        /// <summary>
        /// The amount of hitpoints a character has
        /// </summary>
        public int HP { get; set; }


        public int ZoneId { get; set; }


        public int Speed
        {
            get { return _speed; }
            set { _speed = value; PropertyCollection.WriteValue("Speed", value); }
        }

        protected override void MoveEntity(Vector2 location)
        {
            // For now, we can simply move the object. 
            //TODO: We should send sync packets to keep things in sync
            _position = location;
        }
    }
}
