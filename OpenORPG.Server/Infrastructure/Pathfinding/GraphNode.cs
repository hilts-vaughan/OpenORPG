using System;
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

        public int F { get; set; }
        public int G { get; set; }
        public int H { get; set; }
        public GraphNode Parent { get; set; }
        public Point Position { get; set; }
    }
}
