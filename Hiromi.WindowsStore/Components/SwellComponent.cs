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

        public SwellComponent(int swellInPixels, TimeSpan duration)
        {
            // To swell, we need two tweens (0-1 and 1-0)
            _duration = TimeSpan.FromSeconds(duration.TotalSeconds / 2);

            // Convert to screen coordinates
            _swellSize = (float)swellInPixels / GraphicsService.Instance.GraphicsDevice.Viewport.Width;
        }

        public override void Loaded()
        {
            _transform = this.GameObject.GetComponent<TransformationComponent>();

            this.GameObject.ProcessManager.AttachProcess(Process.BuildProcessChain(
                new TweenProcess(Easing.GetSineFunction(), EasingKind.EaseIn, _duration, tweenValue =>
                {
                    _transform.Scale = GetTweenedSwellSize(tweenValue);
                }),
                new TweenProcess(Easing.GetSineFunction(), EasingKind.EaseOut, _duration, tweenValue =>
                {
                    _transform.Scale = GetTweenedSwellSize(1.0f - tweenValue);
                }),
                new ActionProcess(() =>
                {
                    _transform.Scale = 1.0f;
                    this.GameObject.RemoveComponent<SwellComponent>();
                })));
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
