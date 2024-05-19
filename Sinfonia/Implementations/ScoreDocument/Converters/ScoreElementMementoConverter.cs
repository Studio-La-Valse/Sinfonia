using ColorARGB = StudioLaValse.ScoreDocument.Layout.Templates.ColorARGB;

namespace Sinfonia.Implementations.ScoreDocument.Converters
{
    public static class ScoreElementMementoConverter
    {
        public static Instrument Convert(this StudioLaValse.ScoreDocument.Models.Classes.Instrument instrument)
        {
            var _instrument = Instrument.CreateCustom(instrument.Name, instrument.Clefs.Select(Convert).ToArray());
            return _instrument;
        }
        public static StudioLaValse.ScoreDocument.Models.Classes.Instrument Convert(this Instrument instrument)
        {
            var _instrument = new StudioLaValse.ScoreDocument.Models.Classes.Instrument()
            {
                Clefs = instrument.DefaultClefs.Select(Convert).ToList(),
                Name = instrument.Name,
            };
            return _instrument;
        }

        public static RythmicDuration Convert(this StudioLaValse.ScoreDocument.Models.Classes.RythmicDuration rythmicDuration)
        {
            var _rythmicDuration = new RythmicDuration(rythmicDuration.PowerOfTwo, rythmicDuration.Dots);
            return _rythmicDuration;
        }
        public static StudioLaValse.ScoreDocument.Models.Classes.RythmicDuration Convert(this RythmicDuration rythmicDuration)
        {
            var _rythmicDuration = new StudioLaValse.ScoreDocument.Models.Classes.RythmicDuration()
            {
                PowerOfTwo = rythmicDuration.PowerOfTwo,
                Dots = rythmicDuration.Dots
            };
            return _rythmicDuration;
        }

        public static TimeSignature Convert(this StudioLaValse.ScoreDocument.Models.Classes.TimeSignature timeSignature)
        {
            var _timeSignature = new TimeSignature(timeSignature.Numerator, timeSignature.Denominator);
            return _timeSignature;
        }
        public static StudioLaValse.ScoreDocument.Models.Classes.TimeSignature Convert(this TimeSignature timeSignature)
        {
            var _timeSignature = new StudioLaValse.ScoreDocument.Models.Classes.TimeSignature()
            {
                Denominator = timeSignature.Denominator,
                Numerator = timeSignature.Numerator,
            };
            return _timeSignature;
        }

        public static KeySignature Convert(this StudioLaValse.ScoreDocument.Models.Classes.KeySignature keySignature)
        {
            var step = Convert(keySignature.Step);
            var _keySignature = new KeySignature(step, keySignature.Major ? MajorOrMinor.Major : MajorOrMinor.Minor);
            return _keySignature;
        }
        public static StudioLaValse.ScoreDocument.Models.Classes.KeySignature Convert(this KeySignature keySignature)
        {
            var step = Convert(keySignature.Origin);
            var major = keySignature.MajorOrMinor == MajorOrMinor.Major ? true : false;
            var _keySignature = new StudioLaValse.ScoreDocument.Models.Classes.KeySignature()
            {
                Major = major,
                Step = step,
            };
            return _keySignature;
        }

        public static Clef Convert(this string clef)
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
        public static string Convert(this Clef clef)
        {
            return clef.Name.ToString();
        }


        public static ClefChange Convert(this StudioLaValse.ScoreDocument.Models.Classes.ClefChange clefChange)
        {
            var _clefChange = new ClefChange(clefChange.Clef.Convert(), clefChange.StaffIndex, clefChange.Position.Convert());
            return _clefChange;
        }
        public static StudioLaValse.ScoreDocument.Models.Classes.ClefChange Convert(this ClefChange clefChange)
        {
            var _clefChange = new StudioLaValse.ScoreDocument.Models.Classes.ClefChange()
            {
                Clef = clefChange.Clef.Convert(),
                Position = clefChange.Position.Convert(),
                StaffIndex = clefChange.StaffIndex
            };
            return _clefChange;
        }


        public static Step Convert(this StudioLaValse.ScoreDocument.Models.Classes.Step step)
        {
            var _step = new Step(step.StepsFromC, step.Shifts);
            return _step;
        }
        public static StudioLaValse.ScoreDocument.Models.Classes.Step Convert(this Step step)
        {
            var _step = new StudioLaValse.ScoreDocument.Models.Classes.Step()
            {
                StepsFromC = step.StepsFromC,
                Shifts = step.Shifts
            };
            return _step;
        }

        public static Pitch Convert(this StudioLaValse.ScoreDocument.Models.Classes.Pitch pitch)
        {
            var step = Convert(pitch.Step);
            var _pitch = new Pitch(step, pitch.Octave);
            return _pitch;
        }
        public static StudioLaValse.ScoreDocument.Models.Classes.Pitch Convert(this Pitch pitch)
        {
            var _pitch = new StudioLaValse.ScoreDocument.Models.Classes.Pitch()
            {
                Octave = pitch.Octave,
                Step = Convert(pitch.Step)
            };
            return _pitch;
        }

        public static Position Convert(this StudioLaValse.ScoreDocument.Models.Classes.Position position)
        {
            var _position = new Position(position.Numerator, position.Denominator);
            return _position;
        }
        public static StudioLaValse.ScoreDocument.Models.Classes.Position Convert(this Position position)
        {
            var _position = new StudioLaValse.ScoreDocument.Models.Classes.Position()
            {
                Denominator = position.Denominator,
                Numerator = position.Numerator,
            };
            return _position;
        }

        public static ColorARGB Convert(this StudioLaValse.ScoreDocument.Models.Classes.ColorARGB Color)
        {
            var _color = new ColorARGB()
            {
                A = Color.A,
                B = Color.B,
                G = Color.G,
                R = Color.R,
            };
            return _color;
        }
        public static StudioLaValse.ScoreDocument.Models.Classes.ColorARGB Convert(this ColorARGB Color)
        {
            var _color = new StudioLaValse.ScoreDocument.Models.Classes.ColorARGB()
            {
                A = Color.A,
                B = Color.B,
                G = Color.G,
                R = Color.R,
            };
            return _color;
        }
    }
}
