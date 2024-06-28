using StudioLaValse.ScoreDocument;
using StudioLaValse.ScoreDocument.Implementation;
using StudioLaValse.ScoreDocument.Implementation.Layout;
using StudioLaValse.ScoreDocument.Layout;
using StudioLaValse.ScoreDocument.Models;
using StudioLaValse.ScoreDocument.Models.Base;
using YamlDotNet.Core.Tokens;

namespace Sinfonia.Implementations.ScoreDocument
{
    public class GraceGroupEditorProxy : IGraceGroup
    {
        private readonly GraceGroup graceGroup;
        private readonly ICommandManager commandManager;
        private readonly INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged;

        public int Id => graceGroup.Id;

        public int Length => graceGroup.Length;

        public Position Target => graceGroup.Target;

        public TemplateProperty<RythmicDuration> BlockDuration => graceGroup.AuthorLayout.ChordDuration.UseCommandManager(commandManager).ThenInvalidate(notifyEntityChanged, graceGroup.HostMeasure);

        public TemplateProperty<bool> OccupySpace => graceGroup.AuthorLayout.OccupySpace.UseCommandManager(commandManager).ThenInvalidate(notifyEntityChanged, graceGroup.HostMeasure);

        public TemplateProperty<double> ChordSpacing => graceGroup.AuthorLayout.ChordSpacing.UseCommandManager(commandManager).ThenInvalidate(notifyEntityChanged, graceGroup.HostMeasure);

        public TemplateProperty<RythmicDuration> ChordDuration => graceGroup.AuthorLayout.ChordDuration.UseCommandManager(commandManager).ThenInvalidate(notifyEntityChanged, graceGroup.HostMeasure);

        public TemplateProperty<double> Scale => graceGroup.AuthorLayout.Scale.UseCommandManager(commandManager).ThenInvalidate(notifyEntityChanged, graceGroup.HostMeasure);

        public TemplateProperty<StemDirection> StemDirection => graceGroup.AuthorLayout.StemDirection.UseCommandManager(commandManager).ThenInvalidate(notifyEntityChanged, graceGroup.HostMeasure);

        public TemplateProperty<double> StemLength => graceGroup.AuthorLayout.StemLength.UseCommandManager(commandManager).ThenInvalidate(notifyEntityChanged, graceGroup.HostMeasure);

        public TemplateProperty<double> BeamAngle => graceGroup.AuthorLayout.BeamAngle.UseCommandManager(commandManager).ThenInvalidate(notifyEntityChanged, graceGroup.HostMeasure);

        public ReadonlyTemplateProperty<double> BeamThickness => graceGroup.AuthorLayout.BeamThickness;

        public ReadonlyTemplateProperty<double> BeamSpacing => graceGroup.AuthorLayout.BeamSpacing;


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

        public IEnumerable<IGraceChord> ReadChords()
        {
            return graceGroup.Chords.Select(c => c.ProxyEditor(commandManager, notifyEntityChanged));
        }

        public IGraceGroupLayout ReadLayout()
        {
            return graceGroup.AuthorLayout;
        }

        public void Restore()
        {
            var transaction = commandManager.ThrowIfNoTransactionOpen();
            var command = new RestoreLayoutCommand<AuthorGraceGroupLayout, GraceGroupLayoutMembers>(graceGroup.AuthorLayout).ThenInvalidate(notifyEntityChanged, graceGroup.HostMeasure);
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

        public bool Equals(IUniqueScoreElement? other)
        {
            return other is not null && other.Id == Id;
        }
    }
}
