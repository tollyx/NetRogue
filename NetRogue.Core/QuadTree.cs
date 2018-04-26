using NetRogue.Core.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetRogue.Core {
    public class QuadTree<T> : ICollection<T>, IEnumerable<T> where T : class, IEntity {
        readonly Rect bounds;
        readonly int depth;
        readonly int maxDepth;
        readonly int maxEntities;

        QuadTree<T>[] children;
        List<T> entities;

        public int Count {
            get {
                if (children == null) {
                    return entities.Count;
                }
                return children.Select(c => c.Count).Sum();
            }
        }

        public bool IsReadOnly => false;

        public QuadTree(Rect bounds, int maxEntities = 10, int maxDepth = 3) : this(bounds, maxEntities, maxDepth, 0) {}

        private QuadTree(Rect bounds, int maxEntities, int maxDepth, int atDepth) {
            this.bounds = bounds;
            this.maxDepth = maxDepth;
            this.maxEntities = maxEntities;
            depth = atDepth;
            entities = new List<T>();
            children = null;
        }

        public bool Add(T entity) {
            if (!bounds.Contains(entity.Position)) return false;

            if (children == null) {
                entities.Add(entity);
                if (depth < maxDepth && entities.Count > maxEntities) {
                    Divide();
                }
                return true;
            }

            foreach (var item in children) {
                if (item.Add(entity)) {
                    return true;
                }
            }
            return false;
        }

        public bool Remove(T entity) {
            if (!bounds.Contains(entity.Position)) return false;

            if (children == null) {
                return entities.Remove(entity);
            }

            foreach (var item in children) {
                if (item.Remove(entity)) {
                    return true;
                }
            }
            return false;
        }

        void Divide() {
            if (children != null) return; // Don't divide if we've already divided
            if (maxDepth <= depth) return;
            if (bounds.w <= 1 || bounds.h <= 1) return; // We're small enough already

            // Using ceil, since we'd rather have overlapping children
            // than be missing entities due to rounding errors
            int childW = (int)Math.Ceiling(bounds.w / 2.0);
            int childH = (int)Math.Ceiling(bounds.h / 2.0);

            children = new QuadTree<T>[] {
                new QuadTree<T>(new Rect(bounds.Left, bounds.Top, childW, childH), maxEntities, maxDepth, depth+1),
                new QuadTree<T>(new Rect(bounds.Left + childW, bounds.Top, childW, childH), maxEntities, maxDepth, depth+1),
                new QuadTree<T>(new Rect(bounds.Left, bounds.Top + childH, childW, childH), maxEntities, maxDepth, depth+1),
                new QuadTree<T>(new Rect(bounds.Left + childW, bounds.Top + childH, childW, childH), maxEntities, maxDepth, depth+1),
            };

            foreach (var item in entities) {
                Add(item);
            }
            entities = null;
        }

        public IEnumerable<T> QueryArea(Rect area) {
            if (!bounds.Intersects(area)) yield break;

            if (children == null) {
                foreach (var item in entities.Where(e => area.Contains(e.Position))) {
                    yield return item;
                }
                yield break;
            }

            foreach (var tree in children) {
                foreach (var creature in tree) {
                    yield return creature;
                }
            }
        }

        public T GetAt(Point pos) {
            if (!bounds.Contains(pos)) return null;

            if (children == null) {
                return entities.FirstOrDefault(e => e.Position == pos);
            }

            foreach (var item in children) {
                var e = item.GetAt(pos);
                if (e != null) {
                    return e;
                }
            }
            return null;
        }

        public IEnumerable<T> GetMultipleAt(Point pos) {
            if (!bounds.Contains(pos)) yield break;

            if (children == null) {
                foreach (var item in entities.Where(e => e.Position == pos)) {
                    yield return item;
                }
                yield break;
            }

            foreach (var item in children) {
                foreach (var e in item.GetMultipleAt(pos)) {
                    yield return e;
                }
            }
        }

        public bool Cleanup() {
            if (children == null) {
                return !entities.Any();
            }

            bool isClean = true;
            foreach (var item in children) {
                isClean = item.Cleanup() && isClean;
            }

            if (isClean) {
                children = null;
                entities = new List<T>();
            }
            return isClean;
        }

        public int GetCurrentDepth() {
            if (children == null) {
                return depth;
            }
            return children.Select(c => c.GetCurrentDepth()).Max();
        }

        public IEnumerator<T> GetEnumerator() {
            if (children == null) {
                return entities.GetEnumerator();
            }

            IEnumerable<T> e = null;
            foreach (var item in children) {
                if (e == null) {
                    e = item;
                }
                else {
                    e = e.Concat(item);
                }
            }
            return e.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        void ICollection<T>.Add(T item) {
            Add(item);
        }

        public void Clear() {
            children = null;
            if (entities != null) {
                entities.Clear();
            }
            else {
                entities = new List<T>();
            }
        }

        public bool Contains(T item) {
            if (children == null) {
                return entities.Contains(item);
            }

            foreach (var child in children) {
                if (child.Contains(item)) {
                    return true;
                }
            }
            return false;
        }

        public void CopyTo(T[] array, int arrayIndex) {
            var e = GetEnumerator();
            for (int i = arrayIndex; i < array.Length; i++) {
                if (!e.MoveNext()) {
                    break;
                }
                array[i] = e.Current;
            }
        }
    }
}
