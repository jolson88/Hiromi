using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Hiromi
{
    public enum EasingFunction
    {
        Linear,
        Sine
    }

    public enum EasingKind
    {
        EaseIn,
        EaseOut,
        EaseInOut
    }

    public class TweenProcess : Process
    {
        private EasingFunction _easingFunction;
        private EasingKind _easingKind;
        private double _durationInSeconds;
        private double _elapsedTimeInSeconds = 0;
        private Action<float> _callback;

        // Delegate Lookup
        private static Dictionary<EasingFunction, EasingDelegate> _easingDelegates = new Dictionary<EasingFunction, EasingDelegate>()
        {
            { EasingFunction.Linear, Easing.Linear },
            { EasingFunction.Sine, Easing.Sine }
        };

        public TweenProcess(TimeSpan duration, Action<float> callback) : this(EasingFunction.Linear, EasingKind.EaseIn, duration, callback) { }
        public TweenProcess(EasingFunction easingFunction, EasingKind easingKind, TimeSpan duration, Action<float> callback)
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
                        _callback((float)_easingDelegates[_easingFunction](percentage));
                        break;

                    case EasingKind.EaseOut:
                        _callback(1.0f - (float)_easingDelegates[_easingFunction](1.0 - percentage));
                        break;

                    case EasingKind.EaseInOut:
                        // EasingKind.EaseInOut
                        if (percentage >= 0.5)
                        {
                            _callback((1.0f - (float)_easingDelegates[_easingFunction]((1.0f - percentage) * 2.0f)) * 0.5f + 0.5f);
                        }
                        else
                        {
                            _callback((float)_easingDelegates[_easingFunction](percentage * 2.0f) * 0.5f);
                        }
                        break;
                }
            }
        }
    }
}
