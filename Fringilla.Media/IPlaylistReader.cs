namespace Fringilla.Media;

public interface IPlaylistReader
{
    bool ReadFromDirectory(IPlaylist playlist, string path, SearchOption searchOption = SearchOption.AllDirectories);
}
