﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Hiromi
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
            this._timeRemaining -= gameTime.ElapsedGameTime;
            if (this._timeRemaining.TotalSeconds <= 0)
            {
                this.Succeed();
            }
        }
    }

}
