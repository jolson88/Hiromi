using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Hiromi
{
    public class GameStateManager
    {
        private GameState _currentState;

        public GameStateManager(GameState initialState)
        {
            LoadState(initialState);
        }

        public void Update(GameTime gameTime)
        {
            _currentState.Update(gameTime);
        }

        public void Draw(GameTime gameTime)
        {
            _currentState.Draw(gameTime);
        }

        private void LoadState(GameState newState)
        {
            _currentState = newState;
            _currentState.Load();

            _currentState.MessageManager.AddListener<RequestChangeStateMessage>(msg => OnRequestChangeState((RequestChangeStateMessage)msg));
            _currentState.MessageManager.QueueMessage(new StateChangedMessage(_currentState));
        }

        private void OnRequestChangeState(RequestChangeStateMessage msg)
        {
            LoadState(msg.State);
        }
    }
}
