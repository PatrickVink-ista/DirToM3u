using System.Collections;

namespace Fringilla.Media;

/// <summary>
/// 
/// </summary>
/// <typeparam name="T"></typeparam>
public class Playlist<T> : IList<T> where T : PlaylistEntry
{
    private readonly List<T> _entries = [];
    /// <inheritdoc/>
    public T this[int index] { get => ((IList<T>)_entries)[index]; set => ((IList<T>)_entries)[index] = value; }
    /// <inheritdoc/>
    public int Count => ((ICollection<T>)_entries).Count;
    /// <inheritdoc/>
    public bool IsReadOnly => ((ICollection<T>)_entries).IsReadOnly;
    /// <inheritdoc/>
    public void Add(T item) => ((ICollection<T>)_entries).Add(item);
    /// <inheritdoc/>
    public void Clear() => ((ICollection<T>)_entries).Clear();
    /// <inheritdoc/>
    public bool Contains(T item) => ((ICollection<T>)_entries).Contains(item);
    /// <inheritdoc/>
    public void CopyTo(T[] array, int arrayIndex) => ((ICollection<T>)_entries).CopyTo(array, arrayIndex);
    /// <inheritdoc/>
    public IEnumerator<T> GetEnumerator() => ((IEnumerable<T>)_entries).GetEnumerator();
    /// <inheritdoc/>
    public int IndexOf(T item) => ((IList<T>)_entries).IndexOf(item);
    /// <inheritdoc/>
    public void Insert(int index, T item) => ((IList<T>)_entries).Insert(index, item);
    /// <inheritdoc/>
    public bool Remove(T item) => ((ICollection<T>)_entries).Remove(item);
    /// <inheritdoc/>
    public void RemoveAt(int index) => ((IList<T>)_entries).RemoveAt(index);
    /// <inheritdoc/>
    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_entries).GetEnumerator();
}
