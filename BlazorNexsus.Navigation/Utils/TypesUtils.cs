namespace BlazorNexsus.Navigation.Utils;

internal static class TypesUtils
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