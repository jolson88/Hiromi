using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hiromi.Messaging
{
    public class GameObjectLoadedMessage : Message
    {
        public GameObject GameObject { get; set; }

        public GameObjectLoadedMessage(GameObject obj)
        {
            this.GameObject = obj;
        }
    }

    /// <summary>
    /// Message requesting a new screen to be loaded
    /// </summary>
    public class RequestScreenLoadMessage : Message
    {
        public Screen RequestedScreen { get; set; }

        public RequestScreenLoadMessage(Screen requestedScreen)
        {
            this.RequestedScreen = requestedScreen;
        }
    }

    /// <summary>
    /// Message from the system when a new screen is loaded
    /// </summary>
    public class ScreenLoadedMessage : Message
    {
        public Screen LoadedScreen { get; set; }

        public ScreenLoadedMessage(Screen loadedScreen)
        {
            this.LoadedScreen = loadedScreen;
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
}
