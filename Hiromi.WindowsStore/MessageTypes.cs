using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Hiromi.Components;
using Hiromi.Rendering;

namespace Hiromi
{
    // ************************************
    // **
    // **        STATE MESSAGES
    // **
    // ************************************
    public class RequestChangeStateMessage : Message
    {
        public GameState State { get; set; }

        public RequestChangeStateMessage(GameState state)
        {
            this.State = state;
        }
    }

    public class StateChangedMessage : Message
    {
        public GameState State { get; set; }

        public StateChangedMessage(GameState state)
        {
            this.State = state;
        }
    }

    public class GameStartedMessage : Message
    {
        public override string ToString()
        {
            return "[System] Starting game";
        }
    }


    // ************************************
    // **
    // **   OBJECT + COMPONENT MESSAGES
    // **
    // ************************************
    public class NewGameObjectMessage : Message
    {
        public GameObject GameObject { get; set; }

        public NewGameObjectMessage(GameObject obj)
        {
            this.GameObject = obj;
        }
    }

    public class GameObjectLoadedMessage : Message
    {
        public GameObject GameObject { get; set; }

        public GameObjectLoadedMessage(GameObject obj)
        {
            this.GameObject = obj;
        }
    }

    public class GameObjectRemovedMessage : Message
    {
        public int GameObjectId { get; set; }

        public GameObjectRemovedMessage(int gameObjectId)
        {
            this.GameObjectId = gameObjectId;
        }
    }

    public class GameObjectMovedMessage : Message
    {
        public GameObject GameObject { get; set; }

        public GameObjectMovedMessage(GameObject obj)
        {
            this.GameObject = obj;
        }
    }

    public class NewRenderingComponentMessage : Message
    {
        public IRenderingComponent RenderingComponent { get; set; }

        public NewRenderingComponentMessage(IRenderingComponent renderingComponent)
        {
            this.RenderingComponent = renderingComponent;
        }
    }

    public class RenderingComponentChangedMessage : Message
    {
        public GameObject GameObject { get; set; }
        public IRenderingComponent RenderingComponent { get; set; }

        public RenderingComponentChangedMessage(GameObject gameObject, IRenderingComponent renderingComponent)
        {
            this.GameObject = gameObject;
            this.RenderingComponent = renderingComponent;
        }
    }


    // ************************************
    // **
    // **         CAMERA MESSAGES
    // **
    // ************************************
    public class ZoomCameraMessage : Message
    {
        public float ZoomFactor { get; private set; }

        public ZoomCameraMessage(float zoomFactor)
        {
            this.ZoomFactor = zoomFactor;
        }
    }

    public class RotateCameraMessage : Message
    {
        public float RotationInRadians { get; private set; }

        public RotateCameraMessage(float rotationInRadians)
        {
            this.RotationInRadians = rotationInRadians;
        }
    }

    public class MoveCameraMessage : Message
    {
        public Vector2 Translation { get; private set; }

        public MoveCameraMessage(Vector2 translation)
        {
            this.Translation = translation;
        }
    }

    // ************************************
    // **
    // **        INPUT MESSAGES
    // **
    // ************************************
    public class KeyDownMessage : Message
    {
        public Keys Key { get; set; }
        public KeyDownMessage(Keys key) { this.Key = key; }

        public override MessageVerbosity GetMessageVerbosity()
        {
            return MessageVerbosity.Noise;
        }
    }

    public class KeyUpMessage : Message
    {
        public Keys Key { get; set; }
        public KeyUpMessage(Keys key) { this.Key = key; }

        public override MessageVerbosity GetMessageVerbosity()
        {
            return MessageVerbosity.Noise;
        }
    }

    public class PointerEnterMessage : Message
    {
        public int GameObjectId { get; set; }
        public PointerEnterMessage(int gameObjectId) { this.GameObjectId = gameObjectId; }

        public override MessageVerbosity GetMessageVerbosity()
        {
            return MessageVerbosity.Noise;
        }
    }

    public class PointerExitMessage : Message
    {
        public int GameObjectId { get; set; }
        public PointerExitMessage(int gameObjectId) { this.GameObjectId = gameObjectId; }

        public override MessageVerbosity GetMessageVerbosity()
        {
            return MessageVerbosity.Noise;
        }
    }

    public class PointerPressMessage : Message
    {
        public int GameObjectId { get; set; }
        public PointerPressMessage(int gameObjectId) { this.GameObjectId = gameObjectId; }

        public override MessageVerbosity GetMessageVerbosity()
        {
            return MessageVerbosity.Noise;
        }
    }

    public class PointerReleaseMessage : Message
    {
        public int GameObjectId { get; set; }
        public PointerReleaseMessage(int gameObjectId) { this.GameObjectId = gameObjectId; }

        public override MessageVerbosity GetMessageVerbosity()
        {
            return MessageVerbosity.Noise;
        }
    }

    public class ButtonPressMessage : Message
    {
        public int GameObjectId { get; set; }
        public ButtonPressMessage(int gameObjectId) { this.GameObjectId = gameObjectId; }

        public override MessageVerbosity GetMessageVerbosity()
        {
            return MessageVerbosity.Noise;
        }
    }
}
