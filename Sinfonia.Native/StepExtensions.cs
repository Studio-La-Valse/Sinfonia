﻿namespace Sinfonia.Native
{
    public static class StepExtensions
    {
        public static ColorHSB ToColorChromatic(this Step step)
        {
            int intValue = step.SemiTones;

            while (intValue < 0)
            {
                intValue += 12;
            }

            double hue = intValue % 12 / 12d;

            return new ColorHSB(hue, 1, 0.5);
        }

        public static ColorHSB ToColorFifths(this Step step)
        {
            int intValue = step.SemiTones;

            while (intValue < 0)
            {
                intValue += 12;
            }

            double hue = intValue * 7 % 12 / 12d;

            return new ColorHSB(hue, 1, 0.5);
        }
    }
}
