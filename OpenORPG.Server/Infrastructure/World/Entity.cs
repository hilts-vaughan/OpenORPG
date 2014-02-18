using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using Newtonsoft.Json;
using PropertyChanged;
using Server.Game.Zones;
using Server.Infrastructure.Synchronization;
using Server.Utils.Math;

namespace Server.Infrastructure.World
{
    /// <summary>
    /// An entity is a basic mobile object that can be interacted with in the world.
    /// </summary>
    public abstract class Entity
    {


        protected SyncPropertyCollection PropertyCollection;

        private static ulong _idCounter = 0;
        protected Vector2 _position = new Vector2();
        private string _name;


        /// <summary>
        /// Creating an entity can be fairly expensive due to the sync procedure, making a lot of them
        /// is not advised. 
        /// </summary>
        protected Entity()
        {
            Id = _idCounter++;

            PropertyCollection = new SyncPropertyCollection();
        }


        public Vector2 Position
        {
            get { return _position; }
            set { MoveEntity(value); }
        }

        /// <summary>
        /// Internally moves the entity to the location, notifying other clients as well.
        /// </summary>
        /// <param name="location">The new location to move the entity into</param>
        protected abstract void MoveEntity(Vector2 location);

        public Dictionary<string, dynamic> GetSyncProperties()
        {
            if (!PropertyCollection.IsSynced)
                return PropertyCollection.GetAndFlushValues();
            return null;
        }

        /// <summary>
        /// The unique ID that identifies this entity
        /// </summary>
        public ulong Id { get; set; }

        public string Sprite { get; set; }

        /// <summary>
        /// A reference to the <see cref="World"/> this entity lives within.
        /// </summary>
        [JsonIgnore]
        public Zone Zone { get; set; }

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;

                // Write a new changed value
                PropertyCollection.WriteValue("Name", value);
            }
        }

    }
}
