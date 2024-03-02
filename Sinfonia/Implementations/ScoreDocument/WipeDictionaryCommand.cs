namespace Sinfonia.Implementations.ScoreDocument
{
    internal class WipeDictionaryCommand<TLayout> : BaseCommand where TLayout : class, ILayoutElement<TLayout>
    {
        private readonly Dictionary<Guid, TLayout> noteLayoutDictionary;
        private Dictionary<Guid, TLayout> oldLayout = [];

        public WipeDictionaryCommand(Dictionary<Guid, TLayout> noteLayoutDictionary)
        {
            this.noteLayoutDictionary = noteLayoutDictionary;
        }

        public override void Do()
        {
            oldLayout.Clear();

            foreach (var kv in noteLayoutDictionary)
            {
                oldLayout.Add(kv.Key, kv.Value.Copy());
            }

            noteLayoutDictionary.Clear();
        }

        public override void Undo()
        {
            noteLayoutDictionary.Clear();

            foreach (var kv in oldLayout)
            {
                noteLayoutDictionary.Add(kv.Key, kv.Value.Copy());
            }

            oldLayout.Clear();
        }
    }
}
