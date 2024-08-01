namespace Fringilla.Media;

/// <summary>
/// Base contract for <see cref="Playlist"/> writers
/// </summary>
public interface IPlaylistWriter 
{
    /// <summary>
    /// Writes the tracks from a <see cref="Playlist"/> to a file in the format of instanciated <see cref="IPlaylistWriter"/>
    /// </summary>
    /// <param name="playlist"></param>
    /// <param name="path"></param>
    /// <returns></returns>
    bool WriteToFile(Playlist playlist, string path);
    // todo bool WriteToStream(Playlist playlist, Stream output);
}
