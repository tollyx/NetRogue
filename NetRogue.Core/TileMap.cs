using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetRogue.Core {
    public class TileMap<T> : ICloneable, IEnumerable<T> {
        public readonly int Width;
        public readonly int Height;

        public T this[int x, int y] {
            get {
                return tiles[ToIndex(x, y)];
            }
            set {
                tiles[ToIndex(x, y)] = value;
            }
        }

        private T[] tiles;
        private T initial;

        private TileMap(int w, int h) {
            Width = w;
            Height = h;
        }

        public TileMap(int width, int height, T initial) {
            this.initial = initial;
            Width = width;
            Height = height;
            tiles = new T[width * height];

            if (!initial.Equals(default(T))) {
                for (int y = 0; y < height; y++) {
                    for (int x = 0; x < width; x++) {
                        this[x, y] = initial;
                    }
                }
            }
        }

        public int ToIndex(Point p) {
            return ToIndex(p.x, p.y);
        }

        public int ToIndex(int x, int y) {
            return x + y * Width;
        }

        public Point FromIndex(int i) {
            return new Point(i % Width, i / Width);
        }

        public bool IsInsideBounds(Point p) {
            return IsInsideBounds(p.x, p.y);
        }

        public bool IsInsideBounds(int x, int y) {
            return x >= 0 && x < Width && y >= 0 && y < Height;
        }

        public T GetTile(int x, int y) {
            if (IsInsideBounds(x, y)) {
                return this[x, y];
            }
            return initial;
        }

        public void SetTile(int x, int y, T tile) {
            if (IsInsideBounds(x, y)) {
                this[x, y] = tile;
            }
        }

        public void SetTile(Point p, T tile) {
            SetTile(p.x, p.y, tile);
        }

        public T GetTile(Point point) {
            return GetTile(point.x, point.y);
        }

        public IEnumerator<T> GetEnumerator() {
            return (IEnumerator<T>) tiles.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return tiles.GetEnumerator();
        }

        public TileMap<T> Clone() {
            return new TileMap<T>(Width, Height) {
                tiles = (T[])tiles.Clone(),
                initial = initial,
            };
        }

        object ICloneable.Clone() {
            return Clone();
        }
    }
}
