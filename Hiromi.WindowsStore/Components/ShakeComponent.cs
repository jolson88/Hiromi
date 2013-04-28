using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Hiromi.Components
{
    public class ShakeComponent : GameObjectComponent
    {
        private TransformationComponent _transform;
        private float _maximumShakeDistance;
        private TimeSpan _duration;
        private Random _random;

        public ShakeComponent(int maximumShakeInPixels, TimeSpan duration)
        {
            _duration = duration;
            _random = new Random();

            // Convert to screen coordinates
            _maximumShakeDistance = (float)maximumShakeInPixels / GraphicsService.Instance.GraphicsDevice.Viewport.Width;
        }

        public override void Loaded()
        {
            _transform = this.GameObject.GetComponent<TransformationComponent>();

            var shakeProcess = new TweenProcess(_duration, percentage =>
            {
                var rotation = _random.NextDouble() * (2 * Math.PI);
                var shakeDistance = _maximumShakeDistance - (_maximumShakeDistance * percentage);
                var offset = new Vector2(shakeDistance, 0);
                offset = Vector2.Transform(offset, Matrix.CreateRotationZ((float)rotation));

                _transform.PositionOffset = offset;
            });
            shakeProcess.AttachChild(new ActionProcess(() =>
            {
                this.GameObject.RemoveComponent(this);
            }));
            this.GameObject.ProcessManager.AttachProcess(shakeProcess);
        }
    }
}
