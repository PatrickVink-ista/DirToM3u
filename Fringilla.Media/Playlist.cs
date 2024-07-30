using System.Collections;
using System.Text;
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
    /// <param name="getExtendedInfo"></param>
    /// <returns></returns>
    public static Playlist<PlaylistEntryType> CreateFromDirectory<PlaylistEntryType>(string path, GetExtendedInfo getExtendedInfo = null!, SearchOption searchOption = SearchOption.AllDirectories) where PlaylistEntryType : PlaylistEntry, new()
    {
        Playlist<PlaylistEntryType> result = new() { PlatformGetExtendedInfo = getExtendedInfo };
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
        _basePath = path.ExcludeTrailingPathDelimiter();
        Clear();
        var files = Sort(
            Filter(
                Directory.GetFiles(path, "*.*", searchOption)
            ));
        foreach (var file in files)
        {
            ExtendedInfo info = GetExtendedInfo(file);
            T item = new() { Duration = info.Duration, Title = info.Title, Path = file.GetRelativePath(_basePath) };
            Add(item);
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="path"></param>
    public void WriteToFile(string path)
    {
        PlaylistEntry? first = this.FirstOrDefault();
        if (first is null)
            return;

        bool isExtended = this.All(x => x.IsExtended);

        StringBuilder playlist = new();

        if (isExtended)
            switch (first) 
            { 
                case M3u m3u:
                    playlist.AppendLine(M3u.ExtFileHeader);
                    break;
                case Pls pls:
                    playlist.AppendLine(Pls.FileHeader);
                    break;
            }

        foreach (var file in _entries)
        {
            string s = isExtended ? file.ToString() ?? file.Path : file.Path;
#if DEBUG
            //Console.WriteLine(s);
            System.Diagnostics.Debug.WriteLine(s);
#endif
            playlist.AppendLine(s);
        }

        if (isExtended)
            switch (first)
            {
                case Pls pls:
                    playlist.AppendLine($"{Pls.NumberOfEntriesKey}={Count}");
                    playlist.AppendLine($"{Pls.VersionKey}=2");
                    break;
            }

        File.WriteAllText(path, playlist.ToString());
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

    private string _basePath = string.Empty;
    /// <summary>
    /// 
    /// </summary>
    public string BasePath => _basePath;

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
