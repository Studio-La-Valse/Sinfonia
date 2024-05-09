#nullable disable

namespace StudioLaValse.ScoreDocument.Models.Classes
{
    public class Instrument
    {
        public string Name { get; set; }

        public ICollection<string> Clefs { get; set; }
    }
}

#nullable enable
