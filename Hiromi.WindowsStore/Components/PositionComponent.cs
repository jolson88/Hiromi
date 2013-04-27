using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Hiromi.Components
{
    public enum VerticalAnchor
    {
        Top,
        Center,
        Bottom
    }

    public enum HorizontalAnchor
    {
        Left,
        Center,
        Right
    }

    public class PositionComponent : GameObjectComponent
    {
        public VerticalAnchor VerticalAnchor { get; set; }
        public HorizontalAnchor HorizontalAnchor { get; set; }
        public BoundingBox Bounds { get; set; }

        public Vector2 Position 
        {
            get { return _position; }
            set { _position = value; CalculateBounds(); OnPositionChanged(); }
        }

        private Vector2 _position;

        public PositionComponent(Vector2 position, int widthInPixels, int heightInPixels)
            : this(position, widthInPixels, heightInPixels, HorizontalAnchor.Left, VerticalAnchor.Top) { }
        public PositionComponent(Vector2 position, int widthInPixels, int heightInPixels, HorizontalAnchor horizontalAnchor)
            : this(position, widthInPixels, heightInPixels, horizontalAnchor, VerticalAnchor.Top) { }
        public PositionComponent(Vector2 position, int widthInPixels, int heightInPixels, HorizontalAnchor horizontalAnchor, VerticalAnchor verticalAnchor)
        {
            this.HorizontalAnchor = horizontalAnchor;
            this.VerticalAnchor = verticalAnchor;

            // Convert bounds to screen coordinates (which everything operates in)
            this.Bounds = new BoundingBox(0, 
                0, 
                (float)widthInPixels / GraphicsService.Instance.GraphicsDevice.Viewport.Width, 
                (float)heightInPixels / GraphicsService.Instance.GraphicsDevice.Viewport.Height);
            
            this.Position = position;
        }

        private void OnPositionChanged()
        {
            // When the component is first created, it won't be attached to a GameObject yet.
            if (this.GameObject != null)
            {
                this.GameObject.MessageManager.TriggerMessage(new GameObjectMovedMessage(this.GameObject));
            }
        }

        private void CalculateBounds()
        {
            // Remember: position is in screen coordinates. Bounding box should be in pixel coordinate. 
            this.Bounds = new BoundingBox(this.Position.X, this.Position.Y, this.Bounds.Width, this.Bounds.Height);
            this.Bounds.SizeChanged += (sender, e) => { RepositionPositionBasedOnAlignment(); };

            RepositionPositionBasedOnAlignment();
        }

        private void RepositionPositionBasedOnAlignment()
        {
            if (this.HorizontalAnchor == HorizontalAnchor.Center)
            {
                this.Bounds.X = this.Position.X - this.Bounds.Width / 2;
            }
            else if (this.HorizontalAnchor == HorizontalAnchor.Right)
            {
                this.Bounds.X = this.Position.X - this.Bounds.Width;
            }

            if (this.VerticalAnchor == VerticalAnchor.Center)
            {
                this.Bounds.Y = this.Position.Y - this.Bounds.Height / 2;
            }
            else if (this.VerticalAnchor == VerticalAnchor.Bottom)
            {
                this.Bounds.Y = this.Position.Y - this.Bounds.Height;
            }
        }
    }
}
