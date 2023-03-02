namespace GraphVisitor.UI;

public static class Constants
{
    public const string ApiClientName = nameof(ApiClientName);

#if DEBUG && WINDOWS
    public const string BaseUrl = "https://localhost:5001";
#elif DEBUG && ANDROID
    public const string BaseUrl = "http://10.0.2.2";
#elif DEBUG && IOS
    public const string BaseUrl = "<replace with tunnel URI>";
#else
    public const string BaseUrl = "<replace with prod URI>";
#endif

}
