namespace Sinfonia.Implementations.Commands
{
    internal class WipeDictionaryCommand<TLayout> : WipeDictionaryCommand<Guid, TLayout> where TLayout : class, ILayoutElement<TLayout>
    {
        public WipeDictionaryCommand(Dictionary<Guid, TLayout> noteLayoutDictionary) : base(noteLayoutDictionary, (e) => e.Copy())
        {

        }
    }

    internal abstract class WipeDictionaryCommand<TKey, TValue> : BaseCommand where TKey : IEquatable<TKey>
    {
        private readonly Dictionary<TKey, TValue> noteLayoutDictionary;
        private readonly Func<TValue, TValue> copy;
        private readonly Dictionary<TKey, TValue> oldLayout = [];

        public WipeDictionaryCommand(Dictionary<TKey, TValue> noteLayoutDictionary, Func<TValue, TValue> copy)
        {
            this.noteLayoutDictionary = noteLayoutDictionary;
            this.copy = copy;
        }

        public override void Do()
        {
            oldLayout.Clear();

            foreach (KeyValuePair<TKey, TValue> kv in noteLayoutDictionary)
            {
                oldLayout.Add(kv.Key, copy(kv.Value));
            }

            noteLayoutDictionary.Clear();
        }

        public override void Undo()
        {
            noteLayoutDictionary.Clear();

            foreach (KeyValuePair<TKey, TValue> kv in oldLayout)
            {
                noteLayoutDictionary.Add(kv.Key, copy(kv.Value));
            }

            oldLayout.Clear();
        }
    }
}
