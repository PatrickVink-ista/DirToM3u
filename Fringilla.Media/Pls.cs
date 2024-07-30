namespace Fringilla.Media;
/// <summary>
/// PlaylistEntry in .pls format
/// </summary>
public class Pls : PlaylistEntry
{
    /// <summary>
    /// Index of the track on the playlist
    /// </summary>
    public int Index { get; set; } = 1;
    /// <summary>
    /// File extension for a PLSv2 (.pls) file
    /// </summary>
    public const string FileExtension = ".pls"; // DO NOT TRANSLATE!
    /// <summary>
    /// Start of the content of a PLSv2 (.pls) file
    /// </summary>
    public const string FileHeader = "[playlist]"; // DO NOT TRANSLATE!
    /// <summary>
    /// Key for tracks path to media
    /// </summary>
    public const string FileKeyFormat = "File{0}"; // DO NOT TRANSLATE!
    /// <summary>
    /// Key for tracks title
    /// </summary>
    public const string TitleKeyFormat = "Title{0}"; // DO NOT TRANSLATE!
    /// <summary>
    /// Key for tracks duration
    /// </summary>
    public const string LengthKeyFormat = "Length{0}"; // DO NOT TRANSLATE!
    /// <summary>
    /// Key for total count of entries on playlist
    /// </summary>
    public const string NumberOfEntriesKey = "NumberOfEntries"; // DO NOT TRANSLATE!
    /// <summary>
    /// Key for optional version. Must be 2 or not exist
    /// </summary>
    public const string VersionKey = "Version"; // DO NOT TRANSLATE!
    /// <summary>
    /// Returns the entries according to the PLS format
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        if (IsExtended)
        {
            string[] lines = [
                $"{string.Format(FileKeyFormat, Index)}={Path}",
                $"{string.Format(TitleKeyFormat, Index)}={Title}",
                $"{string.Format(LengthKeyFormat, Index)}={Duration}",
                ];
            return string.Join(Environment.NewLine, lines);
        }
        return Path;
    }
}
