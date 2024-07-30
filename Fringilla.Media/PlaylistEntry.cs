using System.Text;

namespace Fringilla.Media;

/// <summary>
/// Base class for playlist entries
/// </summary>
public class PlaylistEntry
{ 
    /// <summary>
    /// Length of media in seconds
    /// </summary>
    public int Duration { get; set; }
    /// <summary>
    /// Title of the media
    /// </summary>
    public string Title { get; set; } = default!;
    /// <summary>
    /// Source to the media relative to the playlist
    /// </summary>
    public string Source { get; set; } = default!;
    /// <summary>
    /// Indicates that Duration and Title are provided so this entry can be an extended entry
    /// </summary>
    public bool IsExtended => Duration != 0 && !string.IsNullOrEmpty(Title);
    /// <summary>
    /// Factory method for creating entries
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="path"></param>
    /// <param name="title"></param>
    /// <param name="duration"></param>
    /// <returns></returns>
    public static T Create<T>(string path, string title = default!, int duration = default) where T : PlaylistEntry, new()
    {
        return new() { Duration = duration, Title = title, Source = path };
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="playlist"></param>
    /// <param name="path"></param>
    public static bool WriteToFile(Playlist playlist, string path)
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
