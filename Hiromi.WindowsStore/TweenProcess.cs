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
        private static Dictionary<EasingFunction,Dictionary<EasingKind,EasingDelegate>> _easingDelegates = new Dictionary<EasingFunction,Dictionary<EasingKind,EasingDelegate>>()
        {
            { 
                EasingFunction.Linear, new Dictionary<EasingKind,EasingDelegate>()
                {
                    { EasingKind.EaseIn, Easing.Linear },
                    { EasingKind.EaseOut, Easing.Linear },
                    { EasingKind.EaseInOut, Easing.Linear }
                }
            },
            { 
                EasingFunction.Sine, new Dictionary<EasingKind,EasingDelegate>()
                {
                    { EasingKind.EaseIn, Easing.Sine },
                    { EasingKind.EaseOut, Easing.Sine },
                    { EasingKind.EaseInOut, Easing.Sine }
                }
            }
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
                _callback((float)_easingDelegates[_easingFunction][_easingKind](_elapsedTimeInSeconds, _durationInSeconds));
            }
        }
    }
}
