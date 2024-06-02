using StudioLaValse.ScoreDocument.Implementation;
using StudioLaValse.ScoreDocument.Primitives;
using StudioLaValse.ScoreDocument.Reader;

namespace Sinfonia.Implementations.ScoreDocument
{
    public class NoteReaderProxy : INoteReader
    {
        private readonly Note source;



        public Pitch Pitch => source.Pitch;

        public bool Grace => source.Grace;

        public Position Position => source.Position;

        public RythmicDuration RythmicDuration => source.RythmicDuration;

        public Tuplet Tuplet => source.Tuplet;

        public int Id => source.Id;

        public Guid Guid => source.Guid;





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
            return $"Note : [{Guid}]";
        }

        public INoteLayout ReadLayout()
        {
            return source.AuthorLayout;
        }
    }
}
