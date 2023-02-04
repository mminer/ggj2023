using System.Collections.Generic;

public static class StackExtensions
{
    public static void AddRange<T>(this Stack<T> stack, IEnumerable<T> collection)
    {
        foreach (var item in collection)
        {
            stack.Push(item);
        }
    }
}
