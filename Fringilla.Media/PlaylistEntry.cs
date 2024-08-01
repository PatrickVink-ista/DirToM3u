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
}
