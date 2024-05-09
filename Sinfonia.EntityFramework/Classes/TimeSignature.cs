#nullable disable

using Sinfonia.EntityFramework.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Sinfonia.EntityFramework.Classes
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
