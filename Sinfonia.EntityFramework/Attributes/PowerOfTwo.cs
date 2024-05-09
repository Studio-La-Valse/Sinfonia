#nullable disable

using System.ComponentModel.DataAnnotations;

namespace StudioLaValse.ScoreDocument.Models.Attributes
{
    public class PowerOfTwo : ValidationAttribute
    {
        public PowerOfTwo()
        {

        }

        public override bool IsValid(object value)
        {
            if (value is not int i)
            {
                return false;
            }

            return IsPowerOfTwo(i);
        }

        private bool IsPowerOfTwo(int x)
        {
            return x != 0 && (x & (x - 1)) == 0;
        }
    }
}

#nullable enable