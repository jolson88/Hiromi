using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hiromi;

namespace Hiromi.Components
{
    public class ButtonComponent : IComponent
    {
        public Sprite FocusSprite { get; set; }
        public Sprite NonFocusSprite { get; set; }

        public ButtonComponent() : this(null, null) { }
        public ButtonComponent(Sprite focusSprite, Sprite nonFocusSprite)
        {
            this.FocusSprite = focusSprite;
            this.NonFocusSprite = nonFocusSprite;
        }
    }
}
