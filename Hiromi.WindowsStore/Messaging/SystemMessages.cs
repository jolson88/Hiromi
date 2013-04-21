using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hiromi.Messaging
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
}
