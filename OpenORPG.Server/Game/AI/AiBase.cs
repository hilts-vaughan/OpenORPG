using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Game.Entities;
using Server.Infrastructure.Math;
using Server.Utils.Math;
using ServiceStack;

namespace Server.Game.AI
{

    /// <summary>
    /// Represents an interface to an artificial intelligence strategy.
    /// All AI strategies that are implemented in the world must implement this class.
    /// This is for combat or non-combat interfaces both.
    /// </summary>
    public abstract class AiBase
    {
        /// <summary>
        /// Gets the character that this AI module is currently controlling. 
        /// </summary>
        public Character Character { get; set; }

        protected AiBase(Character character)
        {
            if(character == null)
                throw new ArgumentNullException("A null character is not acceptable for an AI module");

            Character = character;
        }


        protected Point GetTileGridPoints()
        {
            var body = Character.Body.GetBodyRectangle();
            var x = body.X;
            var y = body.Y;

            return new Point(MathHelper.ToLowMultiple(x, 32) / 32, MathHelper.ToLowMultiple(y, 32) / 32);
        }

        /// <summary>
        /// Handles the logic for this <see cref="Character"/> that needs to be performed.
        /// </summary>
        /// <param name="deltaTime"></param>
        public abstract void PerformUpdate(float deltaTime);


    }
}
