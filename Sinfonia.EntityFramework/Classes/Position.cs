#nullable disable

using Sinfonia.EntityFramework.Attributes;

namespace Sinfonia.EntityFramework.Classes
{
    public class Position
    {
        public int Numerator { get; set; }

        [PowerOfTwo]
        public int Denominator { get; set; }
    }
}

#nullable enable
