namespace Sugarmaple.Text;
using System.Text;

internal static class StringBuilderPool
{
    static Stack<StringBuilder> pool = new Stack<StringBuilder>();

    public static StringBuilder Obtain()
    {
        if (pool.Count == 0)
            return new StringBuilder(1024);
        return pool.Pop().Clear();
    }

    public static void ToPool(this StringBuilder element)
    {
        pool.Push(element);
    }

    public static string Finish(this StringBuilder element)
    {
        var ret = element.ToString();
        pool.Push(element);
        return ret;
    }
}
