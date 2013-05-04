using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Hiromi
{
    public enum EasingKind
    {
        EaseIn,
        EaseOut,
        EaseInOut
    }

    public class TweenProcess : Process
    {
        private EasingDelegate _easingFunction;
        private double _durationInSeconds;
        private double _elapsedTimeInSeconds = 0;
        private Action<Interpolation> _callback;

        public TweenProcess(TimeSpan duration, Action<Interpolation> callback) : this(Easing.GetLinearFunction(), duration, callback) { }
        public TweenProcess(EasingDelegate easingFunction, TimeSpan duration, Action<Interpolation> callback)
        {
            _easingFunction = easingFunction;
            _durationInSeconds = duration.TotalSeconds;
            _callback = callback;
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            _elapsedTimeInSeconds += gameTime.ElapsedGameTime.TotalSeconds;
            if (_elapsedTimeInSeconds >= _durationInSeconds)
            {
                this.Succeed();
            }
            else
            {
                var percentage = _elapsedTimeInSeconds / _durationInSeconds;
                _callback(new Interpolation(percentage, (float)_easingFunction(percentage)));
            }
        }
    }
}
