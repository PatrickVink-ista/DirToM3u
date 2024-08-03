namespace Fringilla.Media;

public interface IPlaylistFilter
{
    IEnumerable<string> Filter(IEnumerable<string> files);
}
