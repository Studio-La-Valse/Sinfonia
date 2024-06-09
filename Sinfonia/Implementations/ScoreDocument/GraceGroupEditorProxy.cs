using Sinfonia.Extensions;
using StudioLaValse.ScoreDocument;
using StudioLaValse.ScoreDocument.Implementation;
using StudioLaValse.ScoreDocument.Implementation.Layout;
using StudioLaValse.ScoreDocument.Models;
using StudioLaValse.ScoreDocument.Models.Base;

namespace Sinfonia.Implementations.ScoreDocument
{
    public class GraceGroupEditorProxy : IGraceGroupEditor
    {
        private readonly GraceGroup graceGroup;
        private readonly ICommandManager commandManager;
        private readonly INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged;

        public int Id => graceGroup.Id;

        public int Length => graceGroup.Length;

        public Position Target => graceGroup.Target;

        public GraceGroupEditorProxy(GraceGroup graceGroup, ICommandManager commandManager, INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged)
        {
            this.graceGroup = graceGroup;
            this.commandManager = commandManager;
            this.notifyEntityChanged = notifyEntityChanged;
        }



        public IEnumerable<IScoreElement> EnumerateChildren()
        {
            return ReadChords();
        }

        public IEnumerable<IGraceChordEditor> ReadChords()
        {
            return graceGroup.Chords.Select(c => c.ProxyEditor(commandManager, notifyEntityChanged));
        }

        public IGraceGroupLayout ReadLayout()
        {
            return graceGroup.AuthorLayout;
        }

        public void RemoveLayout()
        {
            var transaction = commandManager.ThrowIfNoTransactionOpen();
            var command = new RestoreLayoutCommand<AuthorGraceGroupLayout, GraceGroupLayoutMembers>(graceGroup.AuthorLayout).ThenInvalidate(notifyEntityChanged, graceGroup.HostMeasure);
            transaction.Enqueue(command);
        }

        public void SetBeamAngle(double angle)
        {
            var transaction = commandManager.ThrowIfNoTransactionOpen();
            var command = new MementoCommand<AuthorGraceGroupLayout, GraceGroupLayoutMembers>(graceGroup.AuthorLayout, s => s.BeamAngle = angle).ThenInvalidate(notifyEntityChanged, graceGroup.HostMeasure);
            transaction.Enqueue(command);
        }

        public void SetStemLength(double stemLength)
        {
            var transaction = commandManager.ThrowIfNoTransactionOpen();
            var command = new MementoCommand<AuthorGraceGroupLayout, GraceGroupLayoutMembers>(graceGroup.AuthorLayout, s => s.StemLength = stemLength).ThenInvalidate(notifyEntityChanged, graceGroup.HostMeasure);
            transaction.Enqueue(command);
        }

        public void Splice(int index)
        {
            var transaction = commandManager.ThrowIfNoTransactionOpen();
            var command = new MementoCommand<GraceGroup, GraceGroupModel>(graceGroup, (s) => s.Splice(index)).ThenInvalidate(notifyEntityChanged, graceGroup.HostMeasure);
            transaction.Enqueue(command);
        }

        public void AppendChord(params Pitch[] pitches)
        {
            var transaction = commandManager.ThrowIfNoTransactionOpen();
            var command = new MementoCommand<GraceGroup, GraceGroupModel>(graceGroup, (s) => s.Append(pitches)).ThenInvalidate(notifyEntityChanged, graceGroup.HostMeasure);
            transaction.Enqueue(command);
        }

        public void Clear()
        {
            var transaction = commandManager.ThrowIfNoTransactionOpen();
            var command = new MementoCommand<GraceGroup, GraceGroupModel>(graceGroup, (s) => s.Clear()).ThenInvalidate(notifyEntityChanged, graceGroup.HostMeasure);
            transaction.Enqueue(command);
        }

    }
}
