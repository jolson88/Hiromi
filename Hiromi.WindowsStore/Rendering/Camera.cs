using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Hiromi.Rendering
{
    public class Camera
    {
        public Matrix Transformation { get; private set; }

        private MessageManager _messageManager;
        private float _scale;
        private float _rotation;
        private Vector2 _offset;

        public Camera(MessageManager messageManager)
        {
            _scale = 1f;
            _rotation = 0f;
            _offset = Vector2.Zero;

            RebuildTransformationMatrix();

            _messageManager = messageManager;
            _messageManager.AddListener<RotateCameraMessage>(OnRotateCamera);
            _messageManager.AddListener<ZoomCameraMessage>(OnZoomCamera);
            _messageManager.AddListener<MoveCameraMessage>(OnMoveCamera);
        }

        private void OnRotateCamera(RotateCameraMessage msg)
        {
            _rotation = msg.RotationInRadians;
            RebuildTransformationMatrix();
        }

        private void OnZoomCamera(ZoomCameraMessage msg)
        {
            _scale = msg.ZoomFactor;
            RebuildTransformationMatrix();
        }

        private void OnMoveCamera(MoveCameraMessage msg)
        {
            _offset = msg.Translation;
            RebuildTransformationMatrix();
        }

        private void RebuildTransformationMatrix()
        {
            this.Transformation = Matrix.CreateTranslation(-GraphicsService.Instance.GraphicsDevice.Viewport.Width * 0.5f, -GraphicsService.Instance.GraphicsDevice.Viewport.Height * 0.5f, 0f) *
                Matrix.CreateScale(_scale) *
                Matrix.CreateRotationZ(_rotation) *
                Matrix.CreateTranslation(GraphicsService.Instance.GraphicsDevice.Viewport.Width * 0.5f, GraphicsService.Instance.GraphicsDevice.Viewport.Height * 0.5f, 0f) *
                Matrix.CreateTranslation(_offset.X, _offset.Y, 0f);
        }
    }
}
