using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hiromi
{
    public interface ICameraAwareComponent
    {
        Camera ActiveCamera { get; set; }
    }
}
