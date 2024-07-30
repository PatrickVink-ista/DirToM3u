using System.Text;

namespace Fringilla.Media;

internal static class TeeStringBuilderExtension 
{
    public static StringBuilder TeeLine(this StringBuilder sb, string? value)
    {
        sb.AppendLine(value);
        Console.WriteLine(value);
        return sb;
    }
}