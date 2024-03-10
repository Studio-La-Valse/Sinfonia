namespace Sinfonia.Native
{
    public static class MathUtils
    {
        public static IList<double> Map(IList<double> list, double newMin, double newMax)
        {
            double listMin = list.Min();
            double listMax = list.Max();

            if (listMax - listMin == 0)
            {
                return list;
            }

            List<double> newList = [];

            foreach (double value in list)
            {
                double newValue = Map(value, listMin, listMax, newMin, newMax);

                newList.Add((float)newValue);
            }

            return newList;
        }
        public static IList<float> Map(IList<float> list, float newMin, float newMax)
        {
            float listMin = list.Min();
            float listMax = list.Max();

            if (listMax - listMin == 0)
            {
                return list;
            }

            List<float> newList = [];

            foreach (float value in list)
            {
                double newValue = Map(value, listMin, listMax, newMin, newMax);

                newList.Add((float)newValue);
            }

            return newList;
        }
        public static double[] SubSampleValues(IEnumerable<double> values, int newLength)
        {
            if (!values.Any())
            {
                return new double[0];
            }

            int originalCount = values.Count();

            if (newLength == originalCount)
            {
                return values.ToArray();
            }

            double[] newArray = new double[newLength];

            for (int i = 0; i < newLength; i++)
            {
                int indexInOriginalCollection = (int)Math.Floor(Map(i, 0d, newLength, 0, originalCount));
                newArray[i] = values.ElementAt(indexInOriginalCollection);
            }

            return newArray;
        }
        public static float[] SubSampleValues(IEnumerable<float> values, int newLength)
        {
            if (!values.Any())
            {
                return new float[0];
            }

            int originalCount = values.Count();

            if (newLength == originalCount)
            {
                return values.ToArray();
            }

            float[] newArray = new float[newLength];

            for (int i = 0; i < newLength; i++)
            {
                int indexInOriginalCollection = (int)Math.Floor(Map(i, 0d, newLength, 0, originalCount));
                newArray[i] = values.ElementAt(indexInOriginalCollection);
            }

            return newArray;
        }
        public static double Map(double value, double minStart, double maxStart, double minEnd, double maxEnd)
        {
            double fraction = maxStart - minStart;

            return fraction == 0 ? throw new ArgumentOutOfRangeException() : minEnd + ((maxEnd - minEnd) * ((value - minStart) / fraction));
        }
        public static decimal Map(decimal value, decimal minStart, decimal maxStart, decimal minEnd, decimal maxEnd)
        {
            decimal fraction = maxStart - minStart;

            return fraction == 0 ? throw new ArgumentOutOfRangeException() : minEnd + ((maxEnd - minEnd) * ((value - minStart) / fraction));
        }
        public static double Clamp(double value, double minValue, double maxValue)
        {
            double trueMin = Math.Min(minValue, maxValue);
            double trueMax = Math.Max(minValue, maxValue);

            return Math.Clamp(value, trueMin, trueMax);
        }
        public static decimal Clamp(decimal value, decimal minValue, decimal maxValue)
        {
            decimal trueMin = Math.Min(minValue, maxValue);

            decimal trueMax = Math.Max(minValue, maxValue);

            return Math.Clamp(value, trueMin, trueMax);
        }
        public static int Clamp(int value, int minValue, int maxValue)
        {
            int trueMin = Math.Min(minValue, maxValue);

            int trueMax = Math.Max(minValue, maxValue);

            return Math.Clamp(value, trueMin, trueMax);
        }
        public static double ForcePositiveModulo(double value, double max)
        {
            while (value < 0)
            {
                value += max;
            }

            value %= max;

            return value;
        }
        public static int UnsignedModulo(int value, int max)
        {
            if (max <= 0)
            {
                throw new Exception("The max value needs to be greater than or equal to one.");
            }

            while (value < 0)
            {
                value += max;
            }

            int unsignedValue = value % max;

            return unsignedValue;
        }
        public static bool IsAlmostEqualTo(this double a, double b, double epsilon = 0.001)
        {
            return Math.Abs(a - b) < epsilon;
        }

        public static int GCD(this int[] numbers)
        {
            return numbers.Aggregate(GCD);
        }

        public static int GCD(this int a, int b)
        {
            return a == 0 || b == 0 ? a | b : Math.Min(a, b).GCD(Math.Max(a, b) % Math.Min(a, b));
        }
    }
}