using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Hiromi
{
    public class ActionProcess : Process
    {
        private string _description = string.Empty;
        private bool _oneTime;
        private Action _action;

        public ActionProcess(Action executeAction) : this(executeAction, true) { }
        public ActionProcess(Action executeAction, bool oneTime) : this(string.Empty, executeAction, oneTime) { }
        public ActionProcess(string description, Action executeAction, bool oneTime)
        {
            _description = description;
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

        public override string ToString()
        {
            if (_description.Equals(string.Empty))
            {
                return base.ToString();
            }
            else
            {
                return _description;
            }
        }
    }
}
