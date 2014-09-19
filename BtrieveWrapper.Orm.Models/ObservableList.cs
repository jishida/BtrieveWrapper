using System;
using System.Collections.Generic;
using System.Text;

namespace BtrieveWrapper.Orm.Models
{
    public class ObservableList<T> : IList<T>
    {
        List<T> _list;

        public ObservableList() : base() {
            _list = new List<T>();
        }

        public T this[int index] {
            get { return _list[index]; }
            set { _list[index] = value; }
        }

        public int Count { get { return _list.Count; } }
        public bool IsReadOnly { get { return ((ICollection<T>)_list).IsReadOnly; } }

        public event Action<object, T> OnAdded;

        public void Add(T item) {
            _list.Add(item);
            if (this.OnAdded != null) {
                this.OnAdded(this, item);
            }
        }

        public void Clear() {
            _list.Clear();
        }

        public bool Contains(T item) {
            return _list.Contains(item);
        }

        public void CopyTo(T[] array) {
            _list.CopyTo(array);
        }

        public void CopyTo(T[] array, int arrayIndex) {
            _list.CopyTo(array, arrayIndex);
        }

        public void CopyTo(int index, T[] array, int arrayIndex, int count) {
            _list.CopyTo(index, array, arrayIndex, count);
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return ((System.Collections.IEnumerable)_list).GetEnumerator();
        }

        public IEnumerator<T> GetEnumerator() {
            return _list.GetEnumerator();
        }

        public int IndexOf(T item) {
            return _list.IndexOf(item);
        }

        public void Insert(int index, T item) {
            _list.Insert(index, item);
        }

        public bool Remove(T item) {
            return _list.Remove(item);
        }

        public void RemoveAt(int index) {
            _list.RemoveAt(index);
        }

    }
}
