using StudioLaValse.ScoreDocument;
using StudioLaValse.ScoreDocument.Implementation;
using StudioLaValse.ScoreDocument.Implementation.Layout;
using StudioLaValse.ScoreDocument.Models.Base;

namespace Sinfonia.Implementations.ScoreDocument
{
    public class GraceNoteEditorProxy : IGraceNoteEditor
    {
        private readonly GraceNote graceNote;
        private readonly ICommandManager commandManager;
        private readonly INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged;

        public Pitch Pitch => graceNote.Pitch;

        public int Id => graceNote.Id;



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

        public void RemoveLayout()
        {
            var transaction = commandManager.ThrowIfNoTransactionOpen();
            var command = new RestoreLayoutCommand<AuthorGraceNoteLayout, GraceNoteLayoutMembers>(graceNote.AuthorLayout).ThenInvalidate(notifyEntityChanged, graceNote.InstrumentMeasure);
            transaction.Enqueue(command);
        }

        public INoteLayout ReadLayout()
        {
            return graceNote.AuthorLayout;
        }

        public void SetStaffIndex(int staffIndex)
        {
            var transaction = commandManager.ThrowIfNoTransactionOpen();
            var command = new MementoCommand<AuthorGraceNoteLayout, GraceNoteLayoutMembers>(graceNote.AuthorLayout, s => s.StaffIndex = staffIndex).ThenInvalidate(notifyEntityChanged, graceNote.InstrumentMeasure.HostMeasure.HostDocument);
            transaction.Enqueue(command);
        }
    }
}
