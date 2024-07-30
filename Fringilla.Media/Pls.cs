namespace Fringilla.Media;

public class Pls : PlaylistEntry
{
    public int Index { get; set; } = 1;

    public const string FileExtension = ".pls"; // DO NOT TRANSLATE!
    public const string FileHeader = "[playlist]"; // DO NOT TRANSLATE!
    public const string FileKeyFormat = "File{0}"; // DO NOT TRANSLATE!
    public const string TitleKeyFormat = "Title{0}"; // DO NOT TRANSLATE!
    public const string LengthKeyFormat = "Length{0}"; // DO NOT TRANSLATE!
    public const string NumberOfEntriesKey = "NumberOfEntries"; // DO NOT TRANSLATE!
    public const string VersionKey = "Version"; // DO NOT TRANSLATE!

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
