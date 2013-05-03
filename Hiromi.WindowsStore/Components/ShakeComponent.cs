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
            _maximumShakeDistance = (float)maximumShakeInPixels;
        }

        public override void Loaded()
        {
            _transform = this.GameObject.GetComponent<TransformationComponent>();

            this.GameObject.ProcessManager.AttachProcess(Process.BuildProcessChain(
                new TweenProcess(_duration, interp =>
                {
                    var rotation = _random.NextDouble() * (2 * Math.PI);
                    var shakeDistance = (float)(_maximumShakeDistance - (_maximumShakeDistance * interp.Value));
                    var offset = new Vector2(shakeDistance, 0);
                    offset = Vector2.Transform(offset, Matrix.CreateRotationZ((float)rotation));

                    _transform.PositionOffset = offset;
                }),
                new ActionProcess(() =>
                {
                    _transform.PositionOffset = Vector2.Zero;
                    this.GameObject.RemoveComponent<ShakeComponent>();
                })));
        }
    }
}
