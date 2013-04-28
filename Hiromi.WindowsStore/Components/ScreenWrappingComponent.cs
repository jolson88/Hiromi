using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Hiromi.Components
{
    public class ScreenWrappingComponent : GameObjectComponent
    {
        private bool _recentlyReflected = false;

        public override void Update(GameTime gameTime)
        {
            if (!_recentlyReflected)
            {
                var newPosition = Vector2.Zero;
                var pos = this.GameObject.GetComponent<PositionComponent>();

                // Reflect X-axis
                if (pos.Bounds.Right < 0.0f)
                {
                    newPosition = new Vector2(1.0f, pos.Position.Y);
                }
                else if (pos.Bounds.Left > 1.0f)
                {
                    newPosition = new Vector2(-pos.Bounds.Width, pos.Position.Y);
                }

                // Reflect Y-axis
                if (pos.Bounds.Bottom < 0.0f)
                {
                    newPosition = new Vector2(pos.Position.X, 1.0f);
                }
                else if (pos.Bounds.Top > 1.0f)
                {
                    newPosition = new Vector2(pos.Position.X, -pos.Bounds.Height);
                }

                if (newPosition != Vector2.Zero)
                {
                    pos.Position = newPosition;
                    _recentlyReflected = true;

                    // Make sure we don't keep resetting ourselves by introducing a delay
                    this.GameObject.ProcessManager.AttachProcess(new DelayProcess(TimeSpan.FromSeconds(1),
                        new ActionProcess(() => { _recentlyReflected = true; })));
                }
            }
        }
    }
}
