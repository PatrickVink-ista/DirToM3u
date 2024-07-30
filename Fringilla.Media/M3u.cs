using System.Text;

namespace Fringilla.Media;

/// <summary>
/// PlaylistEntry in .m3u format
/// </summary>
public class M3u : PlaylistEntry
{
    /// <summary>
    /// File extension
    /// </summary>
    public const string FileExtension = ".m3u"; // DO NOT TRANSLATE!
    /// <summary>
    /// Header of a content track
    /// </summary>
    public const string ExtFileHeader = "#EXTM3U"; // DO NOT TRANSLATE!
    /// <summary>
    /// Leader for extended info
    /// </summary>
    public const string ExtInfoLeader = "#EXTINF"; // DO NOT TRANSLATE!
    /// <summary>
    /// Replaces the '#', Extended M3U, marker with "%23"
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static string EncodePath(string path)
    {
        return path.Replace("%", "%25").Replace("#", "%23");
        //System.Text.StringBuilder sb = new();
        //foreach (char c in path)
        //{
        //    switch (c)
        //    {
        //        case '#':
        //        case '%':
        //            sb.Append(string.Format("%{0:X2}", (int)c)); 
        //            break;
        //        default:
        //            sb.Append(c); 
        //            break;
        //    }
        //}
        //return sb.ToString();
    }
    /// <summary>
    /// Returns the lines for this entry as it should be written in a content
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        if (IsExtended)
        {
            string[] lines = [$"{ExtInfoLeader}:{Duration},{Title}", EncodePath(Source)];
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

        if (playlist.Any(x => !x.IsExtended))
            return PlaylistEntry.WriteToFile(playlist, path);

        StringBuilder content = new();
        content.TeeLine(ExtFileHeader);

        string basePath = Path.GetDirectoryName(Path.GetFullPath(path)) ?? string.Empty;
        foreach (M3u track in playlist.Select(x => new M3u() { Duration = x.Duration, Title = x.Title, Source = x.Source.GetRelativePath(basePath) }))
        {
            string s = track.ToString();
            content.TeeLine(s);
        }

        File.WriteAllText(path, content.ToString());

        return true;
    }
}