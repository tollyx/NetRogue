using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetRogue.Core {
    class Goblin : Actor {
        public override Tile Glyph => Tile.Goblin;

        public override bool IsPlayer => false;

        public Goblin(Point position) : base(position) {
        }

        public Goblin(int x, int y) : base(x, y) {
        }
    }
}
