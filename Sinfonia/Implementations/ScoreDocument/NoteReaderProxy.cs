using StudioLaValse.ScoreDocument;
using StudioLaValse.ScoreDocument.Implementation;
using StudioLaValse.ScoreDocument.Reader;

namespace Sinfonia.Implementations.ScoreDocument
{
    public class NoteReaderProxy : INoteReader
    {
        private readonly Note source;



        public Pitch Pitch => source.Pitch;

        public Position Position => source.Position;

        public RythmicDuration RythmicDuration => source.RythmicDuration;

        public Tuplet Tuplet => source.Tuplet;

        public int Id => source.Id;




        public NoteReaderProxy(Note source)
        {
            this.source = source;
        }



        public IEnumerable<IScoreElement> EnumerateChildren()
        {
            yield break;
        }


        public override string ToString()
        {
            return $"Note : [{Pitch}]";
        }

        public INoteLayout ReadLayout()
        {
            return source.AuthorLayout;
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
