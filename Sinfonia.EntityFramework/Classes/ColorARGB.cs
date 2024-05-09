#nullable disable

using Sinfonia;
using System.ComponentModel.DataAnnotations;

namespace Sinfonia.EntityFramework.Classes
{
    public class ColorARGB
    {
        [Range(0, 255)]
        public int R { get; set; }

        [Range(0, 255)]
        public int G { get; set; }

        [Range(0, 255)]
        public int B { get; set; }

        [Range(0, 255)]
        public int A { get; set; }
    }
}

#nullable enable
