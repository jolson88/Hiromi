using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Hiromi
{
    public class TweenProcess : Process
    {
        private double _durationInSeconds;
        private double _elapsedTimeInSeconds = 0;
        private Action<float> _callback;

        public TweenProcess(TimeSpan timeLength, Action<float> callback)
        {
            _durationInSeconds = timeLength.TotalSeconds;
            _callback = callback;
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            _elapsedTimeInSeconds += gameTime.ElapsedGameTime.TotalSeconds;
            if (_elapsedTimeInSeconds >= _durationInSeconds)
            {
                this.Succeed();
            }
            else
            {
                var percentage = (float)(_elapsedTimeInSeconds / _durationInSeconds);
                _callback(percentage);
            }
        }
    }
}
