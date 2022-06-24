using System.Collections.Generic;
using System.Linq;

public static class GameExtensions
{
    public static T Rand<T>(this List<T> list)
    {
        var random = new System.Random();
        T item = (T)list.OrderBy(t => random.Next()).ToList().First();
        return item;
    }

    public static T Rand<T>(this List<T> list, System.Random random)
    {
        T item = (T)list.OrderBy(t => random.Next()).ToList().First();
        return item;
    }

    public static List<T> Pull<T>(this List<T> list, int numberToPull)
    {
        System.Random random = new System.Random();
        if (list.Count < numberToPull)
            numberToPull = list.Count;
        List<T> newList = list.OrderBy(t => random.Next()).Take(numberToPull).ToList();
        return newList;
    }

    public static UnitFaction GetOpposite(this UnitFaction faction)
    {
        if (faction == UnitFaction.Player)
            return UnitFaction.Enemy;
        else return UnitFaction.Player;
    }
}