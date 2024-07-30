namespace Fringilla.Media;

public class PlaylistEntry
{ 
    public int Duration { get; set; }
    public string Title { get; set; } = default!;
    public string Path { get; set; } = default!;

    public bool IsExtended => Duration != 0 && !string.IsNullOrEmpty(Title);

    public static PlaylistEntry Create(string path, string title = default!, int duration = default)
    {
        return new PlaylistEntry() { Duration = duration, Title = title, Path = path };
    }
}
