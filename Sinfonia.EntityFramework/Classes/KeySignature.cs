#nullable disable

using Sinfonia;
using Sinfonia.EntityFramework.Attributes;
using Sinfonia.EntityFramework.Entities;

namespace Sinfonia.EntityFramework.Classes
{
    public class KeySignature
    {
        public Step Step { get; set; }

        public bool Major { get; set; }
    }
}

#nullable enable
