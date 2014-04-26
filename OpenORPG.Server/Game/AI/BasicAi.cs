using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Game.Entities;
using Server.Game.Network.Packets;
using Server.Infrastructure.Math;
using Server.Infrastructure.Pathfinding;
using Server.Utils.Math;
using TiledSharp;

namespace Server.Game.AI
{
    /// <summary>
    /// A basic AI that controls passive characters in the game world.
    ///  
    /// The typical behaviour for an AI acting in this mode is to move randomly into free spots.
    /// </summary>
    public class BasicAi : AiBase
    {
        private float _idleTimer = 0f;

        private const float WanderTime = 1f;

        // A list of nodes we wish to traverse to
        private Queue<Vector2> _destinationNodes = new Queue<Vector2>();
        private Vector2 _start = Vector2.Zero;
        private Vector2 _current = Vector2.Zero;
        private float _acc = 0f;

        public BasicAi(Character character)
            : base(character)
        {

        }

        public override void PerformUpdate(float deltaTime)
        {

            if (_destinationNodes.Count > 0)
            {
                // Perform a destination movement
                var node = _destinationNodes.Peek();
                var velocity = new Vector2(Character.Speed * deltaTime, Character.Speed * deltaTime);

                if ((int)node.Y == (int)_current.Y)
                    velocity.Y = 0;

                if ((int)node.X == (int)_current.X)
                    velocity.X = 0;


                // Change direction as needed
                if (node.X < _start.X)
                    velocity.X *= -1;
                if (node.Y < _start.Y)
                    velocity.Y *= -1;

                _acc += deltaTime;


                // Head towards
                var newPosition = _current + velocity;

                if (node.X < _start.X)
                {
                    // Keep clamped
                    newPosition.X = MathHelper.Clamp(newPosition.X, node.X, float.MaxValue);
                }

                if (node.Y < _start.Y)
                {
                    newPosition.Y = MathHelper.Clamp(newPosition.Y, node.Y, float.MaxValue);
                }

                if (node.X > _start.X)
                {
                    // Keep clamped
                    newPosition.X = MathHelper.Clamp(newPosition.X, float.MinValue, node.X);
                }

                if (node.Y > _start.Y)
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


                _current = newPosition;


                if (_acc > 0.2f)
                {
                    _acc = 0f;
                    Character.Position = _current;
                }


                if ((int)newPosition.X == (int)node.X && (int)newPosition.Y == (int)node.Y)
                {
                    // We're done with this node
                    _acc = 0f;
                    _start = _current;
                    Character.Position = _current;
                    _destinationNodes.Dequeue();

                    if (_destinationNodes.Count == 0)
                        Character.CharacterState = CharacterState.Idle;

                }

                return;
            }

            // When idle, start the timer to wait around
            if (Character.CharacterState == CharacterState.Idle)
                _idleTimer += deltaTime;

            // If the timer has been reached
            if (_idleTimer > WanderTime)
            {
                // Reset
                _idleTimer = 0f;

                TakeRandomStep();
            }

        }


        private void TakeRandomStep()
        {
            // Get our position in the actual tilemap
            var gridPoint = GetTileGridPoints();
            var gridX = gridPoint.X;
            var gridY = gridPoint.Y;

            var destX = 4;
            var destY = 4;

            var searcher = new AStarSearcher(this.Character.Zone.TileMap, new Point(destX, destY), new Point(gridX, gridY));
            var results = searcher.GeneratePath(false);

            var newPoints = new List<Point>();

            foreach (var result in results)
            {
                var point = new Point(result.X * 32 - Character.Body.OffsetX, result.Y * 32 - Character.Body.OffsetY);
                _destinationNodes.Enqueue(new Vector2(point.X, point.Y));
            }


            // On the next AI step, we will begin steering towards there
            _start = Character.Position;
            _current = Character.Position;
            Character.CharacterState = CharacterState.Moving;
        }


    }
}
