using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hiromi
{
    public class GameObjectComponent
    {
        public GameObject GameObject { get; set; }

        public void Loaded()
        {
            OnLoaded();
        }

        protected virtual void OnLoaded() { }
    }
}
