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

        public ScreenManager()
        {
            MessageService.Instance.AddListener<RequestScreenLoadMessage>(msg => OnRequestScreenLoad((RequestScreenLoadMessage)msg));
        }

        public void Update(GameTime gameTime)
        {
            MessageService.Instance.Update(gameTime);
            _currentScreen.Update(gameTime);
        }

        public void Draw(GameTime gameTime)
        {
            _currentScreen.Draw(gameTime);
        }

        private void OnRequestScreenLoad(RequestScreenLoadMessage msg)
        {
            _currentScreen = msg.RequestedScreen;
            _currentScreen.Load();
            GameObjectService.Instance.InitializeGameObjects();

            MessageService.Instance.QueueMessage(new ScreenLoadedMessage(_currentScreen));
        }
    }
}
