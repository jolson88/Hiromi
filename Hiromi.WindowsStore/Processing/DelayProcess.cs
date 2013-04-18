using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Hiromi.Processing
{
    public class DelayProcess : Process
    {
        private TimeSpan _timeRemaining;

        public DelayProcess(TimeSpan timeInSeconds, Process processToExecute)
        {
            this._timeRemaining = timeInSeconds;
            this.AttachChild(processToExecute);
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            this._timeRemaining = this._timeRemaining.Subtract(TimeSpan.FromSeconds(gameTime.ElapsedGameTime.TotalSeconds)); ;
            if (this._timeRemaining.TotalSeconds <= 0)
            {
                this.Succeed();
            }
        }
    }

}
