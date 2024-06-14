using StudioLaValse.ScoreDocument;
using StudioLaValse.ScoreDocument.Implementation;

namespace Sinfonia.Implementations.ScoreDocument
{
    public class GraceNoteReaderProxy : IGraceNoteReader
    {
        private readonly GraceNote graceNote;


        public Pitch Pitch => graceNote.Pitch;

        public int Id => graceNote.Id;



        public GraceNoteReaderProxy(GraceNote graceNote)
        {
            this.graceNote = graceNote;
        }


        public IEnumerable<IScoreElement> EnumerateChildren()
        {
            yield break;
        }

        public INoteLayout ReadLayout()
        {
            return graceNote.AuthorLayout;
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
