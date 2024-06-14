using StudioLaValse.ScoreDocument;
using StudioLaValse.ScoreDocument.Reader;
using Chord = StudioLaValse.ScoreDocument.Implementation.Chord;

namespace Sinfonia.Implementations.ScoreDocument
{
    public class ChordReaderProxy : IChordReader
    {
        private readonly Chord source;




        public Position Position => source.Position;

        public RythmicDuration RythmicDuration => source.RythmicDuration;

        public Tuplet Tuplet => source.Tuplet;

        public int Id => source.Id;


        public ChordReaderProxy(Chord source)
        {
            this.source = source;
        }




        public IEnumerable<INoteReader> ReadNotes()
        {
            return source.EnumerateNotesCore().Select(e => e.ProxyReader());
        }

        public IEnumerable<IScoreElement> EnumerateChildren()
        {
            return ReadNotes();
        }

        public IEnumerable<KeyValuePair<PowerOfTwo, BeamType>> ReadBeamTypes()
        {
            return source.GetBeamTypes();
        }

        public BeamType? ReadBeamType(PowerOfTwo i)
        {
            return source.GetBeamType(i);
        }

        public override string ToString()
        {
            return $"Chord : [{Position}]";
        }

        public IChordLayout ReadLayout()
        {
            return source.AuthorLayout;
        }

        public IGraceGroupReader? ReadGraceGroup()
        {
            if(source.GraceGroup is null)
            {
                return null;
            }

            return new GraceGroupReaderProxy(source.GraceGroup);
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
