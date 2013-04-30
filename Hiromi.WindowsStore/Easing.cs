using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hiromi
{
    public delegate double EasingDelegate(double percentage);

    public static class Easing
    {
        public static double Linear(double percentage)
        {
            return percentage;
        }

        public static double Sine(double percentage)
        {
            return Math.Sin(Linear(percentage) * (Math.PI / 2));
        }
    }
}
