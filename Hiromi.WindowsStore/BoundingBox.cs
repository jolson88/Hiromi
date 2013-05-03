using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Hiromi
{
    public class BoundingBox
    {
        public EventHandler SizeChanged;

        public float X { get; set; }
        public float Y { get; set; }
        public float Width { get { return _width; } set { _width = value; OnSizeChanged(); } }
        public float Height { get { return _height; } set { _height = value; OnSizeChanged(); } }
        public float Bottom { get { return this.Y - this.Height; } }
        public float Left { get { return this.X; } }
        public float Top { get { return this.Y; } }
        public float Right { get { return this.X + this.Width; } }

        private float _width;
        private float _height;

        /// <summary>
        /// Creates a new float-based bounding box for containment and collision checking.
        /// </summary>
        /// <param name="x">The left coordinate of the bounding box</param>
        /// <param name="y">The top coordinate of the bounding box</param>
        /// <param name="width">The width of the bounding box</param>
        /// <param name="height">The height of the bounding box</param>
        public BoundingBox(float x, float y, float width, float height)
        {
            this.X = x;
            this.Y = y;
            _width = width;
            _height = height;
        }

        public bool Contains(float x, float y)
        {
            return this.X <= x && x < this.X + this.Width && this.Y <= y && y < this.Y + this.Height;
        }

        public bool Contains(BoundingBox value)
        {
            return this.X <= value.X && value.X + value.Width <= this.X + this.Width && this.Y <= value.Y && value.Y + value.Height <= this.Y + this.Height;
        }

        public BoundingBox Deflate(int pixelsToDeflate)
        {
            float widthOffset = (float)pixelsToDeflate / GraphicsService.Instance.GraphicsDevice.Viewport.Width;
            float heightOffset = (float)pixelsToDeflate / GraphicsService.Instance.GraphicsDevice.Viewport.Height;

            return new BoundingBox(this.X + widthOffset,
                this.Y + heightOffset,
                this.Width - (widthOffset * 2),
                this.Height - (heightOffset * 2));
        }

        public bool Intersects(BoundingBox value)
        {
            return value.Left < this.Right && this.Left < value.Right && value.Top < this.Bottom && this.Top < value.Bottom;
        }

        private void OnSizeChanged()
        {
            if (this.SizeChanged != null)
            {
                this.SizeChanged(this, null);
            }
        }
    }
}
