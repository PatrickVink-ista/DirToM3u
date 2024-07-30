using System.Text;

namespace Fringilla.Media;
/// <summary>
/// PlaylistEntry in .pls format
/// </summary>
public class Pls : PlaylistEntry
{
    /// <summary>
    /// Index of the track on the playlist
    /// </summary>
    public int Index { get; set; } = 1;
    /// <summary>
    /// File extension for a PLSv2 (.pls) file
    /// </summary>
    public const string FileExtension = ".pls"; // DO NOT TRANSLATE!
    /// <summary>
    /// Start of the content of a PLSv2 (.pls) file
    /// </summary>
    public const string FileHeader = "[playlist]"; // DO NOT TRANSLATE!
    /// <summary>
    /// Key for tracks path to media
    /// </summary>
    public const string FileKeyFormat = "File{0}"; // DO NOT TRANSLATE!
    /// <summary>
    /// Key for tracks title
    /// </summary>
    public const string TitleKeyFormat = "Title{0}"; // DO NOT TRANSLATE!
    /// <summary>
    /// Key for tracks duration
    /// </summary>
    public const string LengthKeyFormat = "Length{0}"; // DO NOT TRANSLATE!
    /// <summary>
    /// Key for total count of entries on playlist
    /// </summary>
    public const string NumberOfEntriesKey = "NumberOfEntries"; // DO NOT TRANSLATE!
    /// <summary>
    /// Key for optional version. Must be 2 or not exist
    /// </summary>
    public const string VersionKey = "Version"; // DO NOT TRANSLATE!
    /// <summary>
    /// Returns the entries according to the PLS format
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        if (IsExtended)
        {
            string[] lines = [
                $"{string.Format(FileKeyFormat, Index)}={Source}",
                $"{string.Format(TitleKeyFormat, Index)}={Title}",
                $"{string.Format(LengthKeyFormat, Index)}={Duration}",
                ];
            return string.Join(Environment.NewLine, lines);
        }
        return Source;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="playlist"></param>
    /// <param name="path"></param>
    public static new bool WriteToFile(Playlist playlist, string path)
    {
        if (playlist.FirstOrDefault() is null)
            return false;

        bool isExtended = playlist.All(x => x.IsExtended);

        StringBuilder content = new();

        content.TeeLine(FileHeader);

        string basePath = Path.GetDirectoryName(Path.GetFullPath(path)) ?? string.Empty;
        int index = 0;
        foreach (Pls track in playlist.Select(x => new Pls() { Duration = x.Duration, Title = x.Title, Source = x.Source.GetRelativePath(basePath), Index = ++index }))
        {
            string s = isExtended ? track.ToString() ?? track.Source : track.Source;
            content.TeeLine(s);
        }

        content.TeeLine($"{NumberOfEntriesKey}={playlist.Count}");
        content.TeeLine($"{VersionKey}=2");

        File.WriteAllText(path, content.ToString());

        return true;
    }
}
