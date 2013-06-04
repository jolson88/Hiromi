using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Hiromi
{
    public class GameStateManager
    {
        private bool _isPaused = false;
        private SpriteBatch _batch;
        private HiromiGame _game;
        private GameState _currentState;

        public GameStateManager(HiromiGame game, GameState initialState)
        {
            _game = game;
            _batch = new SpriteBatch(GraphicsService.Instance.GraphicsDevice);

            LoadState(initialState);
        }

        public void Update(GameTime gameTime)
        {
            if (!_isPaused)
            {
                _currentState.Update(gameTime);
            }
        }

        public void Draw(GameTime gameTime)
        {
            if (_isPaused)
            {
                _batch.Begin();
                _batch.Draw(_game.PauseImage, new Rectangle(0, 0, GraphicsService.Instance.GraphicsDevice.Viewport.Width, GraphicsService.Instance.GraphicsDevice.Viewport.Height), Color.White);
                _batch.End();
            }
            else
            {
                _currentState.Draw(gameTime);
            }
        }

        public void ScreenSizeChanged()
        {
            if (GraphicsService.Instance.GraphicsDevice.Viewport.Width < 400)
            {
                _isPaused = true;
            }
            else
            {
                _isPaused = false;
                _currentState.MessageManager.QueueMessage(new ScreenSizeChangedMessage());
            }
        }

        // TODO: Clean up how previous states are rolled back all the way from the root XAML
        public GameState GetPreviousGameState()
        {
            return _currentState.GetPreviousGameState();
        }

        public void LoadState(GameState newState)
        {
            _currentState = newState;
            _currentState.Load();

            _currentState.MessageManager.AddListener<RequestChangeStateMessage>(OnRequestChangeState);
            _currentState.MessageManager.AddListener<DisableAdsMessage>(OnDisableAds);
            _currentState.MessageManager.AddListener<EnableAdsMessage>(OnEnableAds);
            _currentState.MessageManager.QueueMessage(new StateChangedMessage(_currentState));
        }

        // TODO: Clean up the difference in showing ads between Phone and Windows
        private void OnDisableAds(DisableAdsMessage msg)
        {
            _game.DisableAds();
        }

        private void OnEnableAds(EnableAdsMessage msg)
        {
            _game.EnableAds();
        }

        private void OnRequestChangeState(RequestChangeStateMessage msg)
        {
            LoadState(msg.State);
        }
    }
}
