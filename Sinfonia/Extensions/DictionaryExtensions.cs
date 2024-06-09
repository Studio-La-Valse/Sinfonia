namespace Sinfonia.Extensions
{
    public static class DictionaryExtensions
    {
        public static Dictionary<TKey, TValue> DeepCopy<TKey, TValue>(this IDictionary<TKey, TValue> dictionary) where TKey : IEquatable<TKey> where TValue : struct
        {
            var dict = new Dictionary<TKey, TValue>();
            foreach (var kv in dictionary)
            {
                dict[kv.Key] = kv.Value;
            }
            return dict;
        }

        public static void Replace<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> dictionary, Dictionary<TKey, TValue> target) where TKey : IEquatable<TKey> where TValue : struct
        {
            target.Clear();
            foreach (var kv in dictionary)
            {
                target[kv.Key] = kv.Value;
            }
        }
    }
}
