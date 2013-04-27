using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hiromi.Rendering
{
    public enum RenderPass
    {
        FirstPass = 0,
        BackgroundPass = 1,
        GameObjectPass = 2,
        UserInterfacePass = 3,
        LassPass = 4
    }
}
