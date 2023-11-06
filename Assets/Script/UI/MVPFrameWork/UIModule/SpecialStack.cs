using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MVPFrameWork
{
    public class SpecialStack<T>
    {
        private List<T> _list = new List<T>();

        public T this[int index]
        {
            get
            {
                return _list.GetValueAnyway(index);
            }
            set
            {
                _list.TrySetValue(index, value);
            }
        }

        public int Count => _list.Count;

        public void Push(T item)
        {
            _list.Remove(item);
            _list.Add(item);
        }

        public bool Pop(out T item)
        {
            bool result = false;
            if(_list.Count > 0)
            {
                item = _list[_list.Count - 1];
                _list.RemoveAt(_list.Count - 1);
                result = true;
            }
            else
            {
                item = default(T);
            }

            return result;
        }

        public bool Peek(out T item)
        {
            bool result = false;
            if(_list.Count > 0)
            {
                item = _list[_list.Count - 1];
                result = true;
            }
            else
            {
                item = default(T);
            }

            return result;
        }

        public void Delete(T item)
        {
            _list.Remove(item);
        }

        public bool Contains(T item)
        {
            return _list.Contains(item);
        }

        public void Clear()
        {
            _list.Clear();
        }
    }
}