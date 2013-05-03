using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hiromi.Rendering;

namespace Hiromi.Components
{
    public interface ICameraAware
    {
        Camera ActiveCamera { get; set; }
    }
}
