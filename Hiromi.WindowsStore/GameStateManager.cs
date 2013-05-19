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
        private HiromiGame _game;
        private GameState _currentState;

        public GameStateManager(HiromiGame game, GameState initialState)
        {
            _game = game;
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

            _currentState.MessageManager.AddListener<RequestChangeStateMessage>(OnRequestChangeState);
            _currentState.MessageManager.AddListener<DisableAdsMessage>(OnDisableAds);
            _currentState.MessageManager.AddListener<EnableAdsMessage>(OnEnableAds);
            _currentState.MessageManager.QueueMessage(new StateChangedMessage(_currentState));
        }

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
