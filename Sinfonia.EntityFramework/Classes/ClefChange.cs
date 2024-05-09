#nullable disable

using Sinfonia;
using System.ComponentModel.DataAnnotations;

namespace Sinfonia.EntityFramework.Classes
{
    public class ClefChange
    {
        public string Clef { get; set; }

        [Range(0, int.MaxValue)]
        public int StaffIndex { get; set; }

        public Position Position { get; set; }
    }
}

#nullable enable
