using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetRogue.Core {
    public enum Direction {
        None,
        Up,
        Down,
        Left,
        Right,
    }

    public static class Extensions {
        public static Point ToPoint(this Direction dir) {
            switch (dir) {
                default:
                case Direction.None:
                    return new Point(0, 0);
                case Direction.Up:
                    return new Point(0, -1);
                case Direction.Down:
                    return new Point(0, 1);
                case Direction.Left:
                    return new Point(-1, 0);
                case Direction.Right:
                    return new Point(1, 0);
            }
        }
    }
}
