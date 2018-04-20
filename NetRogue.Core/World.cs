using System;
using System.Collections.Generic;
using System.Linq;

namespace NetRogue.Core {
    public class World {
        Level level;
        public Level Level { get => level; }

        int ticks;
        int currentActor;

        public IReadOnlyList<Actor> Actors { get => actors; }
        List<Actor> actors;
        QuadTree<Actor> tree;
        public Actor Player { get; private set; }
        public QuadTree<Actor> Tree { get => tree; }

        public World() {
            ticks = 0;
            level = Level.GenerateMazeLevel(64, 24, Environment.TickCount);
            Player = new Player(11, 11);
            actors = new List<Actor> {
                Player,
                new Goblin(3, 3),
            };
            tree = new QuadTree<Actor>(new Rect(64, 24));
            foreach (var item in actors) {
                tree.Add(item);
            }

        }

        public Actor GetActorAt(Point pos) {
            return tree.GetAt(pos);
        }

        public bool IsPlayerTurn() {
            return actors[currentActor].IsPlayer;
        }

        public void SetPlayerAction(ActorAction action) {
            Player.Action = action;
        }

        public bool Tick() {
            ticks++;

            actors[currentActor].Action?.Execute(this);
            actors[currentActor].Action = null;

            currentActor++;
            Cleanup();
            currentActor %= actors.Count;
            return false;
        }

        public void Cleanup() {
            for (int i = actors.Count - 1; i >= 0; i--) {
                if (!actors[i].IsAlive) {
                    tree.Remove(actors[i]);
                    actors.RemoveAt(i);
                    if (currentActor > i) {
                        currentActor--;
                    }
                }
            }
        }
    }
}