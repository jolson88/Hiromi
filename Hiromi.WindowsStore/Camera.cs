using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Hiromi.Messaging;

namespace Hiromi
{
    public class Camera
    {
        public Matrix TransformationMatrix { get; private set; }
        private MessageManager _messageManager;
        private float _scale;
        private Vector2 _lookAt;
        private Vector2 _offset;
        private float _rotation;
        private Vector2 _designedScreenSize;

        // TODO: Remove MessageManager once listeners are registered declaritively via attributes
        public Camera(MessageManager messageManager, Vector2 designedScreenSize)
        {
            _designedScreenSize = designedScreenSize;
            _scale = 1f;
            _offset = Vector2.Zero;
            _lookAt = _designedScreenSize / 2.0f;

            RebuildTransformationMatrix();

            _messageManager = messageManager;
            _messageManager.AddListener<ZoomCameraMessage>(OnZoomCamera);
            _messageManager.AddListener<NudgeCameraMessage>(OnMoveCamera);
            _messageManager.AddListener<RotateCameraMessage>(OnRotateCamera);
            _messageManager.AddListener<ScreenSizeChangedMessage>(OnScreenSizeChanged);
        }

        private void OnScreenSizeChanged(ScreenSizeChangedMessage msg)
        {
            RebuildTransformationMatrix();
        }

        private void OnZoomCamera(ZoomCameraMessage msg)
        {
            _scale = msg.ZoomFactor;
            RebuildTransformationMatrix();
        }

        private void OnMoveCamera(NudgeCameraMessage msg)
        {
            _offset = msg.Translation;
            RebuildTransformationMatrix();
        }

        private void OnRotateCamera(RotateCameraMessage msg)
        {
            _rotation = msg.RotationInRadians;
            RebuildTransformationMatrix();
        }

        private void RebuildTransformationMatrix()
        {
            var designedHeight = _designedScreenSize.Y;
            var clientHeight = GraphicsService.Instance.GraphicsDevice.Viewport.Height;

            // **********************************************************************************************************
            // 
            // Build View Matrix:
            //      - Translate (so rotations and scaling treat center camera point as origin)
            //      - Rotate
            //      - Scale
            //      - Translate (to center back on what we are looking at)
            //      - Adapt and letter-box (if appropriate) for different aspect ratios
            //
            // (This is the opposite of a normal World matrix, which is SRT)
            //
            this.TransformationMatrix = Matrix.CreateTranslation(-_lookAt.X + _offset.X, -_lookAt.Y + _offset.Y, 0) *
                                Matrix.CreateScale(_scale) * Matrix.CreateRotationZ(_rotation) *
                                Matrix.CreateTranslation(_lookAt.X + _offset.X, _lookAt.Y + _offset.Y, 0) *
                                AdaptToScreenMatrix();
        }

        public Matrix AdaptToScreenMatrix()
        {
            var screenAdaptMatrix = Matrix.Identity;

            float displayWidth = (float)(GraphicsService.Instance.GraphicsDevice.Viewport.Width);
            float displayHeight = (float)(GraphicsService.Instance.GraphicsDevice.Viewport.Height);
            float AspectRatioDisplay = displayWidth / displayHeight;
            float AspectRatioBaseWindow = (float)(_designedScreenSize.X) / (float)(_designedScreenSize.Y);

            //first stretch it to the screen ignoring the aspect ratio
            float scaleX = displayWidth / (float)_designedScreenSize.X;
            float scaleY = displayHeight / (float)_designedScreenSize.Y;
            screenAdaptMatrix = Matrix.CreateScale(scaleX, scaleY, 1.0f);

            float offsetX = 0.0f;
            float offsetY = 0.0f;

            // now adapt to your native aspect ratio, but keep maximum possible zoom:
            if (AspectRatioDisplay > AspectRatioBaseWindow)
            {
                scaleX = AspectRatioBaseWindow / AspectRatioDisplay;     // smaller than 1
                offsetX = (displayWidth - scaleX * displayWidth) * 0.5f; //shifting towards the center 
                //(blackbars)

                screenAdaptMatrix = screenAdaptMatrix * Matrix.CreateScale(scaleX, 1.0f, 1.0f) *
                                                        Matrix.CreateTranslation(offsetX, 0.0f, 0.0f);
            }
            else if (AspectRatioDisplay < AspectRatioBaseWindow)
            {
                scaleY = AspectRatioDisplay / AspectRatioBaseWindow;        // inverse ratios, smaller than 1
                offsetY = (displayHeight - scaleY * displayHeight) * 0.5f; //shifting towards the center (black 
                //bars)
                screenAdaptMatrix = screenAdaptMatrix * Matrix.CreateScale(1.0f, scaleY, 1.0f) *
                                                        Matrix.CreateTranslation(0.0f, offsetY, 0.0f);
            }

            return screenAdaptMatrix;
        }
    }
}
