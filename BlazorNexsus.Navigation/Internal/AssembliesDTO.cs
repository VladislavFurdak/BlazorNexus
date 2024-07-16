using System.Reflection;

namespace BlazorNexsus.Navigation.Abstractions;

public class AssembliesDTO
{
    public IEnumerable<Assembly> AssembliesWithViews { get; set; }
}