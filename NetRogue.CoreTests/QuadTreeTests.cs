using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetRogue.Core;
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

            Assert.IsTrue(actors.Add(new Goblin(1, 1)));
            Assert.IsTrue(actors.Add(new Goblin(2, 2)));
            Assert.IsTrue(actors.Add(new Goblin(97, 97)));
            Assert.IsTrue(actors.Add(new Goblin(98, 98)));

            Assert.IsFalse(actors.Add(new Goblin(102, 0)));
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
                tree.Add(item);
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
            Assert.Inconclusive();
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
                new Goblin(2, 2),
                new Goblin(3, 3),
                new Goblin(0, 7),
                new Goblin(1, 6),
                new Goblin(2, 5),
                new Goblin(3, 4),
            };

            foreach (var item in actors) {
                tree.Add(item);
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
                tree.Add(item);
            }

            Assert.AreEqual(0, tree.GetCurrentDepth());

            tree.Add(new Goblin(16, 16));

            Assert.AreEqual(1, tree.GetCurrentDepth());
        }
    }
}