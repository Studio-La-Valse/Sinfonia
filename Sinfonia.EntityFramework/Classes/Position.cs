#nullable disable

using StudioLaValse.ScoreDocument.Models.Attributes;

namespace StudioLaValse.ScoreDocument.Models.Classes
{
    public class Position
    {
        public int Numerator { get; set; }

        [PowerOfTwo]
        public int Denominator { get; set; }
    }
}

#nullable enable
