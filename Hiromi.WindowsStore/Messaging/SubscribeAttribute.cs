using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hiromi.Messaging
{
    [AttributeUsage(AttributeTargets.Method)]
    public class SubscribeAttribute : Attribute { }
}
