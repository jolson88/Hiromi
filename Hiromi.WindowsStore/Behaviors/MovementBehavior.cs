using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Hiromi;

namespace Hiromi.Behaviors
{
    public class MovementBehavior : GameObjectBehavior
    {
        // Velocity to change in seconds
        public Vector2 Velocity { get; set; }

        public MovementBehavior(Vector2 velocity)
        {
            this.Velocity = velocity;
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            this.GameObject.Position += this.Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
    }
}
