using Fringilla.Media;

WMPLib.WindowsMediaPlayer player = new();

string path = (args.Length > 0 ? args[0] : Environment.CurrentDirectory).ExcludeTrailingPathDelimiter();

Playlist<M3u> playlist = Playlist<M3u>.CreateFromDirectory<M3u>(path, GetExtendedInfo);
string playlistPath = Path.Combine(path, "playlist.m3u");
if (playlist.Count > 0)
    playlist.WriteToFile(playlistPath);
else
    File.Delete(playlistPath);

ExtendedInfo GetExtendedInfo(string path)
{
    WMPLib.IWMPMedia clip = player.newMedia(path);
    int duration = clip.GetDuration();
    string title = clip.GetTitle(() => Path.ChangeExtension(Path.GetFileName(path), null));
    player.close();
    return new(duration, title, path);
}
