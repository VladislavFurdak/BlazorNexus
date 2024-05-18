namespace BlazorNexsus.Navigation;

internal static class NavigationManagerExtensions
{
    public static bool InheritsFrom(this Type t, Type baseType)
    {
        var cur = t.BaseType;

        while (cur != null)
        {
            if (cur == baseType)
            {
                return true;
            }

            cur = cur.BaseType;
        }

        return false;
    }
}