namespace Fringilla.Media;

/// <summary>
/// 
/// </summary>
public static class PathExtensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static string ExcludeTrailingPathDelimiter(this string path) => path.Last() == Path.DirectorySeparatorChar ? path[..^1] : path;
    /// <summary>
    /// 
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static string IncludeTrailingPathDelimiter(this string path) => path.Last() != Path.DirectorySeparatorChar ? path + Path.DirectorySeparatorChar : path;
    /// <summary>
    /// 
    /// </summary>
    /// <param name="path"></param>
    /// <param name="basePath"></param>
    /// <returns></returns>
    public static string GetRelativePath(this string path, string basePath) => !string.IsNullOrEmpty(basePath) ? path[(basePath.ExcludeTrailingPathDelimiter().Length + 1)..] : path;
}