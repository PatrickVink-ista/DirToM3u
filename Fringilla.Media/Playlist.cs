namespace Fringilla.Media;

public class Playlist 
{
    private List<PlaylistEntry> _entries = [];
    public PlaylistEntry this[int index] => _entries[index];
}
