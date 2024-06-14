using StudioLaValse.ScoreDocument;
using StudioLaValse.ScoreDocument.Implementation;

namespace Sinfonia.Implementations.ScoreDocument
{
    public class GraceChordReaderProxy : IGraceChordReader
    {
        private readonly GraceChord graceChord;


        public int Id => graceChord.Id;

        public int IndexInGroup => graceChord.IndexInGroup;

        public GraceChordReaderProxy(GraceChord graceChord)
        {
            this.graceChord = graceChord;
        }


        public IEnumerable<IScoreElement> EnumerateChildren()
        {
            return graceChord.EnumerateNotes().Select(n => new GraceNoteReaderProxy(n));
        }

        public BeamType? ReadBeamType(PowerOfTwo i)
        {
            graceChord.BeamTypes.TryGetValue(i, out var type);
            return type;
        }

        public IEnumerable<KeyValuePair<PowerOfTwo, BeamType>> ReadBeamTypes()
        {
            return graceChord.BeamTypes;
        }

        public IEnumerable<IGraceNoteReader> ReadNotes()
        {
            return graceChord.EnumerateNotes().Select(n => new GraceNoteReaderProxy(n));
        }

        public IGraceGroupReader? ReadGraceGroup()
        {
            if(graceChord.GraceGroup is null)
            {
                return null;
            }

            return new GraceGroupReaderProxy(graceChord.GraceGroup);
        }

        public IChordLayout ReadLayout()
        {
            return graceChord.AuthorLayout;
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
