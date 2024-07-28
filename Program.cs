WMPLib.WindowsMediaPlayer player = new WMPLib.WindowsMediaPlayer();
string path = args.Length > 0 ? args[0] : Environment.CurrentDirectory;
if (path.Last() == '\\')
    path = path.Substring(0, path.Length - 1);
string[] files = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories);
List<string> m3u = [];
Add("#EXTM3U");
foreach (var file in files)
{
    string ext = Path.GetExtension(file).ToLower();
    switch (ext)
    {
        case ".mp4":
            Add(file);
            break;
    }
}
if (m3u.Count > 1)
{
    path = Path.Combine(path, "playlist.m3u");
    File.WriteAllLines(path, m3u);
}
void Add(string file)
{
    if (file.StartsWith('#'))
    {
        InternalAdd(file);
        return;
    }
    //FileInfo fileInfo = new FileInfo(file);
    int duration = GetDuration(file);
    file = file.Substring(path.Length + 1);
    string title = Path.ChangeExtension(Path.GetFileName(file), null);
    InternalAdd($"#EXTINF:{duration},{title}");
    InternalAdd(file);
}
void InternalAdd(string s)
{
    Console.WriteLine(s);
    m3u.Add(s);
}
int GetDuration(string file)
{
    var clip = player.newMedia(file);
    var duration = Convert.ToInt32(TimeSpan.FromSeconds(clip.duration).TotalSeconds);
    player.close();
    return duration;
}