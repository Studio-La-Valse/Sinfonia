namespace Sinfonia.Implementations.ScoreDocument
{
    internal class LayoutCommand<TLayout> : BaseCommand where TLayout : class, ILayoutElement<TLayout>
    {
        private readonly Dictionary<Guid, TLayout> noteLayoutDictionary;
        private readonly Guid element;
        private readonly TLayout newlayout;
        private TLayout? oldLayout;

        public LayoutCommand(Dictionary<Guid, TLayout> noteLayoutDictionary, Guid element, TLayout newlayout)
        {
            this.noteLayoutDictionary = noteLayoutDictionary;
            this.element = element;
            this.newlayout = newlayout;
        }

        public override void Do()
        {
            _ = noteLayoutDictionary.TryGetValue(element, out TLayout? _oldLayout);

            oldLayout = _oldLayout?.Copy();

            noteLayoutDictionary[element] = newlayout;
        }

        public override void Undo()
        {
            if (oldLayout is null)
            {
                _ = noteLayoutDictionary.Remove(element);
            }
            else
            {
                noteLayoutDictionary[element] = oldLayout;
            }
        }
    }
}
