using System.Text.RegularExpressions;

WMPLib.WindowsMediaPlayer player = new WMPLib.WindowsMediaPlayer();
WMPLib.IWMPMedia clip;
string path = args.Length > 0 ? args[0] : Environment.CurrentDirectory;
if (path.Last() == '\\')
    path = path.Substring(0, path.Length - 1);
string[] files = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories);
Sort();
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
    var keyed = matches
        .Select(x => (int.Parse(x.Item1.Groups.Values.ToList()[1].Value), int.Parse(x.Item1.Groups.Values.ToList()[2].Value), x.Item2));
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
    if (file.Equals("#EXTM3U"))
    {
        InternalAdd(file);
        return;
    }
    //FileInfo fileInfo = new FileInfo(file);
    clip = player.newMedia(file);
    int duration = GetDuration(file);
    file = file.Substring(path.Length + 1);
    string title = Path.ChangeExtension(Path.GetFileName(file), null);
    InternalAdd($"#EXTINF:{duration},{title}");
    InternalAdd(file);
    player.close();
}
void InternalAdd(string s)
{
    Console.WriteLine(s);
    m3u.Add(s);
}
int GetDuration(string file)
{   
    var duration = Convert.ToInt32(TimeSpan.FromSeconds(clip.duration).TotalSeconds);
    return duration;
}
bool HasItems() => m3u.Count > 1;

partial class Program
{
    [GeneratedRegex(@"(\d+)\D+(\d+)")]
    private static partial Regex NumericExtract();
}