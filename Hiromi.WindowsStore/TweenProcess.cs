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
        private TimeSpan _timeLength;
        private TimeSpan _elapsedTime;
        private Action<float> _callback;

        public TweenProcess(TimeSpan timeLength, Action<float> callback)
        {
            _timeLength = timeLength;
            _callback = callback;
            _elapsedTime = TimeSpan.Zero;
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            _elapsedTime += gameTime.ElapsedGameTime;
            if (_elapsedTime >= _timeLength)
            {
                this.Succeed();
            }
            else
            {
                var percentage = (float)(_elapsedTime.TotalSeconds / _timeLength.TotalSeconds);
                _callback(percentage);
            }
        }
    }
}
