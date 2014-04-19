using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Reflection;
using Newtonsoft.Json;
using Server.Game.Zones;
using Server.Infrastructure.Content;
using Server.Infrastructure.Math;
using Server.Infrastructure.Synchronization;
using Server.Utils;
using Server.Utils.Math;

namespace Server.Infrastructure.World
{
    /// <summary>
    /// An entity is a basic mobile object that can be interacted with in the world.
    /// </summary>
    public abstract class Entity
    {


        protected SyncMonitor PropertyCollection;

        private static ulong _idCounter = 0;
        protected Vector2 _position = new Vector2();
        private string _name;
        private ulong _id;
        private string _sprite;


        /// <summary>
        /// Creating an entity can be fairly expensive due to the sync procedure, making a lot of them
        /// is not advised. 
        /// </summary>
        protected Entity(string sprite)
        {
            PropertyCollection = new SyncMonitor();
            _sprite = sprite;
            var bodyInfo = ContentManager.Current.Load<EntityBody>(PathHelper.SpritesPath + _sprite + ".json");
            Body = new EntityBody(bodyInfo.Width, bodyInfo.Height);

            Id = _idCounter++;
        }


        /// <summary>
        /// Checks whether this entity can see another in the defined global view
        /// This is useful for checking whether a client has the potential to see a entity or not
        /// </summary>
        /// <param name="entity">The entity to check for in view</param>
        /// <returns>Returns true if the entity is in view, otherwise false.</returns>
        public bool IsInView(Entity entity)
        {
            const int viewWidth = 1920;
            const int viewHeight = 1080;

            var sourceRectangle = new Rectangle(Position.X - (viewWidth / 2), Position.Y - (viewHeight / 2), viewWidth * 2, viewHeight * 2);
            var destinationRectangle = new Rectangle(entity.Position.X, entity.Position.Y, 1, 1);

            return sourceRectangle.Intersects(destinationRectangle);
        }

        public int X
        {
            get { return (int)_position.X; }
        }

        public int Y
        {
            get { return (int)_position.Y; }
        }

        [JsonIgnore]
        public Vector2 Position
        {
            get { return _position; }
            set { MoveEntity(value); }
        }

        /// <summary>
        /// Internally moves the entity to the location, notifying other clients as well.
        /// </summary>
        /// <param name="location">The new location to move the entity into</param>
        protected virtual void MoveEntity(Vector2 location)
        {
            _position = location;
        }

        public Dictionary<string, dynamic> GetSyncProperties()
        {
            if (!PropertyCollection.IsSynced)
                return PropertyCollection.GetAndFlushValues();
            return null;
        }

        /// <summary>
        /// The unique ID that identifies this entity
        /// </summary>
        public ulong Id
        {
            get { return _id; }
            set { _id = value; PropertyCollection.WriteValue("Id", value); }
        }

        public string Sprite
        {
            get { return _sprite; }
            set { _sprite = value; PropertyCollection.WriteValue("Sprite", value); }
        }

        public EntityBody Body { get; private set; }

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
