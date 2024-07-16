using System.Reflection;

namespace BlazorNexsus.Navigation.Abstractions;

internal class AssembliesDTO
{
    public IEnumerable<Assembly> AssembliesWithViews { get; set; }
}