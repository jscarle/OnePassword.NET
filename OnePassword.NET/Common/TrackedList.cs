using System.Collections;

namespace OnePassword.Common;

[Serializable]
public sealed class TrackedList<T> : IList<T>, IList, IReadOnlyList<T>, ITracked
{
    public int Count => _list.Count;
    public bool IsFixedSize => ((IList)_list).IsFixedSize;
    public bool IsReadOnly => ((IList)_list).IsReadOnly;
    public bool IsSynchronized => ((IList)_list).IsSynchronized;
    public object SyncRoot => ((IList)_list).SyncRoot;
    bool ITracked.Changed => _changed | _list.Any(item => item is ITracked { Changed: true });
    internal IEnumerable<T> Removed => _initialList.Except(_list);

    private readonly List<T> _list;
    private List<T> _initialList;
    private bool _changed;

    public TrackedList()
    {
        _list = new List<T>();
        _initialList = new List<T>(_list);
    }

    public TrackedList(int capacity)
    {
        _list = new List<T>(capacity);
        _initialList = new List<T>(_list);
    }

    public TrackedList(IEnumerable<T> collection)
    {
        _list = new List<T>(collection);
        _initialList = new List<T>(_list);
    }

    object? IList.this[int index]
    {
        get => ((IList)_list)[index];
        set
        {
            ((IList)_list)[index] = value;
            _changed = true;
        }
    }

    public T this[int index]
    {
        get => _list[index];
        set
        {
            _list[index] = value;
            _changed = true;
        }
    }

    void ITracked.AcceptChanges()
    {
        _changed = false;
        _initialList = new List<T>(_list);
    }

    public int Add(object? value)
    {
        var index = ((IList)_list).Add(value);
        _changed = true;
        return index;
    }

    public void Add(T item)
    {
        _list.Add(item);
        _changed = true;
    }

    void IList.Clear()
    {
        _list.Clear();
        _changed = true;
    }

    void ICollection<T>.Clear()
    {
        _list.Clear();
        _changed = true;
    }

    public bool Contains(object? value)
    {
        return ((IList)_list).Contains(value);
    }

    public bool Contains(T item)
    {
        return _list.Contains(item);
    }

    public void CopyTo(Array array, int index)
    {
        ((IList)_list).CopyTo(array, index);
    }

    public void CopyTo(T[] array, int arrayIndex)
    {
        _list.CopyTo(array, arrayIndex);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return _list.GetEnumerator();
    }

    public IEnumerator<T> GetEnumerator()
    {
        return _list.GetEnumerator();
    }

    public int IndexOf(object? value)
    {
        return ((IList)_list).IndexOf(value);
    }

    public int IndexOf(T item)
    {
        return _list.IndexOf(item);
    }

    public void Insert(int index, object? value)
    {
        ((IList)_list).Insert(index, value);
        _changed = true;
    }

    public void Insert(int index, T item)
    {
        _list.Insert(index, item);
        _changed = true;
    }

    public void Remove(object? value)
    {
        ((IList)_list).Remove(value);
        _changed = true;
    }

    public bool Remove(T item)
    {
        var removed = _list.Remove(item);
        _changed = true;
        return removed;
    }

    void IList.RemoveAt(int index)
    {
        _list.RemoveAt(index);
        _changed = true;
    }

    void IList<T>.RemoveAt(int index)
    {
        _list.RemoveAt(index);
        _changed = true;
    }
}