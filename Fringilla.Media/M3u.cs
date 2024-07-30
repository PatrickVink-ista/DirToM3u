namespace Fringilla.Media;

public class M3u : PlaylistEntry
{
    public const string FileExtension = ".m3u"; // DO NOT TRANSLATE!
    public const string ExtFileHeader = "#EXTM3U"; // DO NOT TRANSLATE!
    public const string ExtInfoLeader = "#EXTINF"; // DO NOT TRANSLATE!

    /// <summary>
    /// Replaces the '#', Extended M3U, marker with "%23"
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static string EncodePath(string path)
    {
        return path.Replace("%", "%25").Replace("#", "%23");
        //System.Text.StringBuilder sb = new();
        //foreach (char c in path)
        //{
        //    switch (c)
        //    {
        //        case '#':
        //        case '%':
        //            sb.Append(string.Format("%{0:X2}", (int)c)); 
        //            break;
        //        default:
        //            sb.Append(c); 
        //            break;
        //    }
        //}
        //return sb.ToString();
    }

    public override string ToString()
    {
        if (IsExtended)
        {
            string[] lines = [$"{M3u.ExtInfoLeader}:{Duration},{Title}", M3u.EncodePath(Path)];
            return string.Join(Environment.NewLine, lines);
        }
        return Path;
    }
}