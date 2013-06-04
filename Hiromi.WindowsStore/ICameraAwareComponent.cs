using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hiromi
{
    public interface ICameraAwareComponent
    {
        Camera ActiveCamera { get; set; }
    }
}
