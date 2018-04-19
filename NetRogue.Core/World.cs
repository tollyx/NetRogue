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
        public Actor Player { get; private set; }

        public World() {
            ticks = 0;
            level = Level.GenerateMazeLevel(120, 30, Environment.TickCount);
            actors = new List<Actor> {
                new Player(10, 10),
                new Goblin(2, 2),
            };
            Player = actors[0];
        }

        public Actor GetActorAt(Point pos) {
            return actors.FirstOrDefault(act => act.Position == pos);
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
                    actors.RemoveAt(i);
                    if (currentActor > i) {
                        currentActor--;
                    }
                }
            }
        }
    }
}