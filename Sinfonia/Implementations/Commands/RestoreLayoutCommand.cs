using StudioLaValse.ScoreDocument.Implementation.Interfaces;
using System.Diagnostics;

namespace Sinfonia.Implementations.Commands
{
    internal class RestoreLayoutCommand<TLayout, TMemento> : BaseCommand where TLayout : ILayout<TMemento>
    {
        private readonly TLayout layout;
        private TMemento? memento = default;

        public RestoreLayoutCommand(TLayout layout)
        {
            this.layout = layout;
        }

        public override void Do()
        {
            memento = layout.GetMemento();
            layout.Restore();
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
