
using Sinfonia.Implementations.Commands;
using Sinfonia.Implementations.ScoreDocument.Layout;
using Sinfonia.Implementations.ScoreDocument.Memento.Layout;
using Sinfonia.Implementations.ScoreDocument.Proxy.Reader;
using StudioLaValse.ScoreDocument.Core;

namespace Sinfonia.Implementations.ScoreDocument.Proxy.Editor
{
    internal class ChordEditorProxy : IChordEditor, IUniqueScoreElement
    {
        private readonly Chord source;
        private readonly ICommandManager commandManager;
        private readonly INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged;


        public bool Grace => source.Grace;

        public Position Position => source.Position;

        public RythmicDuration RythmicDuration => source.RythmicDuration;

        public Tuplet Tuplet => source.Tuplet;

        public Guid Guid => source.Guid;

        public int Id => source.Id;


        public ChordEditorProxy(Chord source, ICommandManager commandManager, INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged)
        {
            this.source = source;
            this.commandManager = commandManager;
            this.notifyEntityChanged = notifyEntityChanged;
        }



        public void Add(params Pitch[] pitches)
        {
            var transaction = commandManager.ThrowIfNoTransactionOpen();
            var command = new MementoCommand<Chord, ChordMemento>(source, s => s.Add(pitches)).ThenInvalidate(notifyEntityChanged, source.HostMeasure);
            transaction.Enqueue(command);
        }

        public void Set(params Pitch[] pitches)
        {
            var transaction = commandManager.ThrowIfNoTransactionOpen();
            var command = new MementoCommand<Chord, ChordMemento>(source, s => s.Set(pitches)).ThenInvalidate(notifyEntityChanged, source.HostMeasure);
            transaction.Enqueue(command);
        }

        public void Clear()
        {
            var transaction = commandManager.ThrowIfNoTransactionOpen();
            var command = new MementoCommand<Chord, ChordMemento>(source, s => s.Clear()).ThenInvalidate(notifyEntityChanged, source.HostMeasure);
            transaction.Enqueue(command);
        }

        public IEnumerable<INoteEditor> ReadNotes()
        {
            return source.EnumerateNotesCore().Select(n => n.ProxyEditor(commandManager, notifyEntityChanged));
        }

        public IEnumerable<IScoreElement> EnumerateChildren()
        {
            return ReadNotes();
        }

        public IChordLayout ReadLayout()
        {
            return source.Layout;
        }

        public void RemoveLayout()
        {
            var transaction = commandManager.ThrowIfNoTransactionOpen();
            var command = new RestoreLayoutCommand<ChordLayout, ChordLayoutMemento>(source.Layout).ThenInvalidate(notifyEntityChanged, source.HostMeasure);
            transaction.Enqueue(command);
        }


        public void SetXOffset(double offset)
        {
            var transaction = commandManager.ThrowIfNoTransactionOpen();
            var command = new MementoCommand<Chord, ChordMemento>(source, s => s.Layout.XOffset = offset).ThenInvalidate(notifyEntityChanged, source.HostMeasure);
            transaction.Enqueue(command);
        }
    }
}
