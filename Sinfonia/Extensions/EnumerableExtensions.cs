namespace Sinfonia.Extensions
{
    internal static class EnumerableExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> values, Action<T> action)
        {
            foreach (T? item in values)
            {
                action(item);
            }
        }

        public static IEnumerable<T> SelectRecursive<T>(this T subject, Func<T, IEnumerable<T>> selector)
        {
            Queue<T> stillToProcess = new();
            stillToProcess.Enqueue(subject);

            while (stillToProcess.Count > 0)
            {
                T item = stillToProcess.Dequeue();
                yield return item;
                foreach (T child in selector(item))
                {
                    stillToProcess.Enqueue(child);
                }
            }
        }
    }
}
