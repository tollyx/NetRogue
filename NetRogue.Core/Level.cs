using NetRogue.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NetRogue.Core {
    public class Level {
        public TileMap<Tile> Map { get; private set; }

        public IReadOnlyList<Actor> Actors { get => actors; }
        public IReadOnlyList<Item> Items { get => items; }
        List<Actor> actors;
        List<Item> items;
        QuadTree<Actor> acttree;
        QuadTree<Item> itemtree;
        public QuadTree<Actor> ActorTree { get => acttree; }
        public QuadTree<Item> ItemTree { get => itemtree; }

        public Level(int width, int height, Tile initial = Tile.None) {
            Map = new TileMap<Tile>(width, height, initial);
            actors = new List<Actor> {
                new Goblin(1, 23),
                new Goblin(63, 1),
                new Goblin(63, 23),
            };
            items = new List<Item> {
                Item.Coin(3, 3),
                Item.Coin(3, 5),
                Item.Coin(5, 7),
            };
            acttree = new QuadTree<Actor>(new Rect(Map.Width, Map.Height));
            foreach (var act in actors) {
                acttree.Add(act);
            }
            itemtree = new QuadTree<Item>(new Rect(Map.Width, Map.Height));
            foreach (var item in items) {
                itemtree.Add(item);
            }
        }

        public Actor GetActorAt(Point pos) {
            return acttree.GetAt(pos);
        }

        public IEnumerable<Item> GetItemsAt(Point pos) {
            return itemtree.GetMultipleAt(pos);
        }

        public void AddActor(Actor actor) {
            actors.Add(actor);
            acttree.Add(actor);
        }

        public bool RemoveActor(Actor actor) {
            return actors.Remove(actor) && acttree.Remove(actor);
        }

        public void AddItem(Item item) {
            items.Add(item);
            itemtree.Add(item);
        }

        public bool RemoveItem(Item item) {
            return items.Remove(item) && itemtree.Remove(item);
        }

        public int GetActorIndex(Actor actor) {
            return actors.IndexOf(actor);
        }

        public void Cleanup() {
            for (int i = actors.Count - 1; i >= 0; i--) {
                if (!actors[i].IsAlive) {
                    acttree.Remove(actors[i]);
                    actors.RemoveAt(i);
                }
            }
            acttree.Cleanup();
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