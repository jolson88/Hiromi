using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Hiromi;

namespace Hiromi.Components
{
    public class ButtonComponent : IComponent
    {
        public Texture2D FocusTexture { get; set; }
        public Texture2D NonFocusTexture { get; set; }

        public ButtonComponent() : this(null, null) { }
        public ButtonComponent(Texture2D focusTexture, Texture2D nonFocusTexture)
        {
            this.FocusTexture = focusTexture;
            this.NonFocusTexture = nonFocusTexture;
        }
    }
}
