using StudioLaValse.ScoreDocument;
using StudioLaValse.ScoreDocument.Implementation;
using StudioLaValse.ScoreDocument.Implementation.Layout;
using StudioLaValse.ScoreDocument.Models;
using StudioLaValse.ScoreDocument.Models.Base;
using YamlDotNet.Core.Tokens;

namespace Sinfonia.Implementations.ScoreDocument
{
    public class GraceNoteEditorProxy : IGraceNote
    {
        private readonly GraceNote graceNote;
        private readonly ICommandManager commandManager;
        private readonly INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged;

        public int Id => graceNote.Id;

        public IGraceNoteLayout AuthorLayout => graceNote.AuthorLayout;

        public ReadonlyTemplateProperty<double> Scale => AuthorLayout.Scale;

        public ReadonlyTemplateProperty<double> XOffset => AuthorLayout.XOffset;

        public Pitch Pitch
        {
            get => graceNote.Pitch;
            set
            {
                var transaction = commandManager.ThrowIfNoTransactionOpen();
                var command = new MementoCommand<GraceNote, GraceNoteModel>(graceNote, s => s.Pitch = value).ThenInvalidate(notifyEntityChanged, graceNote.InstrumentMeasure.HostMeasure.HostDocument);
                transaction.Enqueue(command);
            }
        }

        public TemplateProperty<AccidentalDisplay> ForceAccidental => AuthorLayout.ForceAccidental.UseCommandManager(commandManager).ThenInvalidate(notifyEntityChanged, graceNote.InstrumentMeasure);

        public TemplateProperty<int> StaffIndex => AuthorLayout.StaffIndex.UseCommandManager(commandManager).ThenInvalidate(notifyEntityChanged, graceNote.InstrumentMeasure.HostMeasure.HostDocument);



        public GraceNoteEditorProxy(GraceNote graceNote, ICommandManager commandManager, INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged)
        {
            this.graceNote = graceNote;
            this.commandManager = commandManager;
            this.notifyEntityChanged = notifyEntityChanged;
        }


        public IEnumerable<IScoreElement> EnumerateChildren()
        {
            yield break;
        }

        public void SetPitch(Pitch pitch)
        {
            graceNote.Pitch = pitch;
        }

        public void ResetAccidental()
        {
            var transaction = commandManager.ThrowIfNoTransactionOpen();
            var command = new MementoCommand<AuthorGraceNoteLayout, GraceNoteLayoutMembers>(graceNote.AuthorLayout, s => s.ResetAccidental()).ThenInvalidate(notifyEntityChanged, graceNote.InstrumentMeasure.HostMeasure.HostDocument);
            transaction.Enqueue(command);
        }

        public void ResetStaffIndex()
        {
            var transaction = commandManager.ThrowIfNoTransactionOpen();
            var command = new MementoCommand<AuthorGraceNoteLayout, GraceNoteLayoutMembers>(graceNote.AuthorLayout, s => s.ResetStaffIndex()).ThenInvalidate(notifyEntityChanged, graceNote.InstrumentMeasure.HostMeasure.HostDocument);
            transaction.Enqueue(command);
        }

        public void Restore()
        {
            var transaction = commandManager.ThrowIfNoTransactionOpen();
            var command = new RestoreLayoutCommand<AuthorGraceNoteLayout, GraceNoteLayoutMembers>(graceNote.AuthorLayout).ThenInvalidate(notifyEntityChanged, graceNote.InstrumentMeasure);
            transaction.Enqueue(command);
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
