using System;

namespace NetRogue.Core {
    public struct Point : IEquatable<Point> {
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

        internal Direction Direction() {
            if (Math.Abs(x) > Math.Abs(y)) {
                if (x == 0) {
                    return Core.Direction.None;
                }
                else return x < 0 ? Core.Direction.Left : Core.Direction.Right;
            }
            else {
                if (y == 0) {
                    return Core.Direction.None;
                }
                else return y < 0 ? Core.Direction.Up : Core.Direction.Down;
            }
        }

        public static bool operator ==(Point a, Point b) {
            return a.Equals(b);
        }

        internal static float Distance(Point start, Point end) {
            return (float)Math.Sqrt(DistanceSquared(start, end));
        }

        public static float DistanceSquared(Point start, Point end) {
            float dx = start.x - end.x;
            float dy = start.y - end.y;
            return dx * dx + dy * dy;
        }

        public static bool operator !=(Point a, Point b) {
            return !a.Equals(b);
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
            return x == point.x && y == point.y;
        }

        public override int GetHashCode() {
            var hashCode = 1502939027;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + x.GetHashCode();
            hashCode = hashCode * -1521134295 + y.GetHashCode();
            return hashCode;
        }

        public bool Equals(Point other) {
            return x == other.x && y == other.y;
        }
    }
}