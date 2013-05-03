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
        public Matrix TransformationMatrix { get; private set; }
        public BoundingBox Bounds { get; private set; }

        private MessageManager _messageManager;
        private float _scale;
        private Vector2 _lookAt;
        private Vector2 _offset;

        public Camera(MessageManager messageManager)
        {
            _scale = 1f;
            _offset = Vector2.Zero;
            _lookAt = GraphicsService.Instance.DesignedScreenSize / 2.0f;

            RebuildBoundingBox();
            RebuildTransformationMatrix();

            _messageManager = messageManager;
            _messageManager.AddListener<ZoomCameraMessage>(OnZoomCamera);
            _messageManager.AddListener<NudgeCameraMessage>(OnMoveCamera);
        }

        private void OnZoomCamera(ZoomCameraMessage msg)
        {
            _scale = msg.ZoomFactor;
            RebuildBoundingBox();
            RebuildTransformationMatrix();
        }

        private void OnMoveCamera(NudgeCameraMessage msg)
        {
            _offset = msg.Translation;
            RebuildBoundingBox();
            RebuildTransformationMatrix();
        }

        private void RebuildBoundingBox()
        {
            var viewSize = GraphicsService.Instance.DesignedScreenSize * _scale;
            var center = _lookAt + _offset;
            var left = center.X - (viewSize.X / 2);
            var top = center.Y + (viewSize.Y / 2);

            this.Bounds = new BoundingBox(left, top, viewSize.X, viewSize.Y);
        }

        private void RebuildTransformationMatrix()
        {
            var designedHeight = GraphicsService.Instance.DesignedScreenSize.Y;
            var clientHeight = GraphicsService.Instance.GraphicsDevice.Viewport.Height;

            // **********************************************************************************************************
            //
            //      Process for converting from World space to Camera space in our engine:
            //          1) Convert from World coordinates to View coordinates
            //                  - A.K.A. How many world units do we shift objects to account for camera?
            //          2) Scale from designed screen size to actual client size (for device-resolution independence
            //          3) Invert Y axis
            //                  - XNA is Y+ down by default. Our engine is Y+ up (like normal cartesian system)
            //          4) After Y flip, move up the client's height
            //                  - Makes our Top-Left in world (0,designedHeight) = Top-Left in client (0,0)
            //
            // **********************************************************************************************************
            var toView = Matrix.CreateTranslation(-this.Bounds.Left, -this.Bounds.Bottom, 0);
            var resolutionAndYFlip = Matrix.CreateScale(clientHeight / designedHeight) * Matrix.CreateScale(1, -1, 1);
            var toClient = Matrix.CreateTranslation(0, clientHeight, 0);

            this.TransformationMatrix = toView * resolutionAndYFlip * toClient;
        }
    }
}
