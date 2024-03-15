using System.Collections;

namespace OnePassword.Common;

/// <summary>
/// Represents a strongly typed change tracking enabled list of objects that can be accessed by index. Provides methods to search, sort, and manipulate lists.
/// </summary>
/// <typeparam name="T">The type of elements in the list.</typeparam>
[Serializable]
public sealed class TrackedList<T> : IList<T>, IList, IReadOnlyList<T>, ITracked
{
    /// <summary>
    /// Gets the number of elements contained in the <see cref="TrackedList{T}"/>.
    /// </summary>
    public int Count => _list.Count;
    
    /// <inheritdoc />
    public bool IsFixedSize => ((IList)_list).IsFixedSize;
    
    /// <inheritdoc cref="IList.IsReadOnly" />
    public bool IsReadOnly => ((IList)_list).IsReadOnly;
    
    /// <inheritdoc />
    public bool IsSynchronized => ((IList)_list).IsSynchronized;
    
    /// <inheritdoc />
    public object SyncRoot => ((IList)_list).SyncRoot;
    
    /// <inheritdoc />
    bool ITracked.Changed => _changed || _list.Exists(item => item is ITracked { Changed: true });
    
    /// <summary>
    /// Gets all items that where removed from the list.
    /// </summary>
    internal IEnumerable<T> Removed => _initialList.Except(_list);

    private readonly List<T> _list;
    private List<T> _initialList;
    private bool _changed;

    /// <summary>
    /// Initializes a new instance of <see cref="TrackedList{T}"/>.
    /// </summary>
    public TrackedList()
    {
        _list = [];
        _initialList = new List<T>(_list);
    }

    /// <summary>
    /// Initializes a new instance of <see cref="TrackedList{T}"/> that is empty and has the specified initial capacity.
    /// </summary>
    /// <param name="capacity">The initial list capacity.</param>
    public TrackedList(int capacity)
    {
        _list = new List<T>(capacity);
        _initialList = new List<T>(_list);
    }

    /// <summary>
    /// Initializes a new instance of <see cref="TrackedList{T}"/> that contains elements copied from the specified collection and has sufficient capacity to accommodate the number of elements copied.
    /// </summary>
    /// <param name="collection">The collection to copy to the list.</param>
    public TrackedList(IEnumerable<T> collection)
    {
        _list = new List<T>(collection);
        _initialList = new List<T>(_list);
    }

    /// <inheritdoc />
    object? IList.this[int index]
    {
        get => ((IList)_list)[index];
        set
        {
            ((IList)_list)[index] = value;
            _changed = true;
        }
    }

    /// <inheritdoc cref="IList{T}.this[int]" />
    public T this[int index]
    {
        get => _list[index];
        set
        {
            _list[index] = value;
            _changed = true;
        }
    }

    /// <inheritdoc />
    void ITracked.AcceptChanges()
    {
        _changed = false;
        _initialList = new List<T>(_list);
    }

    /// <inheritdoc />
    public int Add(object? value)
    {
        var index = ((IList)_list).Add(value);
        _changed = true;
        return index;
    }

    /// <inheritdoc />
    public void Add(T item)
    {
        _list.Add(item);
        _changed = true;
    }

    /// <inheritdoc />
    void IList.Clear()
    {
        _list.Clear();
        _changed = true;
    }

    /// <inheritdoc />
    void ICollection<T>.Clear()
    {
        _list.Clear();
        _changed = true;
    }

    /// <inheritdoc />
    public bool Contains(object? value)
    {
        return ((IList)_list).Contains(value);
    }

    /// <inheritdoc />
    public bool Contains(T item)
    {
        return _list.Contains(item);
    }

    /// <inheritdoc />
    public void CopyTo(Array array, int index)
    {
        ((IList)_list).CopyTo(array, index);
    }

    /// <inheritdoc />
    public void CopyTo(T[] array, int arrayIndex)
    {
        _list.CopyTo(array, arrayIndex);
    }

    /// <inheritdoc />
    IEnumerator IEnumerable.GetEnumerator()
    {
        return _list.GetEnumerator();
    }

    /// <inheritdoc />
    public IEnumerator<T> GetEnumerator()
    {
        return _list.GetEnumerator();
    }

    /// <inheritdoc />
    public int IndexOf(object? value)
    {
        return ((IList)_list).IndexOf(value);
    }

    /// <inheritdoc />
    public int IndexOf(T item)
    {
        return _list.IndexOf(item);
    }

    /// <inheritdoc />
    public void Insert(int index, object? value)
    {
        ((IList)_list).Insert(index, value);
        _changed = true;
    }

    /// <inheritdoc />
    public void Insert(int index, T item)
    {
        _list.Insert(index, item);
        _changed = true;
    }

    /// <inheritdoc />
    public void Remove(object? value)
    {
        ((IList)_list).Remove(value);
        _changed = true;
    }

    /// <inheritdoc />
    public bool Remove(T item)
    {
        var removed = _list.Remove(item);
        _changed = true;
        return removed;
    }

    /// <inheritdoc />
    void IList.RemoveAt(int index)
    {
        _list.RemoveAt(index);
        _changed = true;
    }

    /// <inheritdoc />
    void IList<T>.RemoveAt(int index)
    {
        _list.RemoveAt(index);
        _changed = true;
    }
}