using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Hiromi
{
    // TODO: Add tiling, auto-scrolling (for movement)
    public class Background
    {
        public Texture2D Texture { get; set; }

        public Background(Texture2D texture)
        {
            this.Texture = texture;
        }
    }
}
