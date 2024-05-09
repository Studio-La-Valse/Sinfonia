#nullable disable

using StudioLaValse.ScoreDocument.Models.Attributes;
using System.ComponentModel.DataAnnotations;

namespace StudioLaValse.ScoreDocument.Models.Classes
{
    public class RythmicDuration
    {
        [PowerOfTwo]
        [Range(1, int.MaxValue)]
        public int PowerOfTwo { get; set; }

        [Range(0, int.MaxValue)]
        public int Dots { get; set; }
    }
}

#nullable enable
