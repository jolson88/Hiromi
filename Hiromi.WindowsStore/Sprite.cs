using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Hiromi
{
    public class Sprite
    {
        public Texture2D Texture { get; set; }

        public Sprite(Texture2D texture)
        {
            this.Texture = texture;
        }
    }
}
