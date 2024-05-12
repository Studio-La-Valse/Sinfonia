namespace Sinfonia.Implementations.ScoreDocument.Converters
{
    public class ScoreElementMementoConverter
    {
        public ScoreElementMementoConverter()
        {

        }

        public Instrument Convert(StudioLaValse.ScoreDocument.Models.Classes.Instrument instrument)
        {
            var _instrument = Instrument.CreateCustom(instrument.Name, instrument.Clefs.Select(Convert).ToArray());
            return _instrument;
        }
        public StudioLaValse.ScoreDocument.Models.Classes.Instrument ConvertBack(Instrument instrument)
        {
            var _instrument = new StudioLaValse.ScoreDocument.Models.Classes.Instrument()
            {
                Clefs = instrument.DefaultClefs.Select(ConvertBack).ToList(),
                Name = instrument.Name,
            };
            return _instrument;
        }

        public RythmicDuration Convert(StudioLaValse.ScoreDocument.Models.Classes.RythmicDuration rythmicDuration)
        {
            var _rythmicDuration = new RythmicDuration(rythmicDuration.PowerOfTwo, rythmicDuration.Dots);
            return _rythmicDuration;
        }
        public StudioLaValse.ScoreDocument.Models.Classes.RythmicDuration ConvertBack(RythmicDuration rythmicDuration)
        {
            var _rythmicDuration = new StudioLaValse.ScoreDocument.Models.Classes.RythmicDuration()
            {
                PowerOfTwo = rythmicDuration.PowerOfTwo,
                Dots = rythmicDuration.Dots
            };
            return _rythmicDuration;
        }

        public TimeSignature Convert(StudioLaValse.ScoreDocument.Models.Classes.TimeSignature timeSignature)
        {
            var _timeSignature = new TimeSignature(timeSignature.Numerator, timeSignature.Denominator);
            return _timeSignature;
        }
        public StudioLaValse.ScoreDocument.Models.Classes.TimeSignature ConvertBack(TimeSignature timeSignature)
        {
            var _timeSignature = new StudioLaValse.ScoreDocument.Models.Classes.TimeSignature()
            {
                Denominator = timeSignature.Denominator,
                Numerator = timeSignature.Numerator,
            };
            return _timeSignature;
        }

        public KeySignature Convert(StudioLaValse.ScoreDocument.Models.Classes.KeySignature keySignature)
        {
            var step = Convert(keySignature.Step);
            var _keySignature = new KeySignature(step, keySignature.Major ? MajorOrMinor.Major : MajorOrMinor.Minor);
            return _keySignature;
        }
        public StudioLaValse.ScoreDocument.Models.Classes.KeySignature ConvertBack(KeySignature keySignature)
        {
            var step = ConvertBack(keySignature.Origin);
            var major = keySignature.MajorOrMinor == MajorOrMinor.Major ? true : false;
            var _keySignature = new StudioLaValse.ScoreDocument.Models.Classes.KeySignature()
            {
                Major = major,
                Step = step,
            };
            return _keySignature;
        }

        public Clef Convert(string clef)
        {
            var _clef = clef.ToLower() switch
            {
                "treble" => Clef.Treble,
                "soprano" => Clef.Soprano,
                "mezzosoprano" => Clef.MezzoSoprano,
                "alto" => Clef.Alto,
                "tenor" => Clef.Tenor,
                "baritone" => Clef.Baritone,
                "bass" => Clef.Bass,
                _ => throw new NotImplementedException()
            };
            return _clef;
        }
        public string ConvertBack(Clef clef)
        {
            return clef.Name.ToString();
        }

        public Step Convert(StudioLaValse.ScoreDocument.Models.Classes.Step step)
        {
            var _step = new Step(step.StepsFromC, step.Shifts);
            return _step;
        }
        public StudioLaValse.ScoreDocument.Models.Classes.Step ConvertBack(Step step)
        {
            var _step = new StudioLaValse.ScoreDocument.Models.Classes.Step()
            {
                StepsFromC = step.StepsFromC,
                Shifts = step.Shifts
            };
            return _step;
        }

        public Pitch Convert(StudioLaValse.ScoreDocument.Models.Classes.Pitch pitch)
        {
            var step = Convert(pitch.Step);
            var _pitch = new Pitch(step, pitch.Octave);
            return _pitch;
        }
        public StudioLaValse.ScoreDocument.Models.Classes.Pitch ConvertBack(Pitch pitch)
        {
            var _pitch = new StudioLaValse.ScoreDocument.Models.Classes.Pitch()
            {
                Octave = pitch.Octave,
                Step = ConvertBack(pitch.Step)
            };
            return _pitch;
        }

        public Position Convert(StudioLaValse.ScoreDocument.Models.Classes.Position position)
        {
            var _position = new Position(position.Numerator, position.Denominator);
            return _position;
        }
        public StudioLaValse.ScoreDocument.Models.Classes.Position ConvertBack(Position position)
        {
            var _position = new StudioLaValse.ScoreDocument.Models.Classes.Position()
            {
                Denominator = position.Denominator,
                Numerator = position.Numerator,
            };
            return _position;
        }
    }
}
