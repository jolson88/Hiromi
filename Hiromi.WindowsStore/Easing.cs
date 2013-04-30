using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hiromi
{
    public delegate double EasingDelegate(double currentTimeInSeconds, double durationInSeconds);

    public static class Easing
    {
        public static double Linear(double currentTimeInSeconds, double durationInSeconds)
        {
            return currentTimeInSeconds / durationInSeconds;
        }

        public static double Sine(double currentTimeInSeconds, double durationInSeconds)
        {
            return Math.Sin(Linear(currentTimeInSeconds, durationInSeconds) * Math.PI);
        }
    }
}
