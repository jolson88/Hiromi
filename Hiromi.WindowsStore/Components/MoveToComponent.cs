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
        private EasingDelegate _tweenX;
        private EasingDelegate _tweenY;

        public MoveToComponent(Vector2 destination, TimeSpan duration) : this(destination, duration, Easing.GetLinearFunction(), Easing.GetLinearFunction()) { }
        public MoveToComponent(Vector2 destination, TimeSpan duration, EasingDelegate tweenX, EasingDelegate tweenY)
        {
            _destination = destination;
            _duration = duration;
            _tweenX = tweenX;
            _tweenY = tweenY;

            _elapsedTime = TimeSpan.FromSeconds(0);
        }

        public override void Loaded()
        {
            _originalPosition = this.GameObject.Transform.Position;
        }

        public override void Update(GameTime gameTime)
        {
            if (_elapsedTime >= _duration)
            {
                this.GameObject.RemoveComponent<MoveToComponent>();
                return;
            }

            _elapsedTime += gameTime.ElapsedGameTime;
            var percentage = _elapsedTime.TotalSeconds / _duration.TotalSeconds;

            var targetOffsetX = _originalPosition.X - _destination.X;
            var targetOffsetY = _originalPosition.Y - _destination.Y;
            var offsetX = _tweenX(percentage) * targetOffsetX;
            var offsetY = _tweenY(percentage) * targetOffsetY;

            this.GameObject.Transform.Position = _originalPosition - new Vector2((float)offsetX, (float)offsetY);
        }
    }
}
