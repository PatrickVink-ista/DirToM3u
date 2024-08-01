using System.Text;

namespace Fringilla.Media;

/// <summary>
/// 
/// </summary>
public class PlsPlaylistWriter : IPlaylistWriter
{
    /// <inheritdoc/>
    public bool WriteToFile(Playlist playlist, string path)
    {
        if (playlist.FirstOrDefault() is null)
            return false;

        if (playlist.Any(x => !x.IsExtended))
            return new PlainTextPlaylistWriter().WriteToFile(playlist, path);

        StringBuilder content = new();

        content.TeeLine(Pls.FileHeader);

        string basePath = Path.GetDirectoryName(Path.GetFullPath(path)) ?? string.Empty;
        int index = 0;
        foreach (Pls track in playlist.Select(x => new Pls() { Duration = x.Duration, Title = x.Title, Source = x.Source.GetRelativePath(basePath), Index = ++index }))
            content.TeeLine(track.ToString());

        content.TeeLine($"{Pls.NumberOfEntriesKey}={playlist.Count}");
        content.TeeLine($"{Pls.VersionKey}=2");

        File.WriteAllText(path, content.ToString());

        return true;
    }
}
