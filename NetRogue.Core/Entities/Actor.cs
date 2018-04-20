using System;

namespace NetRogue.Core {
    public abstract class Actor : IEntity {
        Point position;
        public Point Position { get => position; }
        public abstract Tile Glyph { get; }
        public abstract bool IsPlayer { get; }
        public bool IsAlive => Health > 0;
        public int Health { get; protected set; }
        public int Strength { get; protected set; }
        public int Defence { get; protected set; }
        public ActorAction Action { get; set; }

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

        internal void Attack(Actor defender) {
            if (defender.IsAlive) {
                defender.Damage(this, Strength);
            }
        }

        private void Damage(Actor attacker, int damage) {
            Health -= Math.Max(damage - Defence, 1);
        }
    }
}