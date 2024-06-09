using StudioLaValse.ScoreDocument.Implementation.Interfaces;
using System.Diagnostics;

namespace Sinfonia.Implementations.Commands
{
    internal class LayoutMementoCommand<TLayout, TMemento> : BaseCommand where TLayout : ILayout<TMemento>
    {
        private readonly TLayout layout;
        private readonly Action<TLayout> action;
        private TMemento? memento = default;

        public LayoutMementoCommand(TLayout layout, Action<TLayout> action)
        {
            this.layout = layout;
            this.action = action;
        }

        public override void Do()
        {
            memento = layout.GetMemento();
            action(layout);
        }

        public override void Undo()
        {
            if (memento is null)
            {
                throw new UnreachableException("Memento not recorded; Undo() method called before do() method.");
            }

            layout.ApplyMemento(memento);
        }
    }
}
