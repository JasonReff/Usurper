using System.Collections.Generic;
using System.Linq;

public static class Extensions
{
    public static T Rand<T>(this List<T> list)
    {
        var random = new System.Random();
        T item = (T)list.OrderBy(t => random.Next()).First();
        return item;
    }
}