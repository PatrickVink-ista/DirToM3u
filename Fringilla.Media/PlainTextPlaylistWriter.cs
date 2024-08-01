using System.Text;

namespace Fringilla.Media;

/// <summary>
/// IPlaylistWriter to write a plain-text list of tracks without extended information
/// </summary>
public class PlainTextPlaylistWriter : IPlaylistWriter
{
    /// <inheritdoc/>
    public bool WriteToFile(Playlist playlist, string path)
    {
        if (playlist.FirstOrDefault() is null)
            return false;

        StringBuilder content = new();
        
        string basePath = Path.GetDirectoryName(Path.GetFullPath(path)) ?? string.Empty;

        foreach (string track in playlist.Select(x => x.Source.GetRelativePath(basePath)))
            content.TeeLine(track);

        File.WriteAllText(path, content.ToString());

        return true;
    }
}
