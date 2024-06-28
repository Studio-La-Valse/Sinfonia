using StudioLaValse.ScoreDocument;
using StudioLaValse.ScoreDocument.Core;
using StudioLaValse.ScoreDocument.Implementation.Layout;
using StudioLaValse.ScoreDocument.Models;
using StudioLaValse.ScoreDocument.Models.Base;
using Chord = StudioLaValse.ScoreDocument.Implementation.Chord;

namespace Sinfonia.Implementations.ScoreDocument
{
    public class ChordEditorProxy : IChord
    {
        private readonly Chord source;
        private readonly ICommandManager commandManager;
        private readonly INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged;


        public Position Position => source.Position;

        public RythmicDuration RythmicDuration => source.RythmicDuration;

        public Tuplet Tuplet => source.Tuplet;

        public int Id => source.Id;

        public IChordLayout ReadLayout => source.AuthorLayout;

        public TemplateProperty<double> XOffset => ReadLayout.XOffset.UseCommandManager(commandManager).ThenInvalidate(notifyEntityChanged, source.HostMeasure);

        public TemplateProperty<double> SpaceRight => ReadLayout.SpaceRight.UseCommandManager(commandManager).ThenInvalidate(notifyEntityChanged, source.HostMeasure);


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
        public void Grace(RythmicDuration rythmicDuration, params Pitch[] pitches)
        {
            var transaction = commandManager.ThrowIfNoTransactionOpen();
            var command = new MementoCommand<Chord, ChordModel>(source, s => s.ApplyGrace(rythmicDuration, pitches)).ThenInvalidate(notifyEntityChanged, source.HostMeasure);
            transaction.Enqueue(command);
        }
        public void Clear()
        {
            var transaction = commandManager.ThrowIfNoTransactionOpen();
            var command = new MementoCommand<Chord, ChordModel>(source, s => s.Clear()).ThenInvalidate(notifyEntityChanged, source.HostMeasure);
            transaction.Enqueue(command);
        }

        public void Restore()
        {
            var transaction = commandManager.ThrowIfNoTransactionOpen();
            var command = new RestoreLayoutCommand<AuthorChordLayout, ChordLayoutMembers>(source.AuthorLayout).ThenInvalidate(notifyEntityChanged, source.HostMeasure);
            transaction.Enqueue(command);
        }


        public IEnumerable<INote> ReadNotes()
        {
            return source.EnumerateNotesCore().Select(n => n.ProxyEditor(commandManager, notifyEntityChanged));
        }

        public IEnumerable<IScoreElement> EnumerateChildren()
        {
            return ReadNotes();
        }

        public IGraceGroup? ReadGraceGroup()
        {
            if(source.GraceGroup is null)
            {
                return null;
            }

            return source.GraceGroup.ProxyEditor(commandManager, notifyEntityChanged);
        }

        public IEnumerable<KeyValuePair<PowerOfTwo, BeamType>> ReadBeamTypes()
        {
            return source.AuthorLayout.ReadBeamTypes();
        }

        public BeamType? ReadBeamType(PowerOfTwo i)
        {
            return source.AuthorLayout.ReadBeamType(i);
        }

        public bool Equals(IUniqueScoreElement? other)
        {
            if (other is null)
            {
                return false;
            }

            return other.Id == Id;
        }
    }
}
