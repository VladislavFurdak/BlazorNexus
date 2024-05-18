namespace BlazorNexsus.Navigation.DTOs;

public class NavigationInfo
{
    public Dictionary<string, string> NavigationParams { get; private set; }
    public Dictionary<string, string> QueryStringParams { get; private set; }
    public string FullUri { get; private set; }
    public DateTimeOffset UtcTime { get; private set; }
}