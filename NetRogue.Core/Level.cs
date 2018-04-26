using System;
using System.Collections.Generic;
using System.Linq;

namespace NetRogue.Core {
    public class Level {
        public TileMap<Tile> Map { get; private set; }

        public Level(int width, int height, Tile initial = Tile.None) {
            Map = new TileMap<Tile>(width, height, initial);
        }

        public static Level GenerateMazeLevel(int width, int height, int seed) {
            var level = new Level(width, height, Tile.Wall);
            var rng = new Random(seed);
            var pointStack = new Stack<Point>();

            // Add the random start point for the maze
            // The extra adding * doubling is to make sure said point is on odd x & y coords,
            // which will make sure that we will always have a border around the map (assuming the w/h is even)
            pointStack.Push(new Point(1+rng.Next(0, (width-1)/2)*2, 1+rng.Next(0, (height-1)/2)*2));

            while(pointStack.Any()) {
                var point = pointStack.Peek();
                level.Map.SetTile(point, Tile.Floor);

                // We are incrementing by two to make sure turns only happen on even or odd tiles, not both.
                // Makes the maze much less messy and simplifies the "can we dig here?" check by a lot.
                var neigh = new Point[] {
                    new Point(-2, 0) + point,
                    new Point(2, 0) + point,
                    new Point(0, -2) + point,
                    new Point(0, 2) + point,
                };

                var candidates = neigh.Where(p => 
                        level.Map.IsInsideBounds(p) && 
                        (level.Map.GetTile(p) == Tile.Wall || rng.Next() % 80 == 0)
                    ).ToList();
                if (candidates.Any()) {
                    var next = candidates[rng.Next(candidates.Count)];
                    var tween = point + (next - point) / 2;
                    level.Map.SetTile(tween, Tile.Floor);
                    pointStack.Push(next);
                }
                else {
                    pointStack.Pop();
                }
            }

            return level;
        }
    }
}