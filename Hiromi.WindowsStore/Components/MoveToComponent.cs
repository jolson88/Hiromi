using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hiromi;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Hiromi.Components
{
    public class MoveToComponent : GameObjectComponent
    {
        private Vector2 _originalPosition;
        private Vector2 _destination;
        private TimeSpan _duration;
        private TimeSpan _elapsedTime;
        private EasingDelegate _easingX;
        private EasingDelegate _easingY;
        private float _augmentX;
        private float _augmentY;

        public MoveToComponent(Vector2 destination, TimeSpan duration) : this(destination, duration, Easing.GetLinearFunction(), Easing.GetLinearFunction()) { }
        public MoveToComponent(Vector2 destination, TimeSpan duration, EasingDelegate easingX, EasingDelegate easingY, float augmentX = 0, float augmentY = 0)
        {
            _destination = destination;
            _duration = duration;
            _easingX = easingX;
            _easingY = easingY;
            _augmentX = augmentX;
            _augmentY = augmentY;

            _elapsedTime = TimeSpan.FromSeconds(0);
        }

        protected override void OnLoaded()
        {
            _originalPosition = this.GameObject.Transform.Position;
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            if (_elapsedTime >= _duration)
            {
                this.GameObject.RemoveComponent<MoveToComponent>();
                return;
            }

            // Rev and "clamp" elapsed to duration if we need to (to prevent moving past the destination)
            _elapsedTime += gameTime.ElapsedGameTime;
            if (_elapsedTime > _duration)
            {
                _elapsedTime = _duration;
            }

            var percentage = _elapsedTime.TotalSeconds / _duration.TotalSeconds;
            var targetOffsetX = _destination.X - _originalPosition.X;
            var targetOffsetY = _destination.Y - _originalPosition.Y;
            var offsetX = _easingX(percentage) * targetOffsetX + (_augmentX * _easingX(percentage));
            var offsetY = _easingY(percentage) * targetOffsetY + (_augmentY * _easingY(percentage));

            this.GameObject.Transform.Position = _originalPosition + new Vector2((float)offsetX, (float)offsetY);
        }
    }
}
