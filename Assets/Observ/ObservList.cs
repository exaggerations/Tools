using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ObservList<T> : IList<T>
{
    private readonly IList<T> _list;
    public event Action<IList<T>> OnListChanged;

    public ObservList(IList<T> initialList = null)
    {
        _list = initialList ?? new List<T>();
    }

    public T this[int index] { get => _list[index]; set { _list[index] = value; Invoke(); } }


    public void Invoke() => OnListChanged?.Invoke(_list);
    public int Count => _list.Count;

    public bool IsReadOnly => _list.IsReadOnly;

    public void Add(T item)
    {
        _list.Add(item);
        Invoke();
    }

    public void Clear()
    {
       _list.Clear();
        Invoke();
    }

    public bool Contains(T item)
    {
       return _list.Contains(item);
    }

    public void CopyTo(T[] array, int arrayIndex)
    {
        throw new NotImplementedException();
    }

    public IEnumerator<T> GetEnumerator()
    {
        return _list.GetEnumerator();
    }

    public int IndexOf(T item)
    {
        return _list.IndexOf(item);
    }

    public void Insert(int index, T item)
    {
        _list.Insert(index, item);
        Invoke();
    }

    public bool Remove(T item)
    {
       return _list.Remove(item);
    }

    public void RemoveAt(int index)
    {
       _list.RemoveAt(index);
        Invoke();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    // Start is called before the first frame update
   
}
