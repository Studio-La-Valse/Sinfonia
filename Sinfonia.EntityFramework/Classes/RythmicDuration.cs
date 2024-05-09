#nullable disable

using Sinfonia.EntityFramework.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Sinfonia.EntityFramework.Classes
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
