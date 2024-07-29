public static class WMPLibExtensions
{
    public static int GetDuration(this WMPLib.IWMPMedia media)
    {
        int result = media != null ? Convert.ToInt32(TimeSpan.FromSeconds(media.duration).TotalSeconds) : 0;
        return result;
    }
    public static string GetTitle(this WMPLib.IWMPMedia media, Func<string>? fallBack = null)
    {
        string result = media != null ? media.name : string.Empty;
        if (string.IsNullOrEmpty(result) && fallBack != null)
            result = fallBack();
        return result;
    }
}