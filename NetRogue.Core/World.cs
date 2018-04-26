using NetRogue.Core.Actions;
using NetRogue.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NetRogue.Core {
    public class World {
        Level level;
        public Level Level { get => level; }

        int ticks;
        int currentActor;

        public Player Player { get; private set; }

        public Log Log { get; private set; }

        public World() {
            Log = new Log();
            ticks = 0;
            level = Level.GenerateMazeLevel(64, 24, Environment.TickCount);
            Player = new Player(1, 1);
            level.AddActor(Player);
        }

        public bool IsPlayerTurn() {
            return level.Actors[currentActor].IsPlayer;
        }

        public void SetPlayerAction(IActorAction action) {
            Player.Action = action;
        }

        public void SetPlayerMove(Direction dir) {
            var act = Level.GetActorAt(Player.Position + dir.ToPoint());
            if (act == null) {
                SetPlayerAction(new MoveAction(Player, dir));
            }
            else {
                SetPlayerAction(new AttackAction(Player, act));
            }
        }

        public bool Tick() {
            ticks++;
            var current = level.Actors[currentActor];
            current.Think(this);
            if (current.Action == null) return false;

            if (current.Action.Execute(this)) {
                current.Tick(this);
                current.Action = null;

                // Switch to the next actor, skipping any dead ones.
                do {
                    currentActor++;
                    currentActor %= level.Actors.Count;
                } while (!level.Actors[currentActor].IsAlive);

                // Remember the current actor and gets its index after cleanup,
                // since its index might change after the cleanup.
                var act = level.Actors[currentActor];
                level.Cleanup();
                currentActor = level.GetActorIndex(act);

                return true;
            }
            current.Action = null;
            return false;
        }
    }
}