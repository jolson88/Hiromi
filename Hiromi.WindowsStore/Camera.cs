﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Hiromi
{
    public class Camera
    {
        public Matrix TransformationMatrix { get; private set; }
        public BoundingBox Bounds { get; private set; }

        private MessageManager _messageManager;
        private float _scale;
        private Vector2 _lookAt;
        private Vector2 _offset;
        private float _rotation;

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
            _messageManager.AddListener<RotateCameraMessage>(OnRotateCamera);
            _messageManager.AddListener<ScreenSizeChangedMessage>(OnScreenSizeChanged);
        }

        private void OnScreenSizeChanged(ScreenSizeChangedMessage msg)
        {
            RebuildBoundingBox();
            RebuildTransformationMatrix();
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

        private void OnRotateCamera(RotateCameraMessage msg)
        {
            _rotation = msg.RotationInRadians;
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
            // Nudging, Zooming, and Rotation happen around the origin
            var toView = Matrix.CreateTranslation(-_lookAt.X + _offset.X, -_lookAt.Y + _offset.Y, 0) *
                                Matrix.CreateScale(_scale) * Matrix.CreateRotationZ(_rotation) *
                                Matrix.CreateTranslation(_lookAt.X + _offset.X, _lookAt.Y + _offset.Y, 0);
            var resolutionAndYFlip = AdaptToScreenMatrix() * Matrix.CreateScale(1, -1, 1);
            var toClient = Matrix.CreateTranslation(0, clientHeight, 0);

            this.TransformationMatrix = toView * resolutionAndYFlip * toClient;
        }

        public Matrix AdaptToScreenMatrix()
        {
            var screenAdaptMatrix = Matrix.Identity;

            float displayWidth = (float)(GraphicsService.Instance.GraphicsDevice.Viewport.Width);
            float displayHeight = (float)(GraphicsService.Instance.GraphicsDevice.Viewport.Height);
            float AspectRatioDisplay = displayWidth / displayHeight;
            float AspectRatioBaseWindow = (float)(GraphicsService.Instance.DesignedScreenSize.X) / (float)(GraphicsService.Instance.DesignedScreenSize.Y);

            //first stretch it to the screen ignoring the aspect ratio
            float scaleX = displayWidth / (float)GraphicsService.Instance.DesignedScreenSize.X;
            float scaleY = displayHeight / (float)GraphicsService.Instance.DesignedScreenSize.Y;
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
