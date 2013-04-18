using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Hiromi.Processing
{
    public class ActionProcess : Process
    {
        private bool _oneTime;
        private Action _action;

        public ActionProcess(Action executeAction, bool oneTime = true)
        {
            this._oneTime = oneTime;
            this._action = executeAction;
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            this._action();
            if (this._oneTime)
            {
                this.Succeed();
            }
        }
    }

}
