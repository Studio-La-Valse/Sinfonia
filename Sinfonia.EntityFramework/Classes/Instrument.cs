#nullable disable

namespace Sinfonia.EntityFramework.Classes
{
    public class Instrument
    {
        public string Name { get; set; }

        public ICollection<string> Clefs { get; set; }
    }
}

#nullable enable
