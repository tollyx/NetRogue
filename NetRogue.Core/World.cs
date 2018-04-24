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
        public Log Log { get; private set; }

        public World() {
            Log = new Log();
            ticks = 0;
            level = Level.GenerateMazeLevel(64, 24, Environment.TickCount);
            Player = new Player(1, 1);
            actors = new List<Actor> {
                Player,
                new Goblin(1, 23),
                new Goblin(63, 1),
                new Goblin(63, 23),
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

        public void SetPlayerAction(IActorAction action) {
            Player.Action = action;
        }

        public void SetPlayerMove(Direction dir) {
            var act = GetActorAt(Player.Position + dir.ToPoint());
            if (act == null) {
                SetPlayerAction(new MoveAction(Player, dir));
            }
            else {
                SetPlayerAction(new AttackAction(Player, act));
            }
        }

        public bool Tick() {
            ticks++;
            var current = actors[currentActor];
            current.Think(this);
            if (current.Action == null) return false;

            if (current.Action.Execute(this)) {
                current.Tick(this);
                current.Action = null;

                currentActor++;
                Cleanup();
                currentActor %= actors.Count;
                return true;
            }
            current.Action = null;
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
            tree.Cleanup();
        }
    }
}