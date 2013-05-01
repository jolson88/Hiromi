using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hiromi
{
    public class Interpolation
    {
        public double Percentage { get; set; }
        public float Value { get; set; }

        public Interpolation(double percentage, float value)
        {
            this.Percentage = percentage;
            this.Value = value;
        }
    }

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

        public static EasingDelegate GetBounceFunction()
        {
            return GetBounceFunction(3, 2);
        }

        public static EasingDelegate GetBounceFunction(double bounces, double bounciness)
        {
            return (percentage =>
            {
                // Borrowed from System.Windows.Media.Animation.BounceEase

                // YUCK!!! Need to find better names for variables
                if (bounciness <= 1.0)
                {
                    bounciness = 1.001;
                }
                double num3 = Math.Pow(bounciness, bounces);
                double num4 = 1.0 - bounciness;
                double num5 = (1.0 - num3) / num4 + num3 * 0.5;
                double num6 = percentage * num5;
                double d = Math.Log(-num6 * (1.0 - bounciness) + 1.0, bounciness);
                double num7 = Math.Floor(d);
                double y = num7 + 1.0;
                double num8 = (1.0 - Math.Pow(bounciness, num7)) / (num4 * num5);
                double num9 = (1.0 - Math.Pow(bounciness, y)) / (num4 * num5);
                double num10 = (num8 + num9) * 0.5;
                double num11 = percentage - num10;
                double num12 = num10 - num8;
                double num13 = Math.Pow(1.0 / bounciness, bounces - num7);
                return -num13 / (num12 * num12) * (num11 - num12) * (num11 + num12);
            });
        }
    }
}
