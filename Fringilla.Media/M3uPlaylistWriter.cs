using System.Text;

namespace Fringilla.Media;

/// <summary>
/// 
/// </summary>
public class M3uPlaylistWriter : IPlaylistWriter
{
    /// <inheritdoc/>
    public bool WriteToFile(Playlist playlist, string path)
    {
        if (playlist.FirstOrDefault() is null)
            return false;

        if (playlist.Any(x => !x.IsExtended))
            return new PlainTextPlaylistWriter().WriteToFile(playlist, path);

        StringBuilder content = new();
        content.TeeLine(M3u.ExtFileHeader);

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
