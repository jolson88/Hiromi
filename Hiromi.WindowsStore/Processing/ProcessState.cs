using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hiromi.Processing
{
    public enum ProcessState
    {
        // Neither dead or alive
        Uninitialized,
        Removed,

        // Living
        Running,
        Paused,

        // Dead
        Succeeded,
        Failed,
        Aborted
    }
}
