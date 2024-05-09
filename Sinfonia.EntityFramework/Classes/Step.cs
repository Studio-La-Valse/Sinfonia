#nullable disable

using System.ComponentModel.DataAnnotations;

namespace StudioLaValse.ScoreDocument.Models.Classes
{
    public class Step
    {
        [Range(0, 12)]
        public int StepsFromC { get; set; }

        [Range(-3, 4)]
        public int Shifts { get; set; }
    }
}

#nullable enable
