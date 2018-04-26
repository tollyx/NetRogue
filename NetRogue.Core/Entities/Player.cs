using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetRogue.Core.Entities {
    public class Player : Actor {
        public override Tile Glyph => Tile.Player;

        public override bool IsPlayer => true;

        public override string Name => "Hero";

        public LimitedList<Item> Inventory { get; } = new LimitedList<Item>(10);

        public Player(Point position) : base(position) { }

        public Player(int x, int y) : base(x, y) { }

        internal override void Attack(Actor defender) {
            var msg = $"You attack the {defender.Name}!";
            var dmg = defender.Health;
            base.Attack(defender);
            dmg -= defender.Health;
            msg += $" You hit for {dmg} damage!";
            if (!defender.IsAlive) msg += $" The {defender.Name} dies from the blow!";
            Log.Message(msg);
        }

        internal override void Defend(Actor attacker, int damage) {
            var msg = $"The {attacker.Name} attacks you!";
            var dmg = Health;
            base.Defend(attacker, damage);
            dmg -= Health;
            msg += $" You take {dmg} damage!";
            if (!IsAlive) msg += $" You die from the blow!";
            Log.Message(msg);
        }
    }
}
