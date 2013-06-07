using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace Hiromi.Entities.Components
{
    public class TransformComponent : IComponent
    {
        public Vector2 Position { get; set; }

        public TransformComponent(Vector2 position)
        {
            this.Position = position;
        }
    }
}
