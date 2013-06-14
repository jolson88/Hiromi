using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Hiromi.Components
{
    // ******** LEGACY: Will move to Entities.Components when appropriate

    /*
    public class ShakeComponent : GameObjectComponent
    {
        private TransformationComponent _transform;
        private float _maximumShakeDistance;
        private TimeSpan _duration;
        private Random _random;
        private bool _shakeHarderAtEnd;

        public ShakeComponent(int maximumShakeInPixels, TimeSpan duration) : this(maximumShakeInPixels, duration, false) { }
        public ShakeComponent(int maximumShakeInPixels, TimeSpan duration, bool shakeHarderAtEnd)
        {
            _duration = duration;
            _random = new Random();
            _shakeHarderAtEnd = shakeHarderAtEnd;

            // Convert to screen coordinates
            _maximumShakeDistance = (float)maximumShakeInPixels;
        }

        protected override void OnLoaded()
        {
            _transform = this.GameObject.GetComponent<TransformationComponent>();

            this.GameObject.ProcessManager.AttachProcess(Process.BuildProcessChain(
                new TweenProcess(_duration, interp =>
                {
                    var shakeDistance = 0f;
                    if (_shakeHarderAtEnd)
                    {
                        shakeDistance = (float)(_maximumShakeDistance * interp.Value);
                    }
                    else
                    {
                        shakeDistance = (float)(_maximumShakeDistance - (_maximumShakeDistance * interp.Value));
                    }

                    var rotation = _random.NextDouble() * (2 * Math.PI);
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
     */
}
