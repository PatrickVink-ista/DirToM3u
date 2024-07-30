using System.Collections;
using System.Text;
using System.Text.RegularExpressions;

namespace Fringilla.Media;

/// <summary>
/// 
/// </summary>
/// <typeparam name="T"></typeparam>
public partial class Playlist : IList<PlaylistEntry>
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="PlaylistEntryType"></typeparam>
    /// <param name="path"></param>
    /// <param name="getExtendedInfo"></param>
    /// <param name="searchOption"></param>
    /// <returns></returns>
    public static Playlist CreateFromDirectory(string path, GetExtendedInfo getExtendedInfo = null!, SearchOption searchOption = SearchOption.AllDirectories)
    {
        Playlist result = new() { PlatformGetExtendedInfo = getExtendedInfo };
        result.ReadFromDirectory(path, searchOption);
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
            PlaylistEntry item = new() { Duration = info.Duration, Title = info.Title, Source = file };
            Add(item);
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="path"></param>
    public void WriteToFile<T>(string path) where T : PlaylistEntry
    {
        if (typeof(T) == typeof(M3u))
            M3u.WriteToFile(this, path);
        else if (typeof(T) == typeof(Pls))
            Pls.WriteToFile(this, path);
        else
            PlaylistEntry.WriteToFile(this, path);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="files"></param>
    /// <returns></returns>
    protected virtual IEnumerable<string> Filter(IEnumerable<string> files) => files.Where(CanAccept);
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
    protected virtual ExtendedInfo GetExtendedInfo(string path) => 
        PlatformGetExtendedInfo != null ? PlatformGetExtendedInfo(path) : DefaultGetExtendedInfo(path);
    private static ExtendedInfo DefaultGetExtendedInfo(string path) => new(0, Path.ChangeExtension(Path.GetFileName(path), null), path);
    /// <summary>
    /// 
    /// </summary>
    public GetExtendedInfo? PlatformGetExtendedInfo { get; set; }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    protected virtual bool CanAccept(string path) => HasValidExtension(path) && HasValidSize(path);
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
    /// <summary>
    /// 
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    protected virtual bool HasValidSize(string path) => new FileInfo(path).Length > 0;

    [GeneratedRegex(@"(\d+)\D+(\d+)")]
    private static partial Regex NumericExtract();

    private readonly List<PlaylistEntry> _entries = [];

    /// <inheritdoc/>
    public PlaylistEntry this[int index] { get => ((IList<PlaylistEntry>)_entries)[index]; set => ((IList<PlaylistEntry>)_entries)[index] = value; }
    /// <inheritdoc/>
    public int Count => ((ICollection<PlaylistEntry>)_entries).Count;
    /// <inheritdoc/>
    public bool IsReadOnly => ((ICollection<PlaylistEntry>)_entries).IsReadOnly;
    /// <inheritdoc/>
    public void Add(PlaylistEntry item) => ((ICollection<PlaylistEntry>)_entries).Add(item);
    /// <inheritdoc/>
    public void Clear() => ((ICollection<PlaylistEntry>)_entries).Clear();
    /// <inheritdoc/>
    public bool Contains(PlaylistEntry item) => ((ICollection<PlaylistEntry>)_entries).Contains(item);
    /// <inheritdoc/>
    public void CopyTo(PlaylistEntry[] array, int arrayIndex) => ((ICollection<PlaylistEntry>)_entries).CopyTo(array, arrayIndex);
    /// <inheritdoc/>
    public IEnumerator<PlaylistEntry> GetEnumerator() => ((IEnumerable<PlaylistEntry>)_entries).GetEnumerator();
    /// <inheritdoc/>
    public int IndexOf(PlaylistEntry item) => ((IList<PlaylistEntry>)_entries).IndexOf(item);
    /// <inheritdoc/>
    public void Insert(int index, PlaylistEntry item) => ((IList<PlaylistEntry>)_entries).Insert(index, item);
    /// <inheritdoc/>
    public bool Remove(PlaylistEntry item) => ((ICollection<PlaylistEntry>)_entries).Remove(item);
    /// <inheritdoc/>
    public void RemoveAt(int index) => ((IList<PlaylistEntry>)_entries).RemoveAt(index);
    /// <inheritdoc/>
    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_entries).GetEnumerator();
}
