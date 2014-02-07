using System.ComponentModel;
using Newtonsoft.Json;
using Server.Game.Zones;
using Server.Utils.Math;

namespace Server.Infrastructure.World
{
    /// <summary>
    /// An entity is a basic mobile object that can be interacted with in the world.
    /// 
    /// </summary>
    public abstract class Entity
    {


        private static ulong _idCounter = 0;
        protected Vector2 _position = new Vector2();


        protected Entity()
        {
            Id = _idCounter++;
        }


        public Vector2 Position
        {
            get { return _position; }
            set { MoveEntity(value);  }
        }

        /// <summary>
        /// Internally moves the entity to the location, notifying other clients as well.
        /// </summary>
        /// <param name="location">The new location to move the entity into</param>
        protected abstract void MoveEntity(Vector2 location);
        

        /// <summary>
        /// The unique ID that identifies this entity
        /// </summary>
        public ulong Id { get; set; }

        public string Sprite { get; set; }

        /// <summary>
        /// A reference to the <see cref="World"/> this entity lives within.
        /// </summary>
        [JsonIgnore]
        public Zone Zone{ get; set; }

        public string Name { get; set; }
    }
}
