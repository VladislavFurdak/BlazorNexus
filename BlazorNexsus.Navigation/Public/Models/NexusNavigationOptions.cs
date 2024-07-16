namespace BlazorNexsus.Navigation;

public class NexusNavigationOptions<T> where T : Enum
{
    public bool NewTab { get; set; } = false;
    public T? BackPage { get; set; }
    public IReadOnlyDictionary<string, string>? NavigationParams { get; set; } = null;
    public IReadOnlyDictionary<string, string>? QueryParams { get; set; } = null;
}