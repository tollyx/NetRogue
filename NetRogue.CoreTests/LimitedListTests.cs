using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetRogue.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetRogue.Core.Tests {
    [TestClass()]
    public class LimitedListTests {
        [TestMethod()]
        public void LimitedList_Create_Test() {
            // Arrange
            int capacity = 4;

            // Act
            LimitedList<object> list = new LimitedList<object>(capacity);

            // Assert
            Assert.AreEqual(capacity, list.Capacity);
        }

        [TestMethod()]
        public void Add_When_Not_Full_Test() {
            // Arrange
            int capacity = 1;
            LimitedList<object> list = new LimitedList<object>(capacity);

            // Act
            bool added = list.Add(new object());

            // Assert
            Assert.IsTrue(added);
            Assert.AreEqual(1, list.Count);
        }

        [TestMethod()]
        public void Add_When_Full_Test() {
            // Arrange
            int capacity = 0;
            LimitedList<object> list = new LimitedList<object>(capacity);

            // Act
            bool added = list.Add(new object());

            // Assert
            Assert.IsFalse(added);
            Assert.AreEqual(capacity, list.Count);
        }

        [TestMethod()]
        public void ClearTest() {
            // Arrange
            int capacity = 2;
            LimitedList<object> list = new LimitedList<object>(capacity) {
                new object(),
                new object()
            };

            // Act
            list.Clear();

            // Assert
            Assert.AreEqual(0, list.Count);
        }

        [TestMethod()]
        public void Contains_True_Test() {
            // Arrange
            int capacity = 1;
            LimitedList<object> list = new LimitedList<object>(capacity);
            var obj = new object();
            list.Add(obj);


            // Act
            bool contains = list.Contains(obj);

            // Assert
            Assert.IsTrue(contains);
        }

        [TestMethod()]
        public void Contains_False_Test() {
            // Arrange
            int capacity = 1;
            LimitedList<object> list = new LimitedList<object>(capacity);
            var obj = new object();

            // Act
            bool contains = list.Contains(obj);

            // Assert
            Assert.IsFalse(contains);
        }

        [TestMethod()]
        public void CopyToTest() {
            // Arrange
            int capacity = 3;
            LimitedList<object> list = new LimitedList<object>(capacity) {
                new object(),
                new object(),
                new object(),
            };
            object[] arr = new object[3];


            // Act
            list.CopyTo(arr, 0);

            // Assert
            for (int i = 0; i < 3; i++) {
                Assert.AreEqual(list[i], arr[i]);
            }
        }

        [TestMethod()]
        public void IsFull_True_Test() {
            // Arrange
            LimitedList<object> list = new LimitedList<object>(1);

            // Act
            list.Add(new object());

            // Assert
            Assert.IsTrue(list.IsFull);
        }

        [TestMethod()]
        public void IsFull_False_Test() {
            // Arrange
            LimitedList<object> list = new LimitedList<object>(2);

            // Act
            list.Add(new object());

            // Assert
            Assert.IsFalse(list.IsFull);
        }

        [TestMethod()]
        public void GetEnumeratorTest() {
            // Arrange
            LimitedList<object> list = new LimitedList<object>(3);
            object[] objs = new object[] {
                new object(), new object(), new object(),
            };
            foreach (var item in objs) {
                list.Add(item);
            }

            // Act
            IEnumerator<object> enumerator = list.GetEnumerator();

            // Assert
            Assert.IsTrue(enumerator.MoveNext());
            Assert.AreEqual(objs[0], enumerator.Current);
            Assert.IsTrue(enumerator.MoveNext());
            Assert.AreEqual(objs[1], enumerator.Current);
            Assert.IsTrue(enumerator.MoveNext());
            Assert.AreEqual(objs[2], enumerator.Current);
            Assert.IsFalse(enumerator.MoveNext());
        }

        [TestMethod()]
        public void IndexOfTest() {
            // Arrange
            LimitedList<object> list = new LimitedList<object>(2);
            var obj1 = new Object();
            var obj2 = new Object();
            list.Add(obj1);
            list.Add(obj2);

            // Act
            int i1 = list.IndexOf(obj1);
            int i2 = list.IndexOf(obj2);

            // Assert
            Assert.AreEqual(0, i1);
            Assert.AreEqual(1, i2);
        }

        [TestMethod()]
        public void Insert_Success_Test() {
            // Arrange
            int capacity = 2;
            LimitedList<object> list = new LimitedList<object>(capacity);
            var obj = new object();
            list.Add(new object());

            // Act
            bool inserted = list.Insert(0, obj);

            // Assert
            Assert.IsTrue(inserted);
            Assert.AreEqual(obj, list[0]);
            Assert.AreEqual(2, list.Count);
        }

        [TestMethod()]
        public void Insert_When_Full_Test() {
            // Arrange
            int capacity = 1;
            LimitedList<object> list = new LimitedList<object>(capacity);
            var obj = new object();
            list.Add(new object());

            // Act
            bool inserted = list.Insert(0, obj);

            // Assert
            Assert.IsFalse(inserted);
            Assert.AreNotEqual(obj, list[0]);
            Assert.AreEqual(1, list.Count);
        }

        [TestMethod()]
        public void Remove_Success_Test() {
            // Arrange
            int capacity = 1;
            LimitedList<object> list = new LimitedList<object>(capacity);
            var obj = new object();
            list.Add(obj);

            // Act
            bool removed = list.Remove(obj);

            // Assert
            Assert.IsTrue(removed);
            Assert.IsFalse(list.Contains(obj));
        }

        [TestMethod()]
        public void Remove_Failed_Test() {
            // Arrange
            int capacity = 1;
            LimitedList<object> list = new LimitedList<object>(capacity);
            var obj1 = new object();
            var obj2 = new object();
            list.Add(obj2);

            // Act
            bool removed = list.Remove(obj1);

            // Assert
            Assert.IsFalse(removed);
            Assert.IsTrue(list.Contains(obj2));
        }

        [TestMethod()]
        public void RemoveAtTest() {
            // Arrange
            int capacity = 3;
            LimitedList<object> list = new LimitedList<object>(capacity);
            var obj1 = new object();
            var obj2 = new object();
            var obj3 = new object();
            list.Add(obj1);
            list.Add(obj2);
            list.Add(obj3);

            // Act
            list.RemoveAt(1);

            // Assert
            Assert.AreEqual(2, list.Count);
            Assert.AreEqual(obj3, list[1]);
        }
    }
}