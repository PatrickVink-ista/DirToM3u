string path = args.Length > 0 ? args[0] : Environment.CurrentDirectory;
if (path.Last() == Path.DirectorySeparatorChar)
    path = path.Substring(0, path.Length - 1);

string[] files = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories);

List<string> m3u = [];

WMPLib.WindowsMediaPlayer player = new WMPLib.WindowsMediaPlayer();

Sort();

Add("#EXTM3U");

foreach (var file in files)
{
    Add(file);
}

if (HasItems())
{
    path = Path.Combine(path, "playlist.m3u");
    File.WriteAllLines(path, m3u);
}

void Sort()
{
    var filtered = files
        .Where(HasValidExtension);
    var matches = filtered
        .Select(x => (NumericExtract().Match(Path.GetFileName(x)), x));
    if (matches.Count() != filtered.Count())
    {
        files = filtered.ToArray();
        return;
    }
    var keyed = matches
        .Select(x => (
            int.Parse(x.Item1.Groups.Values.ToList()[1].Value), 
            int.Parse(x.Item1.Groups.Values.ToList()[2].Value), 
            x.Item2));
    var indexed = keyed
        .OrderBy(x => x.Item1).ThenBy(x => x.Item2);
    var sorted = indexed
        .Select(x => x.Item3);
    files = sorted
        .ToArray();
}

bool HasValidExtension(string path) => Path.GetExtension(path).ToLower() switch 
{ 
    ".mp4" => true, 
    _ => false 
};

void Add(string file)
{
    if (file.StartsWith('#'))
    {
        InternalAdd(file);
        return;
    }
    FileInfo fileInfo = new FileInfo(file);
    if (fileInfo.Length == 0)
        return;
    WMPLib.IWMPMedia clip = player.newMedia(file);
    string relPath = file.Substring(path.Length + 1);
    int duration = clip.GetDuration();
    string title = clip.GetTitle(() => Path.ChangeExtension(Path.GetFileName(relPath), null));
    InternalAdd($"#EXTINF:{duration},{title}");
    InternalAdd(relPath);
    player.close();
}

void InternalAdd(string s)
{
    Console.WriteLine(s);
    m3u.Add(s);
}

bool HasItems() => m3u.Count > 1;