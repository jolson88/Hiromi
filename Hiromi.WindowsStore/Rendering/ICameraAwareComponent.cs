using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hiromi.Rendering;

namespace Hiromi.Rendering
{
    public interface ICameraAwareComponent
    {
        Camera ActiveCamera { get; set; }
    }
}
