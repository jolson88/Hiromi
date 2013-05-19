using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Hiromi.Components
{
    public class SimpleMovementComponent : GameObjectComponent
    {
        public Vector2 Velocity { get; set; }

        public SimpleMovementComponent(Vector2 velocity)
        {
            this.Velocity = velocity;
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            var pos = this.GameObject.GetComponent<TransformationComponent>();
            pos.Position += this.Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
    }
}
