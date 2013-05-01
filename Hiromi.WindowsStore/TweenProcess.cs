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
        private EasingKind _easingKind;
        private double _durationInSeconds;
        private double _elapsedTimeInSeconds = 0;
        private Action<Interpolation> _callback;

        public TweenProcess(TimeSpan duration, Action<Interpolation> callback) : this(Easing.GetLinearFunction(), EasingKind.EaseIn, duration, callback) { }
        public TweenProcess(EasingDelegate easingFunction, EasingKind easingKind, TimeSpan duration, Action<Interpolation> callback)
        {
            _easingFunction = easingFunction;
            _easingKind = easingKind;
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
                switch (_easingKind)
                {
                    case EasingKind.EaseIn:
                        _callback(new Interpolation(percentage, (float)_easingFunction(percentage)));
                        break;

                    case EasingKind.EaseOut:
                        _callback(new Interpolation(percentage, 1.0f - (float)_easingFunction(1.0 - percentage)));
                        break;

                    case EasingKind.EaseInOut:
                        if (percentage >= 0.5)
                        {
                            _callback(new Interpolation(percentage, (1.0f - (float)_easingFunction((1.0f - percentage) * 2.0f)) * 0.5f + 0.5f));
                        }
                        else
                        {
                            _callback(new Interpolation(percentage, (float)_easingFunction(percentage * 2.0f) * 0.5f));
                        }
                        break;
                }
            }
        }
    }
}
