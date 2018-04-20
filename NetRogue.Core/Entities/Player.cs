using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetRogue.Core {
    class Player : Actor {
        public override Tile Glyph => Tile.Player;

        public override bool IsPlayer => true;

        public Player(Point position) : base(position) {
        }

        public Player(int x, int y) : base(x, y) {
        }
    }
}
