using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Hiromi
{
    public abstract class GameState
    {
        public ProcessManager ProcessManager { get; set; }
        public MessageManager MessageManager { get; set; }
        public GameObjectManager GameObjectManager { get; set; }

        private List<IGameView> _gameViews;

        public GameState()
        {
            this.ProcessManager = new ProcessManager();
            this.MessageManager = new MessageManager();
            this.GameObjectManager = new GameObjectManager(this.ProcessManager, this.MessageManager);
        }

        public void Load()
        {
            this.OnInitialize();

            _gameViews = new List<IGameView>();
            foreach (var view in LoadGameViews())
            {
                view.Initialize(this.GameObjectManager, this.MessageManager);
                _gameViews.Add(view);
            }

            foreach (var obj in LoadGameObjects())
            {
                this.GameObjectManager.AddGameObject(obj);
            }

            this.RegisterMessageListeners();

            foreach (var view in _gameViews)
            {
                view.OnLoaded();
            }
        }

        public void Update(GameTime gameTime)
        {
            this.ProcessManager.Update(gameTime);
            this.MessageManager.Update(gameTime);

            foreach (var view in _gameViews)
            {
                view.Update(gameTime);
            }

            this.GameObjectManager.Update(gameTime);
            
            OnUpdate(gameTime);
        }

        public void Draw(GameTime gameTime)
        {
            GraphicsService.Instance.GraphicsDevice.Clear(Color.Black);

            foreach (var view in _gameViews)
            {
                view.Draw(gameTime);
            }
        }

        protected virtual void RegisterMessageListeners() { }
        protected virtual void OnInitialize() { }
        protected virtual void OnUpdate(GameTime gameTime) { }
        protected virtual IEnumerable<IGameView> LoadGameViews() { return Enumerable.Empty<IGameView>(); }
        protected abstract IEnumerable<GameObject> LoadGameObjects();
    }
}
