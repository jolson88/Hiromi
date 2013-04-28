using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hiromi
{
    public static class Tweening
    {
        /// <summary>
        /// Converts a linear ramp (linear values from 0.0 to 1.0) to a sawtooth (0.0 to 1.0 and back to 0.0)
        /// </summary>
        /// <param name="percentage"></param>
        /// <returns></returns>
        public static float ConvertFromLinearRampToSawtooth(float percentage)
        {
            return (percentage < 0.5f) ? percentage * 2 : (1.0f - percentage) * 2;
        }
    }
}
