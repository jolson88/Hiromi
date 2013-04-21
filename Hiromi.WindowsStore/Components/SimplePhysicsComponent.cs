using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Hiromi.Components
{
    public class SimplePhysicsComponent : GameObjectComponent
    {
        public Vector2 Velocity { get; set; }

        public SimplePhysicsComponent(Vector2 velocity)
        {
            this.Velocity = velocity;
        }
    }
}
