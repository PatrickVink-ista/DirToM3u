﻿using Fringilla.Media;

WMPLib.WindowsMediaPlayer? player = null;

string path = (args.Length > 0 ? args[0] : Environment.CurrentDirectory).ExcludeTrailingPathDelimiter();
if (!Directory.Exists(path))
    throw new DirectoryNotFoundException(path);

Playlist playlist = Playlist.CreateFromDirectory(path, GetExtendedInfo);
if (playlist.Count > 0)
{
    playlist.WriteToM3uFile(Path.Combine(path, "playlist.m3u"));
    playlist.WriteToPlsFile(Path.Combine(path, "playlist.pls"));
    playlist.WriteToPlainTextFile(Path.Combine(path, "playlist.txt"));
}
else
{
    File.Delete(Path.Combine(path, "playlist.m3u"));
    File.Delete(Path.Combine(path, "playlist.pls"));
    File.Delete(Path.Combine(path, "playlist.txt"));
}
ExtendedInfo GetExtendedInfo(string path)
{
    WMPLib.IWMPMedia clip = (player ??= new()).newMedia(path);
    int duration = clip.GetDuration();
    string title = clip.GetTitle(() => Path.ChangeExtension(Path.GetFileName(path), null));
    player.close();
    return new(duration, title, path);
}
