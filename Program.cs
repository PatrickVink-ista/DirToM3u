using Fringilla.Media;

WMPLib.WindowsMediaPlayer player = new();

string path = (args.Length > 0 ? args[0] : Environment.CurrentDirectory).ExcludeTrailingPathDelimiter();

Playlist playlist = Playlist.CreateFromDirectory(path, GetExtendedInfo);
if (playlist.Count > 0)
{
    playlist.WriteToFile<M3u>(Path.Combine(path, "playlist.m3u"));
    playlist.WriteToFile<Pls>(Path.Combine(path, "playlist.pls"));
    playlist.WriteToFile<PlaylistEntry>(Path.Combine(path, "playlist.txt"));
}
else
{
    File.Delete(Path.Combine(path, "playlist.m3u"));
    File.Delete(Path.Combine(path, "playlist.pls"));
    File.Delete(Path.Combine(path, "playlist.txt"));
}
ExtendedInfo GetExtendedInfo(string path)
{
    WMPLib.IWMPMedia clip = player.newMedia(path);
    int duration = clip.GetDuration();
    string title = clip.GetTitle(() => Path.ChangeExtension(Path.GetFileName(path), null));
    player.close();
    return new(duration, title, path);
}
