using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetRogue.Core {
    public class QuadTree<T> : IEnumerable<T> where T: class, IEntity {
        readonly Rect bounds;
        QuadTree<T>[] children;
        List<T> entities;
        int depth;
        int maxDepth;
        int maxEntities;

        public QuadTree(Rect bounds, int maxEntities = 10, int maxDepth = 3, int atDepth = 0) {
            this.bounds = bounds;
            entities = new List<T>();
            this.maxDepth = maxDepth;
            this.maxEntities = maxEntities;
            depth = atDepth;
        }

        public bool Add(T entity) {
            if(bounds.Contains(entity.Position)) {
                if (children == null) {
                    entities.Add(entity);
                    if (depth != maxDepth && entities.Count >= maxEntities) {
                        Divide();
                    }
                    return true;
                }
                else {
                    foreach (var item in children) {
                        if (item.Add(entity)) {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public bool Remove(T entity) {
            if (bounds.Contains(entity.Position)) {
                if (children == null) {
                    return entities.Remove(entity);
                }
                else {
                    foreach (var item in children) {
                        if (item.Remove(entity)) {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        void Divide() {
            if (children != null) return;
            if (maxDepth == depth) return;
            int childW = bounds.w / 2;
            int childH = bounds.h / 2;

            children = new QuadTree<T>[4] {
                new QuadTree<T>(new Rect(bounds.Left, bounds.Top, bounds.w / 2, bounds.h / 2), maxDepth, depth+1),
                new QuadTree<T>(new Rect(bounds.Left + bounds.w / 2, bounds.Top, bounds.w / 2, bounds.h / 2), maxDepth, depth+1),
                new QuadTree<T>(new Rect(bounds.Left, bounds.Top + bounds.h / 2, bounds.w / 2, bounds.h / 2), maxDepth, depth+1),
                new QuadTree<T>(new Rect(bounds.Left + bounds.w / 2, bounds.Top + bounds.h / 2, bounds.w / 2, bounds.h / 2), maxDepth, depth+1),
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
                    return null;
                }
            }
            return null;
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
                    e.Concat(item);
                }
            }
            return e.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }
}
