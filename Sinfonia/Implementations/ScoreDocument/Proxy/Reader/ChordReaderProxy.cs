namespace Sinfonia.Implementations.ScoreDocument.Proxy.Reader
{
    internal class ChordReaderProxy : IChordReader
    {
        private readonly Chord source;




        public bool Grace => source.Grace;

        public Position Position => source.Position;

        public RythmicDuration RythmicDuration => source.RythmicDuration;

        public Tuplet Tuplet => source.Tuplet;

        public int Id => source.Id;

        public Guid Guid => source.Guid;





        public ChordReaderProxy(Chord source)
        {
            this.source = source;
        }




        public IEnumerable<INoteReader> ReadNotes()
        {
            return source.EnumerateNotesCore().Select(e => e.Proxy());
        }

        public IEnumerable<IScoreElement> EnumerateChildren()
        {
            return ReadNotes();
        }

        public IEnumerable<(BeamType beam, PowerOfTwo duration)> ReadBeamTypes()
        {
            return source.GetBeamTypes();
        }

        public BeamType? ReadBeamType(PowerOfTwo i)
        {
            return source.GetBeamType(i);
        }
    }
}
