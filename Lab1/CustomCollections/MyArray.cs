﻿using System.Collections;

namespace CustomCollections
{
    public class MyArray<T> : IList<T> 
        where T : class, new()
    {
        #region PRIVATE FIELDS
        private int _defaultCapacity = 4;

        private int _count = 0;
        private int _capacity;
        private T[] _items;
        #endregion

        #region CONSTRUCTORS
        public MyArray()
        {
            _capacity = _defaultCapacity;
            _items = new T[_capacity];
        }

        public MyArray(int capacity)
        {
            if (capacity <= 0)
                throw new ArgumentOutOfRangeException("Capacity must be a positive value.");

            _capacity = _defaultCapacity = capacity;
            _items = new T[_capacity];
        }
        #endregion

        #region INTERFACE REALIZATION
        public int Count => _count;

        public bool IsReadOnly => false;

        public T this[int index] { get => _items[GetProperIndex(index)]; set => _items[GetProperIndex(index)] = value; }

        public void Add(T item)
        {
            if (_count == _capacity)
            {
                Resize();
            }

            _items[_count++] = item;
        }

        public void Clear()
        {
            _count = 0;
            _capacity = _defaultCapacity;

            _items = new T[_capacity];
        }

        public bool Contains(T item)
        {
            for (int i = 0; i < _count; i++)
            {
                if (_items[i].Equals(item))
                    return true;
            }

            return false;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            Array.ConstrainedCopy(_items, 0, array, arrayIndex, _count);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new CustomEnumerator<T>(this);
        }

        public bool Remove(T item)
        {
            int index = IndexOf(item);

            if (index >= 0)
            {
                RemoveAt(index);
                return true;
            }

            return false;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int IndexOf(T item)
        {
            for (int i = 0; i < _count; i++)
            {
                if (_items[i].Equals(item))
                    return i;
            }

            return -1;
        }

        public void Insert(int index, T item)
        {
            if (_count == _capacity)
            {
                Resize();
            }

            MovePartOfArray(GetProperIndex(index));

            _items[GetProperIndex(index)] = item;

            _count++;
        }

        public void RemoveAt(int index)
        {
            MovePartOfArray(GetProperIndex(index) + 1, moveBack: true);

            _count--;
        }
        #endregion

        #region PRIVATE METHODS
        private int GetProperIndex(int index)
            => index >= 0 ? index % _count : _count + index % _count;

        private void Resize()
        {
            _capacity *= 2;

            T[] tempArray = new T[_capacity];

            Array.Copy(_items, tempArray, _count);

            _items = tempArray;
        }

        private void MovePartOfArray(int index, bool moveBack = false)
        {
            if(index == 0 || index >= _count)
            {
                return;
            }

            Array.Copy(_items, index, _items, moveBack ? index - 1 : index + 1, _count - index);
        }
        #endregion
    }
}
