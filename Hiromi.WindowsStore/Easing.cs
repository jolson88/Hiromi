using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hiromi
{
    public delegate double EasingDelegate(double percentage);

    //
    // See http://msdn.microsoft.com/en-us/library/ee308751.aspx for some great charts on what these functions look like
    //
    public static class Easing
    {
        public static EasingDelegate GetLinearFunction()
        {
            return (percentage => { return percentage; });
        }

        public static EasingDelegate GetSineFunction()
        {
            return (percentage =>
            {
                return Math.Sin(GetLinearFunction()(percentage) * (Math.PI / 2));
            });
        }

        public static EasingDelegate GetElasticFunction()
        {
            return GetElasticFunction(3.0, 3.0);
        }

        public static EasingDelegate GetElasticFunction(double oscillations, double springiness)
        {
            return (percentage =>
            {
                // Borrowed from System.Windows.Media.Animation.ElasticEase
                var springFactor = (Math.Exp(springiness * percentage) - 1.0) / (Math.Exp(springiness) - 1.0);
                return springFactor * Math.Sin(((2 * Math.PI) * oscillations + (Math.PI / 2)) * percentage);
            });
        }

        public static EasingDelegate GetBackFunction()
        {
            return GetBackFunction(1.0);
        }

        public static EasingDelegate GetBackFunction(double amplitude)
        {
            return (percentage =>
            {
                // Borrowed from System.Windows.Media.Animation.BackEase
                return Math.Pow(percentage, 3.0) - percentage * amplitude * Math.Sin(Math.PI * percentage);
            });
        }
    }
}
