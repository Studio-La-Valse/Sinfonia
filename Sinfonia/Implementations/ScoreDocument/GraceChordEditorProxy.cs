using Sinfonia.Extensions;
using StudioLaValse.ScoreDocument;
using StudioLaValse.ScoreDocument.Implementation;
using StudioLaValse.ScoreDocument.Implementation.Layout;
using StudioLaValse.ScoreDocument.Models;
using StudioLaValse.ScoreDocument.Models.Base;

namespace Sinfonia.Implementations.ScoreDocument
{
    public class GraceChordEditorProxy : IGraceChord
    {
        private readonly GraceChord graceChord;
        private readonly ICommandManager commandManager;
        private readonly INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged;


        public int Id => graceChord.Id;

        public int IndexInGroup => graceChord.IndexInGroup;

        public ReadonlyTemplateProperty<double> SpaceRight => ReadLayout().SpaceRight;



        public GraceChordEditorProxy(GraceChord graceChord, ICommandManager commandManager, INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged)
        {
            this.graceChord = graceChord;
            this.commandManager = commandManager;
            this.notifyEntityChanged = notifyEntityChanged;
        }


        public IEnumerable<IScoreElement> EnumerateChildren()
        {
            return ReadNotes();
        }

        public BeamType? ReadBeamType(PowerOfTwo i)
        {
            graceChord.BeamTypes.TryGetValue(i, out var type);
            return type;
        }

        public IEnumerable<KeyValuePair<PowerOfTwo, BeamType>> ReadBeamTypes()
        {
            return graceChord.BeamTypes;
        }

        public IEnumerable<IGraceNote> ReadNotes()
        {
            return graceChord.EnumerateNotes().Select(n => n.ProxyEditor(commandManager, notifyEntityChanged));
        }

        public IGraceGroup? ReadGraceGroup()
        {
            if (graceChord.GraceGroup is null)
            {
                return null;
            }

            return graceChord.GraceGroup.ProxyEditor(commandManager, notifyEntityChanged);
        }

        public void Clear()
        {
            var transaction = commandManager.ThrowIfNoTransactionOpen();
            var command = new MementoCommand<GraceChord, GraceChordModel>(graceChord, s => s.Clear()).ThenInvalidate(notifyEntityChanged, graceChord.HostMeasure);
            transaction.Enqueue(command);
        }

        public void Add(params Pitch[] pitches)
        {
            var transaction = commandManager.ThrowIfNoTransactionOpen();
            var command = new MementoCommand<GraceChord, GraceChordModel>(graceChord, s => s.Add(pitches)).ThenInvalidate(notifyEntityChanged, graceChord.HostMeasure);
            transaction.Enqueue(command);
        }

        public void Set(params Pitch[] pitches)
        {
            var transaction = commandManager.ThrowIfNoTransactionOpen();
            var command = new MementoCommand<GraceChord, GraceChordModel>(graceChord, s => s.Set(pitches)).ThenInvalidate(notifyEntityChanged, graceChord.HostMeasure);
            transaction.Enqueue(command);
        }

        public void Grace(RythmicDuration rythmicDuration, params Pitch[] pitches)
        {
            var transaction = commandManager.ThrowIfNoTransactionOpen();
            var command = new MementoCommand<GraceChord, GraceChordModel>(graceChord, s => s.Grace(rythmicDuration, pitches)).ThenInvalidate(notifyEntityChanged, graceChord.HostMeasure);
            transaction.Enqueue(command);
        }

        public void Restore()
        {
            var transaction = commandManager.ThrowIfNoTransactionOpen();
            var command = new RestoreLayoutCommand<AuthorGraceChordLayout, GraceChordLayoutMembers>(graceChord.AuthorLayout).ThenInvalidate(notifyEntityChanged, graceChord.HostMeasure);
            transaction.Enqueue(command);
        }

        public IGraceChordLayout ReadLayout()
        {
            return graceChord.AuthorLayout;
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
