using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Hiromi
{
    public class DelayProcess : Process
    {
        private string _description = string.Empty;
        private TimeSpan _timeRemaining;

        public DelayProcess(TimeSpan timeInSeconds, Process processToExecute) : this(string.Empty, timeInSeconds, processToExecute) { }
        public DelayProcess(string description, TimeSpan timeInSeconds, Process processToExecute)
        {
            _description = description;
            this._timeRemaining = timeInSeconds;
            this.AttachChild(processToExecute);
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            this._timeRemaining -= gameTime.ElapsedGameTime;
            if (this._timeRemaining.TotalSeconds <= 0)
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
