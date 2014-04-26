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
    /// 
    /// There is no logic checking done here for mobs already occupying spots or anything special.
    /// Just uses a basic random number generator to generate spots
    /// </summary>
    public class WanderAi : AiBase
    {
        private float _idleTimer = 0f;
        private const float WanderTime = 5f;
        private Random _random = new Random();

        private const int MaxWanderX = 4;
        private const int MaxWnaderY = 4;

        public WanderAi(Character character)
            : base(character)
        {

        }

        public override void PerformUpdate(float deltaTime)
        {
            // When walking, we should ignore everything else and just walk our path
            if (DestinationNodes.Count > 0)
            {
                WalkPath(deltaTime);
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
            var gridPoint = GetTileGridPoints();

            var newX = gridPoint.X + _random.Next(-MaxWanderX, MaxWanderX + 1);
            var newY = gridPoint.Y + _random.Next(-MaxWnaderY, MaxWnaderY + 1);

            var searcher = new AStarSearcher(Character.Zone.TileMap, new Point(newX, newY),
                new Point(gridPoint.X, gridPoint.Y));
            var results = searcher.GeneratePath(false);

            var destList = new List<Vector2>();

            // If it's even possible
            if (results.Count > 0)
            {

                foreach (var result in results)
                {
                    var point = new Point(result.X * 32 - Character.Body.OffsetX, result.Y * 32 - Character.Body.OffsetY);
                    destList.Add(new Vector2(point.X, point.Y));
                }

                BeginPath(destList);
            }

        }

        private void ToRandomSpot()
        {
            // Get our position in the actual tilemap
            var gridPoint = GetTileGridPoints();
            var gridX = gridPoint.X;
            var gridY = gridPoint.Y;

            var destX = 3;
            var destY = 3;

            var searcher = new AStarSearcher(this.Character.Zone.TileMap, new Point(destX, destY), new Point(gridX, gridY));
            var results = searcher.GeneratePath(true);

            var destList = new List<Vector2>();

            foreach (var result in results)
            {
                var point = new Point(result.X * 32 - Character.Body.OffsetX, result.Y * 32 - Character.Body.OffsetY);
                destList.Add(new Vector2(point.X, point.Y));
            }

            BeginPath(destList);
        }


    }
}
