using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetRogue.Core;
using NetRogue.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetRogue.Core.Tests {
    [TestClass()]
    public class QuadTreeTests {

        [TestMethod()]
        public void AddTest() {
            Rect bounds = new Rect(100, 100);
            QuadTree<Actor> actors = new QuadTree<Actor>(bounds, 2, 3);

            Assert.IsTrue(actors.Add(new Goblin(0, 0)));
            Assert.IsTrue(actors.Add(new Goblin(99, 99)));

            Assert.IsFalse(actors.Add(new Goblin(-12, 0)));
            Assert.IsFalse(actors.Add(new Goblin(0, 100)));
        }

        [TestMethod()]
        public void RemoveTest() {
            Rect bounds = new Rect(8, 8);
            QuadTree<Actor> tree = new QuadTree<Actor>(bounds, 2, 3);
            List<Actor> actors = new List<Actor> {
                new Goblin(0, 0),
                new Goblin(1, 1),
                new Goblin(2, 2),
                new Goblin(5, 5),
                new Goblin(6, 6),
                new Goblin(7, 7)
            };

            foreach (var item in actors) {
                if (!tree.Add(item)) {
                    Assert.Inconclusive();
                }
            }

            Assert.IsTrue(tree.Remove(actors[5]));
            Assert.IsTrue(tree.Remove(actors[2]));
            Assert.AreEqual(actors.Count - 2, tree.Count);
        }

        [TestMethod()]
        public void QueryAreaTest() {
            Assert.Inconclusive();
        }

        [TestMethod()]
        public void GetAtTest() {
            Rect bounds = new Rect(16, 16);
            QuadTree<Actor> tree = new QuadTree<Actor>(bounds, 2, 3);
            List<Actor> actors = new List<Actor> {
                new Goblin(4, 0),
                new Goblin(3, 5),
                new Goblin(5, 15),
            };

            foreach (var item in actors) {
                if (!tree.Add(item)) {
                    Assert.Inconclusive();
                }
            }

            foreach (var item in actors) {
                Assert.AreEqual(item, tree.GetAt(item.Position), $"Could not get actor at position {item.Position}");
            }
        }

        [TestMethod()]
        public void GetMultipleAtTest() {
            Assert.Inconclusive();
        }

        [TestMethod()]
        public void CleanupTest() {
            Assert.Inconclusive();
        }

        [TestMethod()]
        public void GetEnumeratorTest() {
            Rect bounds = new Rect(8, 8);
            QuadTree<Actor> tree = new QuadTree<Actor>(bounds, 2, 3);
            List<Actor> actors = new List<Actor> {
                new Goblin(0, 0),
                new Goblin(1, 1),
                new Goblin(2, 5),
                new Goblin(3, 4),
            };

            foreach (var item in actors) {
                if (!tree.Add(item)) {
                    Assert.Inconclusive();
                }
            }
            var fromtree = tree.ToList(); // ...and back to a list again via the enumerator

            Assert.AreEqual(actors.Count, fromtree.Count);
        }

        [TestMethod()]
        public void GetCurrentDepthTest() {
            Rect bounds = new Rect(128, 128);
            QuadTree<Actor> tree = new QuadTree<Actor>(bounds, 2, 7);
            List<Actor> actors = new List<Actor> {
                new Goblin(0, 0),
                new Goblin(0, 127),
            };

            // A tree without children == 0 depth
            Assert.AreEqual(0, tree.GetCurrentDepth());

            foreach (var item in actors) {
                if (!tree.Add(item)) {
                    Assert.Inconclusive();
                }
            }

            Assert.AreEqual(0, tree.GetCurrentDepth());

            if (!tree.Add(new Goblin(16, 16))) {
                Assert.Inconclusive();
            }

            Assert.AreEqual(1, tree.GetCurrentDepth());
        }
    }
}