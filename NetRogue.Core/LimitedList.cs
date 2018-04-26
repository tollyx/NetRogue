using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetRogue.Core {
    public class LimitedList<T> : IList<T> {

        List<T> list;

        public LimitedList(int capacity) {
            Capacity = capacity;
            list = new List<T>(capacity);
        }

        public T this[int index] { get => list[index]; set => list[index] = value; }

        public int Count => list.Count;

        public bool IsReadOnly => false;

        public int Capacity { get; }

        public bool IsFull => Count >= Capacity;

        public bool Add(T item) {
            if (!IsFull) {
                list.Add(item);
                return true;
            }
            return false;
        }

        void ICollection<T>.Add(T item) {
            Add(item);
        }

        public void Clear() {
            list.Clear();
        }

        public bool Contains(T item) {
            return list.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex) {
            list.CopyTo(array, arrayIndex);
        }

        public IEnumerator<T> GetEnumerator() {
            return list.GetEnumerator();
        }

        public int IndexOf(T item) {
            return list.IndexOf(item);
        }

        public bool Insert(int index, T item) {
            if (!IsFull) {
                list.Insert(index, item);
                return true;
            }
            return false;
        }

        void IList<T>.Insert(int index, T item) {
            Insert(index, item);
        }

        public bool Remove(T item) {
            return list.Remove(item);
        }

        public void RemoveAt(int index) {
            list.RemoveAt(index);
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }
}
