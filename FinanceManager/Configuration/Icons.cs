namespace FinanceManager.Configuration;

public static class Icons
{
    public static string Circle(string? classes = null)
    {
        return Icon($"bi bi-circle {classes}",
            "<path d=\"M8 15A7 7 0 1 1 8 1a7 7 0 0 1 0 14m0 1A8 8 0 1 0 8 0a8 8 0 0 0 0 16\"/>");
    }

    public static string ArrowDownCircle(string? classes = null)
    {
        return Icon($"bi bi-arrow-down-circle {classes}",
            "<path fill-rule=\"evenodd\" d=\"M1 8a7 7 0 1 0 14 0A7 7 0 0 0 1 8m15 0A8 8 0 1 1 0 8a8 8 0 0 1 16 0M8.5 4.5a.5.5 0 0 0-1 0v5.793L5.354 8.146a.5.5 0 1 0-.708.708l3 3a.5.5 0 0 0 .708 0l3-3a.5.5 0 0 0-.708-.708L8.5 10.293z\"/>");
    }

    public static string ArrowUpCircle(string? classes = null)
    {
        return Icon($"bi bi-arrow-up-circle {classes}",
            "<path fill-rule=\"evenodd\" d=\"M1 8a7 7 0 1 0 14 0A7 7 0 0 0 1 8m15 0A8 8 0 1 1 0 8a8 8 0 0 1 16 0m-7.5 3.5a.5.5 0 0 1-1 0V5.707L5.354 7.854a.5.5 0 1 1-.708-.708l3-3a.5.5 0 0 1 .708 0l3 3a.5.5 0 0 1-.708.708L8.5 5.707z\"/>\n");
    }
    
    private static string Icon(string cssClass, string pathTag,
        int width = 16, int height = 16, string fill = "currentColor", string viewBox = "0 0 16 16")
    {
        return
            $"<svg xmlns=\"http://www.w3.org/2000/svg\" width=\"{width}\" height=\"{height}\" fill=\"{fill}\" class=\"{cssClass}\" viewBox=\"{viewBox}\">\n  {pathTag}\n</svg>";
    }
    
}