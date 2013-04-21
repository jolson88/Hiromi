using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Hiromi.Messaging;

namespace Hiromi
{
    public class ScreenManager
    {
        private Screen _currentScreen;

        public ScreenManager(Screen initialScreen)
        {
            LoadScreen(initialScreen);
        }

        public void Update(GameTime gameTime)
        {
            _currentScreen.Update(gameTime);
        }

        public void Draw(GameTime gameTime)
        {
            _currentScreen.Draw(gameTime);
        }

        private void LoadScreen(Screen newScreen)
        {
            _currentScreen = newScreen;
            _currentScreen.Load();

            _currentScreen.MessageManager.AddListener<RequestLoadScreenMessage>(msg => OnRequestLoadScreen((RequestLoadScreenMessage)msg));
            _currentScreen.MessageManager.QueueMessage(new ScreenLoadedMessage(_currentScreen));
        }

        private void OnRequestLoadScreen(RequestLoadScreenMessage msg)
        {
            LoadScreen(msg.Screen);
        }
    }
}
