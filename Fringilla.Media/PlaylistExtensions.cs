namespace Fringilla.Media;

/// <summary>
/// 
/// </summary>
public static class PlaylistExtensions
{
    /// <summary>
    /// Generic WriteToFile method
    /// </summary>
    /// <param name="playlist"></param>
    /// <param name="path"></param>
    public static bool WriteToFile<T>(this Playlist playlist, string path) where T : PlaylistEntry
    {
        if (typeof(T) == typeof(M3u))
            return playlist.WriteToM3uFile(path);
        else if (typeof(T) == typeof(Pls))
            return playlist.WriteToPlsFile(path);
        else
            return playlist.WriteToPlainTextFile(path);
    }
    /// <summary>
    /// Writes an .m3u file, when is extended
    /// </summary>
    /// <param name="playlist"></param>
    /// <param name="path"></param>
    /// <returns></returns>
    public static bool WriteToM3uFile(this Playlist playlist, string path) => new M3uPlaylistWriter().WriteToFile(playlist, path);
    /// <summary>
    /// Writes an .pls file, when is extended
    /// </summary>
    /// <param name="playlist"></param>
    /// <param name="path"></param>
    /// <returns></returns>
    public static bool WriteToPlsFile(this Playlist playlist, string path) => new PlsPlaylistWriter().WriteToFile(playlist, path);
    /// <summary>
    /// Writes plain list file, not extended
    /// </summary>
    /// <param name="playlist"></param>
    /// <param name="path"></param>
    /// <returns></returns>
    public static bool WriteToPlainTextFile(this Playlist playlist, string path) => new PlainTextPlaylistWriter().WriteToFile(playlist, path);
}
