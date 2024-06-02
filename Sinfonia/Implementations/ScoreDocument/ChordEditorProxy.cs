using StudioLaValse.ScoreDocument;
using StudioLaValse.ScoreDocument.Implementation.Layout;
using StudioLaValse.ScoreDocument.Models;
using StudioLaValse.ScoreDocument.Models.Base;
using Chord = StudioLaValse.ScoreDocument.Implementation.Chord;

namespace Sinfonia.Implementations.ScoreDocument
{
    public class ChordEditorProxy : IChordEditor
    {
        private readonly Chord source;
        private readonly ICommandManager commandManager;
        private readonly INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged;


        public bool Grace => source.Grace;

        public Position Position => source.Position;

        public RythmicDuration RythmicDuration => source.RythmicDuration;

        public Tuplet Tuplet => source.Tuplet;

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
            var command = new MementoCommand<Chord, ChordModel>(source, s => s.Add(pitches)).ThenInvalidate(notifyEntityChanged, source.HostMeasure);
            transaction.Enqueue(command);
        }

        public void Set(params Pitch[] pitches)
        {
            var transaction = commandManager.ThrowIfNoTransactionOpen();
            var command = new MementoCommand<Chord, ChordModel>(source, s => s.Set(pitches)).ThenInvalidate(notifyEntityChanged, source.HostMeasure);
            transaction.Enqueue(command);
        }

        public void Clear()
        {
            var transaction = commandManager.ThrowIfNoTransactionOpen();
            var command = new MementoCommand<Chord, ChordModel>(source, s => s.Clear()).ThenInvalidate(notifyEntityChanged, source.HostMeasure);
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
            return source.AuthorLayout;
        }

        public void RemoveLayout()
        {
            var transaction = commandManager.ThrowIfNoTransactionOpen();
            var command = new RestoreLayoutCommand<AuthorChordLayout, ChordLayoutMembers>(source.AuthorLayout).ThenInvalidate(notifyEntityChanged, source.HostMeasure);
            transaction.Enqueue(command);
        }


        public void SetXOffset(double offset)
        {
            var transaction = commandManager.ThrowIfNoTransactionOpen();
            var command = new MementoCommand<AuthorChordLayout, ChordLayoutMembers>(source.AuthorLayout, s => s.XOffset = offset).ThenInvalidate(notifyEntityChanged, source.HostMeasure);
            transaction.Enqueue(command);
        }

        public void SetSpaceRight(double spaceRight)
        {
            var transaction = commandManager.ThrowIfNoTransactionOpen();
            var command = new MementoCommand<AuthorChordLayout, ChordLayoutMembers>(source.AuthorLayout, s => s.SpaceRight = spaceRight).ThenInvalidate(notifyEntityChanged, source.HostMeasure);
            transaction.Enqueue(command);
        }
    }
}
