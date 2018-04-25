using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetRogue.Core {
    public static class PathFinder {
        class AStarNode : IComparable<AStarNode> {
            public Point pos;
            public float costFromStart;
            public float potentialCostToEnd;
            public AStarNode previous;

            public int CompareTo(AStarNode other) {
                return potentialCostToEnd.CompareTo(other.potentialCostToEnd);
            }
        }

        public static List<Point> AStar(Level map, Point start, Point end) {
            // TODO: Priority queue instead of list
            List<AStarNode> open = new List<AStarNode>();
            HashSet<Point> closed = new HashSet<Point>();

            AStarNode startnode = new AStarNode {
                pos = start,
                costFromStart = 0,
                potentialCostToEnd = (float)Point.Distance(start, end),
                previous = null,
            };
            open.Add(startnode);


            AStarNode goalNode = null;
            while (open.Any()) {
                AStarNode current = null;
                foreach (var item in open) {
                    if (current == null || item.potentialCostToEnd < current.potentialCostToEnd) {
                        current = item;
                    }
                }

                if (current.pos == end) {
                    goalNode = current;
                    break;
                }

                Point[] neigh = new Point[] {
                    current.pos + new Point(1,0),
                    current.pos + new Point(0,1),
                    current.pos + new Point(-1,0),
                    current.pos + new Point(0,-1),
                };
                var valid = neigh.Where(x => map.GetTile(x) == Tile.Floor);
                foreach (var item in valid) {
                    if (!closed.Contains(item) && open.All(x => x.pos != item)) {
                        open.Add(new AStarNode {
                            pos = item,
                            costFromStart = current.costFromStart + 1,
                            potentialCostToEnd = (float)Point.Distance(item, end) + current.costFromStart + 1,
                            previous = current,
                        });
                    }
                }
                open.Remove(current);
                closed.Add(current.pos);
            }

            List<Point> path = new List<Point>();

            while (goalNode.previous != null) {
                path.Add(goalNode.pos);
                goalNode = goalNode.previous;
            }

            path.Reverse();

            return path;
        }
    }
}
