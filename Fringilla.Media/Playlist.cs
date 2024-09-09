using System.Collections;
using System.Text.RegularExpressions;

namespace Fringilla.Media;

/// <summary>
/// Container for <see cref="PlaylistEntry"/>
/// </summary>
public partial class Playlist : IList<PlaylistEntry>
{
    /// <summary>
    /// Creates a Playlist filled by given path
    /// </summary>
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
    /// Fills this Playlist by given path
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

    #region Filtering
    /// <summary>
    /// 
    /// </summary>
    /// <param name="files"></param>
    /// <returns></returns>
    protected virtual IEnumerable<string> Filter(IEnumerable<string> files) => files.Where(CanAccept).ToList();
    /// <summary>
    /// 
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    protected virtual bool CanAccept(string path) => IsExtensionValid(path) && IsFileValid(path);
    /// <summary>
    /// https://en.wikipedia.org/wiki/Video_file_format https://fileinfo.com/filetypes/video
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    protected virtual bool IsExtensionValid(string path) => Path.GetExtension(path).ToLower() switch
    {
        // audio
        ".flac" => true,
        ".mp3" => true,
        ".mpa" => true,
        ".ogg" => true,
        ".wav" => true,
        ".wma" => true,
        // video
        ".3g2" => true,
        ".3gp" => true,
        ".avi" => true,
        ".divx" => true,
        ".f4a" => true,
        ".f4b" => true,
        ".f4p" => true,
        ".f4v" => true,
        ".flv" => true,
        ".mkv" => true,
        ".mov" => true,
        ".mp2" => true,
        ".mp4" => true,
        ".mpe" => true,
        ".mpeg" => true,
        ".mpg" => true,
        ".m2ts" => true,
        ".m2v" => true,
        ".m4v" => true,
        ".mts" => true,
        ".ogv" => true,
        ".swf" => true,
        ".ts" => true,
        ".webm" => true,
        ".wmv" => true,
        // unsupported
        _ => false
    };
    /// <summary>
    /// 
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    protected virtual bool IsFileValid(string path) => File.Exists(path) && new FileInfo(path).Length > 0;
    #endregion

    #region Sorting
    /// <summary>
    /// 
    /// </summary>
    /// <param name="files"></param>
    /// <returns></returns>
    protected virtual IEnumerable<string> Sort(IEnumerable<string> files)
    {
        try
        {
            List<MatchWithSource> matches = files
                .Select(source => new MatchWithSource(NumericExtract.Match(Path.GetFileNameWithoutExtension(source)), source))
                .ToList();
            if (matches.Any(x => !x.RegexMatch.Success) || matches.Count != files.Count())
            {
                return files;
            }
            IEnumerable<(int, int, string)> keyed = matches
                .Select(x => CreateKey(x))
               .ToList();
            IEnumerable<(int, int, string)> indexed = keyed
                .OrderBy(x => x.Item1).ThenBy(x => x.Item2)
                .ToList();
            IEnumerable<string> sorted = indexed
                .Select(x => x.Item3)
                .ToList();
            return sorted;
        }
        catch
        {
            return files;
        }
    }

    private static (int, int, string) CreateKey(MatchWithSource match)
    {
        List<Group> groups = match.RegexMatch.Groups.Values.ToList();
        if (groups.Count < 3)
            return (0, 0, match.Source);
        return (
                int.Parse(groups[1].Value),
                int.Parse(groups[2].Value),
                match.Source);
    }

    //[GeneratedRegex(@"(\d+)\D+(\d+)")]
    //private static partial Regex NumericExtract();
    private static Regex NumericExtract => NumericExtractRegex.Instance;
    #endregion

    #region ExtendedInfo
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
    #endregion

    private readonly List<PlaylistEntry> _entries = [];

    #region IList<PlaylistEntry>
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
    #endregion
}

// Helper record struct for sorting
internal record struct MatchWithSource(Match RegexMatch, string Source)
{
    public static implicit operator (Match, string)(MatchWithSource value)
    {
        return (value.RegexMatch, value.Source);
    }

    public static implicit operator MatchWithSource((Match RegexMatch, string Source) value)
    {
        return new MatchWithSource(value.RegexMatch, value.Source);
    }
}