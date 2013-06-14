using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Hiromi.Components;

namespace Hiromi.Messaging
{
    public class DisableAdsMessage { }
    public class EnableAdsMessage { }
    public class ScreenSizeChangedMessage { }

    // ************************************
    // **
    // **        STATE MESSAGES
    // **
    // ************************************
    public class RequestScreenChangeMessage
    {
        public Screen Screen { get; set; }

        public RequestScreenChangeMessage(Screen screen)
        {
            this.Screen = screen;
        }
    }

    public class ScreenChangedMessage
    {
        public Screen NewScreen { get; set; }

        public ScreenChangedMessage(Screen screen)
        {
            this.NewScreen = screen;
        }
    }

    public class GameStartedMessage
    {
        public override string ToString()
        {
            return "[System] Starting game";
        }
    }


    // ************************************
    // **
    // **         CAMERA MESSAGES
    // **
    // ************************************
    public class ZoomCameraMessage
    {
        public float ZoomFactor { get; private set; }

        public ZoomCameraMessage(float zoomFactor)
        {
            this.ZoomFactor = zoomFactor;
        }
    }

    public class RotateCameraMessage
    {
        public float RotationInRadians { get; private set; }

        public RotateCameraMessage(float rotationInRadians)
        {
            this.RotationInRadians = rotationInRadians;
        }
    }

    /// <summary>
    /// Offset the point that the Camera is looking at (while preserving the point the Camera is looking at)
    /// </summary>
    public class NudgeCameraMessage
    {
        public Vector2 Translation { get; private set; }

        public NudgeCameraMessage(Vector2 translation)
        {
            this.Translation = translation;
        }
    }
}
