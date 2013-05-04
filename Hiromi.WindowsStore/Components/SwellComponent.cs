using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hiromi.Components
{
    public class SwellComponent : GameObjectComponent
    {
        private TransformationComponent _transform;
        private float _swellSize;
        private TimeSpan _duration;
        private bool _isRepeating;
        private Process _animationProcess;

        public SwellComponent(int swellInPixels, TimeSpan duration, bool isRepeating = false)
        {
            // To swell, we need two tweens (0-1 and 1-0)
            _duration = TimeSpan.FromSeconds(duration.TotalSeconds / 2);

            // Convert to screen coordinates
            _swellSize = swellInPixels;

            _isRepeating = isRepeating;
        }

        public override void Loaded()
        {
            CreateAnimationCycle();
        }

        public override void Removed()
        {
            _animationProcess.Fail();
            _isRepeating = false;
            _transform.Scale = 1.0f;
        }

        private void CreateAnimationCycle()
        {
            _transform = this.GameObject.GetComponent<TransformationComponent>();

            _animationProcess = Process.BuildProcessChain(
                new TweenProcess(Easing.GetSineFunction(), _duration, interp =>
                {
                    _transform.Scale = GetTweenedSwellSize(interp.Value);
                }),
                new TweenProcess(Easing.ConvertTo(EasingKind.EaseOut, Easing.GetSineFunction()), _duration, interp =>
                {
                    _transform.Scale = GetTweenedSwellSize(1.0f - interp.Value);
                }),
                new ActionProcess(() =>
                {
                    _transform.Scale = 1.0f;
                    if (_isRepeating)
                    {
                        CreateAnimationCycle();
                    }
                    else
                    {
                        // We are done, remove self
                        this.GameObject.RemoveComponent<SwellComponent>();
                    }
                }));

            this.GameObject.ProcessManager.AttachProcess(_animationProcess);
        }

        private float GetTweenedSwellSize(float tweenValue)
        {
            var rampedSwellSize = _swellSize * (float)tweenValue;

            // Final swell is how much percentage of the size of the image itself we need to scale up
            // (Example, if we need to scale up the image twice as large, it needs to be a scale of 2.0)
            var scaleOffset = rampedSwellSize / _transform.Bounds.Width;
            return 1.0f + scaleOffset;
        }
    }
}
