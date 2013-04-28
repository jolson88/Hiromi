using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public override void Update(GameTime gameTime)
        {
            var pos = this.GameObject.GetComponent<PositionComponent>();
            pos.Position += this.Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
    }
}
