using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Game.Combat;
using Server.Game.Entities;
using Server.Game.Network.Packets;
using Server.Game.Zones;
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

        private void ChaseTarget()
        {

        }

        public override void PerformUpdate(float deltaTime)
        {
            if (!Character.IsAlive)
                return;

            //TODO: If there is agression and a target is close by
            if (AgressionTracker.HasAgression())
            {
                var victim = GetVictim();

                if (Vector2.Distance(victim.Position, Character.Position) < 70)
                {
                    if (Character.CharacterState == CharacterState.Moving)
                    {
                        Character.CharacterState = CharacterState.Idle;
                        EndPath();
                    }


                    FaceVictim();

                    Character.SelectTarget(victim);
                    Character.UseSkill(1, (int) victim.TargetId);
                }
                else
                {



                    if (DestinationNodes.Count == 0)
                        ChasePlayer();

                    else
                    {
                        UpdatePath();
                        DestinationNodes = new Queue<Vector2>(RegeneratePath());
                    }
                }


            }

            // When walking, we should ignore everything else and just walk our path
            if (DestinationNodes.Count > 0)
            {
                WalkPath(deltaTime);
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

        private void FaceVictim()
        {
            var victim = GetVictim();

            // Do something nice here
            if (CombatUtility.CanSeeInDirection(Character, victim, Direction.North))
            {
                Character.Direction = Direction.North;
                return;
            }

            // Do something nice here
            if (CombatUtility.CanSeeInDirection(Character, victim, Direction.South))
            {
                Character.Direction = Direction.South;
                return;
            }

            // Do something nice here
            if (CombatUtility.CanSeeInDirection(Character, victim, Direction.East))
            {
                Character.Direction = Direction.East;
                return;
            }

            // Do something nice here
            if (CombatUtility.CanSeeInDirection(Character, victim, Direction.West))
            {
                Character.Direction = Direction.West;
                return;
            }


        }


        private void ChasePlayer()
        {


            var destList = RegeneratePath();


            BeginPath(destList);




        }

        private List<Vector2> RegeneratePath()
        {
            var victim = GetVictim();
            var start = GetTileGridPoints();

            var gridPoint = GetTileGridPoints(victim);

            var newX = gridPoint.X;
            var newY = gridPoint.Y;

            var searcher = new AStarSearcher(Character.Zone.TileMap, new Point(newX, newY),
                new Point(start.X, start.Y));
            var results = searcher.GeneratePath(true);

            var destList = new List<Vector2>();


            // If it's even possible
            if (results.Count > 0)
            {
                results.RemoveAt(0);
                results.RemoveAt(results.Count - 1);

                foreach (var result in results)
                {
                    var point = new Point(result.X * 32 - Character.Body.OffsetX, result.Y * 32 - Character.Body.OffsetY);
                    destList.Add(new Vector2(point.X, point.Y));
                }

            }
            else
            {
                AgressionTracker.RemoveAgression(victim.Id);
                EndPath();
            }

            // ResetPath();

            return destList;
        }

        private Character GetVictim()
        {
            var victim =
                Character.Zone.ZoneCharacters.First(x => x.Id == AgressionTracker.GetCharacterIdWithMostAgression());
            return victim;
        }


        private void TakeRandomStep()
        {
            var gridPoint = GetTileGridPoints();

            var newX = gridPoint.X + _random.Next(-MaxWanderX, MaxWanderX + 1);
            var newY = gridPoint.Y + _random.Next(-MaxWnaderY, MaxWnaderY + 1);

            var searcher = new AStarSearcher(Character.Zone.TileMap, new Point(newX, newY),
                new Point(gridPoint.X, gridPoint.Y));
            var results = searcher.GeneratePath(true);

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
