using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Game.Combat;
using Server.Game.Entities;
using Server.Game.Network.Packets;
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

        // A list of nodes we wish to traverse to
        protected Queue<Vector2> DestinationNodes = new Queue<Vector2>();

        protected Vector2 Start = Vector2.Zero;
        protected Vector2 Current = Vector2.Zero;

        private float _acc = 0f;

        public AgressionTracker AgressionTracker { get; set; }


        protected void BeginPath(IEnumerable<Vector2> pathPoints)
        {
            foreach (var pathPoint in pathPoints)
            {
                DestinationNodes.Enqueue(pathPoint);
            }

                _acc = 500f;
                      
            ResetPath();

            //ResetPath();

            Character.CharacterState = CharacterState.Moving;



        }

        protected void UpdatePath()
        {
            DestinationNodes.Clear();  
        }

        protected void ResetPath()
        {
            // On the next AI step, we will begin steering towards there
            Start = Character.Position;
            Current = Character.Position;

        }

        protected void EndPath()
        {
            DestinationNodes.Clear();
            Character.CharacterState = CharacterState.Idle;
            Start = Vector2.Zero;
            Current = Vector2.Zero;
        }


        protected AiBase(Character character)
        {
            if (character == null)
                throw new ArgumentNullException("A null character is not acceptable for an AI module");

            Character = character;

            AgressionTracker = new AgressionTracker();
        }


        protected Point GetTileGridPoints()
        {
            var body = Character.Body.GetBodyRectangle();
            var x = body.X;
            var y = body.Y;

            return new Point(MathHelper.ToLowMultiple(x, 32) / 32, MathHelper.ToLowMultiple(y, 32) / 32);
        }

        protected Point GetTileGridPoints(Character character)
        {
            var body = character.Body.GetBodyRectangle();
            var x = body.X;
            var y = body.Y;

            return new Point(MathHelper.ToLowMultiple(x, 32) / 32, MathHelper.ToLowMultiple(y, 32) / 32);
        }


        /// <summary>
        /// Handles the logic for this <see cref="Character"/> that needs to be performed.
        /// </summary>
        /// <param name="deltaTime"></param>
        public abstract void PerformUpdate(float deltaTime);

        protected void WalkPath(float deltaTime)
        {

            // Perform a destination movement
            var node = DestinationNodes.Peek();
            var velocity = new Vector2(Character.Speed * deltaTime, Character.Speed * deltaTime);

            if (Math.Abs(node.Y - Current.Y) < 0.0001f)
                velocity.Y = 0;

            if (Math.Abs(node.X - Current.X) < 0.0001f)
                velocity.X = 0;


            // Change direction as needed
            if (node.X < Start.X)
                velocity.X *= -1;
            if (node.Y < Start.Y)
                velocity.Y *= -1;

            _acc += deltaTime;


            // Head towards
            var newPosition = Current + velocity;

            if (node.X < Start.X)
            {
                // Keep clamped
                newPosition.X = MathHelper.Clamp(newPosition.X, node.X, float.MaxValue);
            }

            if (node.Y < Start.Y)
            {
                newPosition.Y = MathHelper.Clamp(newPosition.Y, node.Y, float.MaxValue);
            }

            if (node.X > Start.X)
            {
                // Keep clamped
                newPosition.X = MathHelper.Clamp(newPosition.X, float.MinValue, node.X);
            }

            if (node.Y > Start.Y)
            {
                newPosition.Y = MathHelper.Clamp(newPosition.Y, float.MinValue, node.Y);
            }


            // Direction code
            if (velocity.X > 0)
                Character.Direction = Direction.East;

            if (velocity.X < 0)
                Character.Direction = Direction.West;

            if (velocity.Y > 0)
                Character.Direction = Direction.South;

            if (velocity.Y < 0)
                Character.Direction = Direction.North;


            Current = newPosition;


            if (_acc > 0.2f)
            {
                _acc = 0f;
                Character.Position = Current;
            }


            if (velocity.X == 0 && velocity.Y == 0)
            {
                // We're done with this node
                _acc = 0f;
                Start = Current;
                Character.Position = Current;
                DestinationNodes.Dequeue();

                if (DestinationNodes.Count == 0)
                    EndPath();
            }
        }


    }
}
