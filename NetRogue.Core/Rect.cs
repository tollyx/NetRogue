using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetRogue.Core {
    public struct Rect {
        public int x;
        public int y;
        public int w;
        public int h;

        public int Left => x;
        public int Right => x + w-1;
        public int Top => y;
        public int Bottom => y + h-1;
        public Point TopLeft => new Point(Left, Top);
        public Point BottomLeft => new Point(Left, Bottom);
        public Point TopRight => new Point(Right, Top);
        public Point BottomRight => new Point(Right, Bottom);

        public Rect(int width, int height) : this(0, 0, width, height) { }

        public Rect(int x, int y, int w, int h) {
            this.x = x;
            this.y = y;
            this.w = w;
            this.h = h;
        }

        public bool Contains(Point p) {
            return Contains(p.x, p.y);
        }

        public bool Contains(int px, int py) {
            return px >= Left && px <= Right && py >= Top && py <= Bottom;
        }

        public bool Intersects(Rect other) {
            return !(Right < other.Left || Left > other.Right
                  || Bottom < other.Top || Top > other.Bottom);
        }

        public void Destruct(out int x, out int y, out int w, out int h) {
            x = this.x;
            y = this.y;
            w = this.w;
            h = this.h;
        }
    }
}
