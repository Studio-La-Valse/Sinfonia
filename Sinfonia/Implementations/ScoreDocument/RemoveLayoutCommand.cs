namespace Sinfonia.Implementations.ScoreDocument
{
    internal class RemoveLayoutCommand<TLayout> : BaseCommand where TLayout : class, ILayoutElement<TLayout>
    {
        private readonly Dictionary<Guid, TLayout> noteLayoutDictionary;
        private readonly Guid element;
        private TLayout? oldLayout;

        public RemoveLayoutCommand(Dictionary<Guid, TLayout> noteLayoutDictionary, Guid element)
        {
            this.noteLayoutDictionary = noteLayoutDictionary;
            this.element = element;
        }

        public override void Do()
        {
            if (!noteLayoutDictionary.TryGetValue(element, out var oldLayout))
            {
                return;
            }

            this.oldLayout = oldLayout;

            noteLayoutDictionary.Remove(element);
        }

        public override void Undo()
        {
            if (oldLayout is null)
            {
                return;
            }

            noteLayoutDictionary[element] = oldLayout;
        }
    }
}
