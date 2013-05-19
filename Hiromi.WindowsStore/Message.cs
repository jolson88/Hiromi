using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hiromi
{
    public enum MessageVerbosity
    {
        Signal,
        Noise
    }

    public class Message
    {
        public virtual MessageVerbosity GetMessageVerbosity()
        {
            return MessageVerbosity.Signal;
        }
    }
}
