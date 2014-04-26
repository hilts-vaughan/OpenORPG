using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Infrastructure.Math;
using Server.Utils.Math;

namespace Server.Infrastructure.World
{
    /// <summary>
    /// An entity body represents information regarding collision boxes and the size of a given entity
    /// </summary>
    public class EntityBody
    {
        private Entity _parent;

        /// <summary>
        /// The width of this body (this is in tiles)
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// The height of this body (this is in tiles)
        /// </summary>
        public int Height { get; set; }

        public int OffsetX { get; set; }
        public int OffsetY { get; set; }

        public EntityBody(Entity parent, int width, int height, int offsetX, int offsetY)
        {
            // Clamps the values to the nearest multiple
            Width = MathHelper.ToNextMultiple(width, 32);
            Height = MathHelper.ToNextMultiple(height, 32);
            OffsetX = offsetX;
            OffsetY = offsetY;

            _parent = parent;
        }

        /// <summary>
        /// Returns a bounding box that represents the entitys body that represents their
        /// hitbox and essentially their 'body'. This is seperated from their actual sprite
        /// and position. The position represents the top left of a sprite, while the body is said
        /// to usually represent where the feet usually rest on a sprite. 
        /// 
        /// Collision testing and movement are usually done with this box.
        /// </summary>
        /// <returns></returns>
        public Rectangle GetBodyRectangle()
        {
            return new Rectangle(_parent.Position.X + OffsetX, _parent.Position.Y + OffsetY, Width, Height);
        }

        public EntityBody()
        {
            
        }


       
    }
}
