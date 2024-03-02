namespace Sinfonia.Implementations.ScoreDocument.Proxy.Reader
{
    internal class NoteReaderProxy : INoteReader
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
    }
}
