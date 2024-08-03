using System.CodeDom.Compiler;

namespace Fringilla.Media;

public interface IPlaylistSort
{
    IEnumerable<string> Sort(IEnumerable<string> files);
}
