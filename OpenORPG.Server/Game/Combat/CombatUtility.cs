using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Game.Network.Packets;
using Server.Infrastructure.Math;
using Server.Infrastructure.World;

namespace Server.Game.Combat
{
    public static class CombatUtility
    {

        public static bool CanSee(Entity eye, Entity goal)
        {

            var ray = GetRay(eye);
            var goalRect = goal.Body.GetBodyRectangle();

            if (ray.Intersects(goalRect))
                return true;

            return false;
        }

        public static bool CanSeeInDirection(Entity eye, Entity goal, Direction direction)
        {
            var ray = GetRayInDirection(eye, direction);
            var goalRect = goal.Body.GetBodyRectangle();

            if (ray.Intersects(goalRect))
                return true;

            return false;
        }

        private static Rectangle GetRayInDirection(Entity eye, Direction direction)
        {
            var rect = eye.Body.GetBodyRectangle();
            switch (direction)
            {
                case Direction.North:
                    var y = rect.Y;
                    rect.Y = 0;
                    rect.Height = y;
                    break;
                case Direction.South:
                    rect.Height = 9999999;
                    break;
                case Direction.West:
                    var x = rect.X;
                    rect.X = 0;
                    rect.Width = x;
                    break;
                case Direction.East:
                    rect.Width = 9999999;
                    break;
            }

            return rect;
        }

        private static Rectangle GetRay(Entity eye)
        {
            var rect = eye.Body.GetBodyRectangle();
            switch (eye.Direction)
            {
                case Direction.North: ;
                    var y = rect.Y;
                    rect.Y = 0;
                    rect.Height = y;
                    break;
                case Direction.South:
                    rect.Height = 9999999;
                    break;
                case Direction.West:
             var x = rect.X;
                    rect.X = 0;
                    rect.Width = x;
                    break;
                case Direction.East:
                    rect.Width = 9999999;
                    break;
            }

            return rect;
        }
    }
}
