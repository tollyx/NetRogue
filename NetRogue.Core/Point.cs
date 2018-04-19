namespace NetRogue.Core {
    public struct Point {
        public int x;
        public int y;

        public Point(int x, int y) {
            this.x = x;
            this.y = y;
        }

        public static Point operator +(Point a, Point b) {
            return new Point {
                x = a.x + b.x,
                y = a.y + b.y,
            };
        }

        public static Point operator -(Point a, Point b) {
            return new Point {
                x = a.x - b.x,
                y = a.y - b.y,
            };
        }

        public static Point operator /(Point a, int b) {
            return new Point {
                x = a.x / b,
                y = a.y / b,
            };
        }

        public static bool operator ==(Point a, Point b) {
            return a.x == b.x && a.y == b.y;
        }

        public static bool operator !=(Point a, Point b) {
            return a.x != b.x || a.y != b.y;
        }

        public void Deconstruct(out int x, out int y) {
            x = this.x;
            y = this.y;
        }

        public override bool Equals(object obj) {
            if (!(obj is Point)) {
                return false;
            }

            var point = (Point)obj;
            return x == point.x &&
                   y == point.y;
        }

        public override int GetHashCode() {
            var hashCode = 1502939027;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + x.GetHashCode();
            hashCode = hashCode * -1521134295 + y.GetHashCode();
            return hashCode;
        }
    }
}