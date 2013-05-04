using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Hiromi.Components
{
    public class ScreenWrappingComponent : GameObjectComponent, ICameraAwareComponent
    {
        public Camera ActiveCamera { get; set; }
        public bool IsEnabled { get; set; }

        private bool _recentlyReflected = false;

        public ScreenWrappingComponent(bool isEnabled = true)
        {
            this.IsEnabled = isEnabled;
        }

        public override void Update(GameTime gameTime)
        {
            if (this.IsEnabled && !_recentlyReflected && this.ActiveCamera != null)
            {
                var transform = this.GameObject.Transform;
                var newPosition = transform.Position;

                // Reflect X-axis
                if (transform.Bounds.Right < this.ActiveCamera.Bounds.Left)
                {
                    newPosition = new Vector2(this.ActiveCamera.Bounds.Right, transform.Bounds.Top);
                }
                else if (transform.Bounds.Left > this.ActiveCamera.Bounds.Right)
                {
                    newPosition = new Vector2(this.ActiveCamera.Bounds.Left - transform.Bounds.Width, transform.Bounds.Top);
                }

                // Reflect Y-axis
                if (transform.Bounds.Bottom > this.ActiveCamera.Bounds.Top)
                {
                    newPosition = new Vector2(transform.Bounds.Left, this.ActiveCamera.Bounds.Bottom);
                }
                else if (transform.Bounds.Top < this.ActiveCamera.Bounds.Bottom)
                {
                    newPosition = new Vector2(transform.Bounds.Left, this.ActiveCamera.Bounds.Top + transform.Bounds.Height);
                }

                if (newPosition != transform.Position)
                {
                    transform.Position = newPosition;
                    _recentlyReflected = true;

                    // Make sure we don't keep resetting ourselves by introducing a delay
                    this.GameObject.ProcessManager.AttachProcess(new DelayProcess(TimeSpan.FromSeconds(0.1),
                        new ActionProcess(() => { _recentlyReflected = false; })));
                }
            }
        }
    }
}
