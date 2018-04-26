using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetRogue.Core.Entities {
    public class Item : IEntity {
        string name;
        Point position;
        Tile glyph;

        public string Name => name;
        public Point Position => position;
        public Tile Glyph => glyph;

        public Item(string name, Point position, Tile glyph) {
            this.name = name;
            this.position = position;
            this.glyph = glyph;
        }

        public static Item Coin(int x, int y) => new Item("Coin", new Point(x, y), Tile.Coin);
    }
}
