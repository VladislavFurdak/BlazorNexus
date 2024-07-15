namespace BlazorNexsus.Navigation.DTOs;

public class NavigationInfo<T> where T : Enum
{
    public Dictionary<string, string> NavigationParams { get; }
    public Dictionary<string, string> QueryStringParams { get; }
    public string FullUri { get; }
    public DateTimeOffset UtcTime { get; }
    public T PageKey { get; }
}