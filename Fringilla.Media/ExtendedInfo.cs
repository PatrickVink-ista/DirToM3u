namespace Fringilla.Media;

/// <summary>
/// 
/// </summary>
/// <param name="Duration"></param>
/// <param name="Title"></param>
/// <param name="Path"></param>
public readonly record struct ExtendedInfo (int Duration, string Title, string Path);
/// <summary>
/// 
/// </summary>
/// <param name="path"></param>
/// <returns></returns>
public delegate ExtendedInfo GetExtendedInfo(string path);
