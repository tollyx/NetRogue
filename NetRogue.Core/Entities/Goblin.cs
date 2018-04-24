using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetRogue.Core {
    class Goblin : Actor {
        public override Tile Glyph => Tile.Goblin;

        public override bool IsPlayer => false;

        public override string Name => "Goblin";

        public Goblin(Point position) : base(position) {
        }

        public Goblin(int x, int y) : base(x, y) {
        }

        public override void Tick(World world) {
            base.Tick(world);
        }

        public override void Think(World world) {
            if (world.Player?.IsAlive ?? false) {
                var path = PathFinder.AStar(world.Level, Position, world.Player.Position);
                if (path.Any()) {
                    var next = path.First();
                    var act = world.GetActorAt(next);
                    if (act == null) {
                        Action = new MoveAction(this, (next - Position).Direction());
                    }
                    else if (act is Player) {
                        Action = new AttackAction(this, act);
                    }
                }
            }

            if (Action == null) {
                Action = new WaitAction();
            }
        }
    }
}
