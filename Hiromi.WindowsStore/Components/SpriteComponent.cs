using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Hiromi.Components
{
    public class SpriteComponent : IComponent
    {
        public bool IsVisible { get; set; }
        public Sprite Sprite { get; set; }

        public SpriteComponent(Sprite sprite)
        {
            this.Sprite = sprite;
            this.IsVisible = true;
        }
    }
}
