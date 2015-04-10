using System;
using System.Security.Policy;
using Server.Infrastructure.Math;

namespace Server.Infrastructure.Pathfinding
{
    class GraphNode
    {
        public GraphNode(Point position, GraphNode parent)
        {
            Position = position;
            Parent = parent;
        }

        //NOTE: We don't use auto-properties here since this is a hotspot of code.
        // It actual takes a fairly hefty penalty hit using auto properties here, so we just don't

        public int F;
        public int G;
        public int H;
        public GraphNode Parent;
        public readonly Point Position;



        public override int GetHashCode()
        {
            return (F + G + H)*(Position.X - Position.Y);
        }
    }
}
