using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;

namespace Hiromi.Entities.Components
{
    public class SpriteComponent : IComponent
    {
        public Texture2D Texture { get; set; }

        public SpriteComponent(Texture2D texture)
        {
            this.Texture = texture;
        }
    }
}
