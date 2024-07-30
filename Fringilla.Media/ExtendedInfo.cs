namespace Fringilla.Media;

/// <summary>
/// 
/// </summary>
/// <param name="Duration"></param>
/// <param name="Title"></param>
/// <param name="Source"></param>
public readonly record struct ExtendedInfo (int Duration, string Title, string Source);
/// <summary>
/// 
/// </summary>
/// <param name="path"></param>
/// <returns></returns>
public delegate ExtendedInfo GetExtendedInfo(string path);
