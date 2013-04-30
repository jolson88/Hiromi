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
            _duration = duration;

            // Convert to screen coordinates
            _swellSize = (float)swellInPixels / GraphicsService.Instance.GraphicsDevice.Viewport.Width;
        }

        public override void Loaded()
        {
            _transform = this.GameObject.GetComponent<TransformationComponent>();

            var swellProcess = new TweenProcess(EasingFunction.Sine, EasingKind.EaseIn, _duration, tweenValue =>
            {
                var rampedSwellSize = _swellSize * tweenValue;
                
                // Final swell is how much percentage of the size of the image itself we need to scale up
                // (Example, if we need to scale up the image twice as large, it needs to be a scale of 2.0)
                var scaleOffset = rampedSwellSize / _transform.Bounds.Width;
                _transform.Scale = 1.0f + scaleOffset;
            });
            swellProcess.AttachChild(new ActionProcess(() =>
            {
                _transform.Scale = 1.0f;
                this.GameObject.RemoveComponent<SwellComponent>();
            }));
            this.GameObject.ProcessManager.AttachProcess(swellProcess);
        }
    }
}
