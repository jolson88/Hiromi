using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;

namespace Hiromi
{
    public class RequestLoadScreenMessage : Message
    {
        public Screen Screen { get; set; }

        public RequestLoadScreenMessage(Screen screen)
        {
            this.Screen = screen;
        }
    }

    public class ScreenLoadedMessage : Message
    {
        public Screen Screen { get; set; }

        public ScreenLoadedMessage(Screen screen)
        {
            this.Screen = screen;
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

    /// <summary>
    /// Message from the system to start a game.
    /// </summary>
    public class GameStartedMessage : Message
    {
        public override string ToString()
        {
            return "[System] Starting game";
        }
    }

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
