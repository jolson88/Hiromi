using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

    public class TransformationComponent : GameObjectComponent
    {
        public VerticalAnchor VerticalAnchor { get; set; }
        public HorizontalAnchor HorizontalAnchor { get; set; }
        public BoundingBox Bounds { get; set; }
        public float Rotation { get; set; }
        public float Z { get; set; }

        public Vector2 Position 
        {
            get { return _position; }
            set { _position = value; CalculateBounds(); OnGameObjectMoved(); }
        }

        public Vector2 PositionOffset
        {
            get { return _positionOffset; }
            set { _positionOffset = value; CalculateBounds(); OnGameObjectMoved(); }
        }

        public float Scale
        {
            get { return _scale; }
            set { _scale = value; CalculateBounds(); OnGameObjectMoved(); }
        }

        public float OriginalWidth
        {
            get { return _originalWidth; }
            set { _originalWidth = value; CalculateBounds(); OnGameObjectMoved(); }
        }

        public float OriginalHeight
        {
            get { return _originalHeight; }
            set { _originalHeight = value; CalculateBounds(); OnGameObjectMoved(); }
        }

        private float _originalWidth;
        private float _originalHeight;
        private Vector2 _position;
        private Vector2 _positionOffset;
        private float _scale = 1.0f;

        public TransformationComponent(Vector2 position, int widthInPixels, int heightInPixels) : this(position, widthInPixels, heightInPixels, HorizontalAnchor.Left, VerticalAnchor.Top) { }
        public TransformationComponent(Vector2 position, int widthInPixels, int heightInPixels, HorizontalAnchor horizontalAnchor, VerticalAnchor verticalAnchor)
        {
            this.Z = 0f;
            this.HorizontalAnchor = horizontalAnchor;
            this.VerticalAnchor = verticalAnchor;

            _originalWidth = (float)widthInPixels;
            _originalHeight = (float)heightInPixels;
            
            this.Position = position;
            this.PositionOffset = Vector2.Zero;

            CalculateBounds();
        }

        private void OnGameObjectMoved()
        {
            // When the component is first created, it won't be attached to a GameObject yet.
            if (this.GameObject != null)
            {
                this.GameObject.MessageManager.TriggerMessage(new GameObjectMovedMessage(this.GameObject));
            }
        }

        private void CalculateBounds()
        {
            // Remember: position is in screen coordinates.
            this.Bounds = new BoundingBox(this.Position.X, this.Position.Y, _originalWidth * _scale, _originalHeight * _scale);
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
                this.Bounds.Y = this.Position.Y + this.Bounds.Height / 2;
            }
            else if (this.VerticalAnchor == VerticalAnchor.Bottom)
            {
                this.Bounds.Y = this.Position.Y + this.Bounds.Height;
            }

            this.Bounds.X += this.PositionOffset.X;
            this.Bounds.Y += this.PositionOffset.Y;
        }
    }
}
