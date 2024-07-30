using System.Collections;
using System.Text.RegularExpressions;

namespace Fringilla.Media;

/// <summary>
/// 
/// </summary>
/// <typeparam name="T"></typeparam>
public partial class Playlist<T> : IList<T> where T : PlaylistEntry, new()
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="PlaylistEntryType"></typeparam>
    /// <param name="path"></param>
    /// <returns></returns>
    public static Playlist<PlaylistEntryType> CreateFromDirectory<PlaylistEntryType>(string path) where PlaylistEntryType : PlaylistEntry, new()
    {
        Playlist<PlaylistEntryType> result = new();
        result.ReadFromDirectory(path);
        return result;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="path"></param>
    /// <param name="searchOption"></param>
    public void ReadFromDirectory(string path, SearchOption searchOption = SearchOption.AllDirectories)
    {
        Clear();
        var files = Sort(
            Filter(
                Directory.GetFiles(path, "*.*", searchOption)
            ));
        foreach (var file in files)
        {
            ExtendedInfo info = GetExtendedInfo(file);
            T item = new() { Duration = info.duration, Title = info.title, Path = file };
            Add(item);
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="files"></param>
    /// <returns></returns>
    protected virtual IEnumerable<string> Filter(IEnumerable<string> files) => files
            .Where(HasValidExtension);
    /// <summary>
    /// 
    /// </summary>
    /// <param name="files"></param>
    /// <returns></returns>
    protected virtual IEnumerable<string> Sort(IEnumerable<string> files)
    {
        IEnumerable<(Match, string f)> matches = files
            .Select(f => (NumericExtract().Match(Path.GetFileName(f)), f));
        if (matches.Count() != files.Count())
        {
            return files;
        }
        IEnumerable<(int, int, string)> keyed = matches
            .Select(x => (
                int.Parse(x.Item1.Groups.Values.ToList()[1].Value),
                int.Parse(x.Item1.Groups.Values.ToList()[2].Value),
                x.Item2));
        IOrderedEnumerable<(int, int, string)> indexed = keyed
            .OrderBy(x => x.Item1).ThenBy(x => x.Item2);
        IEnumerable<string> sorted = indexed
            .Select(x => x.Item3);
        return sorted;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    protected virtual ExtendedInfo GetExtendedInfo(string path) => PlatformGetExtendedInfo(path);
    /// <summary>
    /// 
    /// </summary>
    public Func<string, ExtendedInfo> PlatformGetExtendedInfo { get; set; } = 
        (path) => new(0, Path.ChangeExtension(Path.GetFileName(path), null));
    /// <summary>
    /// 
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    protected virtual bool HasValidExtension(string path) => Path.GetExtension(path).ToLower() switch
    {
        ".mp4" => true,
        _ => false
    };

    [GeneratedRegex(@"(\d+)\D+(\d+)")]
    private static partial Regex NumericExtract();

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
/// <summary>
/// 
/// </summary>
/// <param name="duration"></param>
/// <param name="title"></param>
public record ExtendedInfo (int duration, string title);