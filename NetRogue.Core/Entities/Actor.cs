using System;

namespace NetRogue.Core.Entities {
    public abstract class Actor : IEntity {
        Point position;
        public Point Position { get => position; }
        public abstract Tile Glyph { get; }
        public abstract bool IsPlayer { get; }
        public bool IsAlive => Health > 0;
        public int Health { get; protected set; }
        public int Strength { get; protected set; }
        public int Defence { get; protected set; }
        public IActorAction Action { get; set; }
        public abstract string Name { get; }

        public Actor(int x, int y) {
            position = new Point(x, y);
            Health = 10;
            Strength = 5;
            Defence = 0;
        }

        public Actor(Point position) {
            this.position = position;
        }

        internal void Move(Direction dir) {
            Move(dir.ToPoint());
        }

        internal void Move(Point dir) {
            position += dir;
        }

        internal virtual void Attack(Actor defender) {
            if (defender.IsAlive) {
                defender.Defend(this, Strength);
            }
        }

        internal virtual void Defend(Actor attacker, int damage) {
            Health -= Math.Max(damage - Defence, 1);
        }

        public virtual void Tick(World world) {
            if (IsAlive) {
                // Do stuff
            }
        }

        public virtual void Think(World world) { }
    }
}