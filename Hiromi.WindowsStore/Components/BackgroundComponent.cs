using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Hiromi.Components
{
    public class BackgroundComponent : IComponent
    {
        public Background Background { get; set; }

        public BackgroundComponent(Background background)
        {
            this.Background = background;
        }
    }
}
