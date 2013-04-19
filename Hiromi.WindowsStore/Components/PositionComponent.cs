using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Hiromi.Components
{
    public class PositionComponent : IComponent
    {
        public BoundingBox Bounds { get; set; }
        public Vector2 Position 
        {
            get { return _position; }
            set { _position = value; CalculateBounds(); }
        }

        private Vector2 _position;

        public PositionComponent(Vector2 position, int widthInPixels, int heightInPixels)
        {
            // Convert bounds to screen coordinates (which everything operates in)
            this.Bounds = new BoundingBox(0, 
                0, 
                (float)widthInPixels / GraphicsService.Instance.GraphicsDevice.Viewport.Width, 
                (float)heightInPixels / GraphicsService.Instance.GraphicsDevice.Viewport.Height);
            
            this.Position = position;
        }

        // TODO: Does this better belong in a physics component?
        private void CalculateBounds()
        {
            // Remember: position is in screen coordinates. Bounding box should be in pixel coordinate. 
            this.Bounds = new BoundingBox(this.Position.X, this.Position.Y, this.Bounds.Width, this.Bounds.Height);
        }
    }
}
