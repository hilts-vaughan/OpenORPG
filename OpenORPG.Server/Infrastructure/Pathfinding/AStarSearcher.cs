﻿using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Infrastructure.Math;
using TiledSharp;

namespace Server.Infrastructure.Pathfinding
{
    /// <summary>
    /// An implemenation of the A* alogirithim that is used for seeking to specific spots on a map.
    /// These calls can be semi-expensive so it is advised that path-finding only be used for long range
    /// distances and used sparingly. 
    /// 
    /// This implemenation is actually quite bad -- so use sparingly.
    /// 
    /// </summary>

    // Eric Lippert has a beautiful 
    public class AStarSearcher
    {
        private List<GraphNode> _openList = new List<GraphNode>();
        private HashSet<Point> _closedList = new HashSet<Point>();

        private List<Point> _waypoints = new List<Point>();

        private GraphNode _start;
        private GraphNode _end;

        private GraphNode _current;

        private TmxMap _map;

        public AStarSearcher(TmxMap map, Point dest, Point start)
        {
            _map = map;

            // Setup our start and goal here
            _start = new GraphNode(start, null);
            _end = new GraphNode(dest, null);
        }

        /// <summary>
        /// Generates a path on the given map
        /// </summary>
        /// <param name="diagonal"></param>
        /// <returns></returns>
        public List<Point> GeneratePath(bool diagonal = true)
        {

            // Check for whether it's even possible first, and then return early if it's not
            var adjPoint = GetAdjacentCell(_end.Position.X, _end.Position.Y);
            if(adjPoint == new Point(-1, -1))
                return new List<Point>();

            // Otherwise, begin the A* search

            // Start at the beginning
            _openList.Add(_start);
            _current = _start;

            // Let us find where we need to go
            while (true)
            {
                // If we're out of places to go..
                if (_openList.Count == 0)
                    break;

                _current = GetSmallestF();

                if (_current.Position == _end.Position)
                    break;

                _openList.Remove(_current);
                _closedList.Add(_current.Position);

                if (diagonal)
                {
                    AddAdjacentCellToNodeToOpenList(_current, 1, -1, 14);
                    AddAdjacentCellToNodeToOpenList(_current, -1, -1, 14);
                    AddAdjacentCellToNodeToOpenList(_current, -1, 1, 14);
                    AddAdjacentCellToNodeToOpenList(_current, 1, 1, 14);
                }

                AddAdjacentCellToNodeToOpenList(_current, 0, -1, 10);
                AddAdjacentCellToNodeToOpenList(_current, -1, 0, 10);
                AddAdjacentCellToNodeToOpenList(_current, 1, 0, 10);
                AddAdjacentCellToNodeToOpenList(_current, 0, 1, 10);

            }


            while (_current != null)
            {
                bool endOnClosed = false;
                for (int v = 0; v < _openList.Count; v++)
                    if (_openList[v].Position == _end.Position)
                        endOnClosed = true;
                if (endOnClosed)
                    _waypoints.Add(_current.Position);

                // Walk
                _current = _current.Parent;
            }

            _waypoints.Reverse();
            return _waypoints;
        }

        private void AddAdjacentCellToNodeToOpenList(GraphNode parentNode, int xOffset, int yOffset, int gCost)
        {
            // Get adjacent cell
            var adjacentCell = GetAdjacentCell(parentNode.Position.X + xOffset, parentNode.Position.Y + yOffset);

            // Ignore blocked and invalid points
            if (adjacentCell == new Point(-1, -1))
                return;



            /**
             * 
             *      1) A* requires searching the entire god damn map sometimes; runs on the same thread; will search every node to find a path just to find out that it's not traversable

                    2) If the path-finding leads to somewhere that is ultimately not reachable, the code will stutter horribly and die.

                    Optimizations:

                    1) Make sure we don't bother path finding to a place that is unreachable
                    2) Remove linearity from closedList; replace it with something more high speed (O(1), hash set?)
             * 
             *      #2 was implemented here, gains are huge as is vs. a linear search
             * 
             * 
             */

            // Ignore stuff on the closed list; hash set for performance reasons
            if (_closedList.Contains(adjacentCell))
                return;

            var adjacentNode = _openList.SingleOrDefault(n => n.Position == adjacentCell);

            if (adjacentNode != null)
            {
                if (parentNode.G + gCost < adjacentNode.G)
                {
                    adjacentNode.Parent = parentNode;
                    adjacentNode.G = parentNode.G + gCost;
                    adjacentNode.F = adjacentNode.G + adjacentNode.H;
                }

                return;
            }

            // Otherwise, set some parameters for ourselves
            var dist = GetDistance(adjacentCell, _end.Position);
            var newNode = new GraphNode(adjacentCell, parentNode) { G = gCost, H = dist };
            newNode.F = newNode.G + newNode.H;
            _openList.Add(newNode);
        }



        private Point GetAdjacentCell(int x, int y)
        {
            if (x > _map.Width - 1 || y > _map.Height - 1 || x < 0 || y < 0)
                return new Point(-1, -1);

            if (_map.BlockMap[x, y])
                return new Point(-1, -1);

            return new Point(x, y);
        }

        /// <summary>
        /// In the currently open list, find the smallest F value object.
        /// </summary>
        /// <returns>A node with the smallest F (cost) in the open list.</returns>
        private GraphNode GetSmallestF()
        {
            var smallestF = int.MaxValue;
            GraphNode selectedNode = null;

            foreach (var node in _openList)
            {
                if (node.F < smallestF)
                {
                    selectedNode = node;
                    smallestF = node.F;
                }
            }

            return selectedNode;
        }

        private int GetDistance(Point start, Point end)
        {
            var startX = start.X * 32 / _map.Width;
            var startY = start.Y * 32 / _map.Height;

            var endX = end.X * 32 / _map.Width;
            var endY = end.Y * 32 / _map.Height;

            return System.Math.Abs(startX - endX) + System.Math.Abs(startY - endY);

        }



    }
}
