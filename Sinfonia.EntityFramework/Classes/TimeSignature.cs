#nullable disable

using StudioLaValse.ScoreDocument.Models.Attributes;
using System.ComponentModel.DataAnnotations;

namespace StudioLaValse.ScoreDocument.Models.Classes
{
    public class TimeSignature
    {
        [Range(1, int.MaxValue)]
        public int Numerator { get; set; }

        [PowerOfTwo]
        public int Denominator { get; set; }
    }
}

#nullable enable
