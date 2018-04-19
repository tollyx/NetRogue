using System;
using System.Collections.Generic;
using System.Linq;

namespace NetRogue.Core {
    public class Level {
        public readonly int Width;
        public readonly int Height;

        private Tile[,] tiles;
        private Tile initial;

        public Level(int width, int height, Tile initial = Tile.None) {
            this.initial = initial;
            Width = width;
            Height = height;
            tiles = new Tile[width, height];
            if (initial != Tile.None) {
                for (int y = 0; y < height; y++) {
                    for (int x = 0; x < width; x++) {
                        tiles[x, y] = initial;
                    }
                }
            }
        }

        public bool IsInsideBounds(Point p) {
            return IsInsideBounds(p.x, p.y);
        }

        public bool IsInsideBounds(int x, int y) {
            return x >= 0 && x < Width && y >= 0 && y < Height;
        }

        public Tile GetTile(int x, int y) {
            if (IsInsideBounds(x, y)) {
                return tiles[x, y];
            }
            return initial;
        }

        public void SetTile(int x, int y, Tile t) {
            if (IsInsideBounds(x, y)) {
                tiles[x, y] = t;
            }
        }

        public void SetTile(Point point, Tile tile) {
            SetTile(point.x, point.y, tile);
        }

        public Tile GetTile(Point point) {
            return GetTile(point.x, point.y);
        }

        public static Level GenerateMazeLevel(int width, int height, int seed) {
            var level = new Level(width, height, Tile.Wall);
            var rng = new Random(seed);
            var pointStack = new Stack<Point>();
            pointStack.Push(new Point(rng.Next(1, width - 1), rng.Next(1, height - 1)));

            while(pointStack.Any()) {
                var point = pointStack.Peek();
                level.SetTile(point, Tile.Floor);
                var neigh = new Point[] {
                    new Point(-2, 0) + point,
                    new Point(2, 0) + point,
                    new Point(0, -2) + point,
                    new Point(0, 2) + point,
                };
                var candidates = neigh.Where(p => level.IsInsideBounds(p) && (level.GetTile(p) == Tile.Wall || rng.Next() % 75 == 0)).ToList();
                if (candidates.Any()) {
                    var next = candidates[rng.Next(candidates.Count)];
                    var tween = point + (next - point) / 2;
                    level.SetTile(tween, Tile.Floor);
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