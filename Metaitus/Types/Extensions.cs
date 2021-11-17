using System;
using System.Collections.Generic;
using System.Text;

namespace Metaitus.Types
{
    public static class Extensions
    {
        private const double FloatEpsilon = 1e-5;
        private const double DoubleEpsilon = 1e-10;

        public static bool IsZero(this float f)
        {
            return Math.Abs(f) < FloatEpsilon;
        }

        public static bool IsZero(this double d)
        {
            return Math.Abs(d) < DoubleEpsilon;
        }
    }
}
