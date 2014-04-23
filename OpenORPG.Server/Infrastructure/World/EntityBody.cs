using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Utils.Math;

namespace Server.Infrastructure.World
{
    /// <summary>
    /// An entity body represents information regarding collision boxes and the size of a given entity
    /// </summary>
    public class EntityBody
    {
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

        public EntityBody(int width, int height)
        {
            // Clamps the values to the nearest multiple
            Width = MathHelper.ToNextMultiple(width, 32);
            Height = MathHelper.ToNextMultiple(height, 32);
        }

        public EntityBody()
        {
            
        }


       
    }
}
