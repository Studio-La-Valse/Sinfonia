using StudioLaValse.ScoreDocument;
using StudioLaValse.ScoreDocument.Implementation;

namespace Sinfonia.Implementations.ScoreDocument
{
    public class GraceGroupReaderProxy : IGraceGroupReader
    {
        private readonly GraceGroup graceGroup;


        public int Id => graceGroup.Id;

        public int Length => graceGroup.Length;

        public Position Target => throw new NotImplementedException();

        public GraceGroupReaderProxy(GraceGroup graceGroup)
        {
            this.graceGroup = graceGroup;
        }




        public IEnumerable<IScoreElement> EnumerateChildren()
        {
            return ReadChords();
        }

        public IEnumerable<IGraceChordReader> ReadChords()
        {
            return graceGroup.Chords.Select(c => new GraceChordReaderProxy(c));
        }

        public IGraceGroupLayout ReadLayout()
        {
            return graceGroup.AuthorLayout;
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
