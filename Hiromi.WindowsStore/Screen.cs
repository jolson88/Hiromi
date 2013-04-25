using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Hiromi
{
    public abstract class Screen
    {
        public ProcessManager ProcessManager { get; set; }
        public MessageManager MessageManager { get; set; }
        public GameObjectManager GameObjectManager { get; set; }

        private List<IGameView> _gameViews;
        
        public Screen()
        {
            this.ProcessManager = new ProcessManager();
            this.MessageManager = new MessageManager();
            this.GameObjectManager = new GameObjectManager(this.ProcessManager, this.MessageManager);

            this.OnInitialize();
        }

        public void Load()
        {
            _gameViews = new List<IGameView>();
            _gameViews.AddRange(LoadGameViews());

            foreach (var obj in LoadGameObjects())
            {
                this.GameObjectManager.AddGameObject(obj);
            }

            this.RegisterMessageListeners();
        }

        public void Update(GameTime gameTime)
        {
            this.ProcessManager.Update(gameTime);
            this.MessageManager.Update(gameTime);

            foreach (var view in _gameViews)
            {
                view.Update(gameTime);
            }

            foreach (var obj in this.GameObjectManager.GetAllGameObjects())
            {
                obj.Update(gameTime);
            }

            OnUpdate(gameTime);
        }

        public void Draw(GameTime gameTime)
        {
            GraphicsService.Instance.GraphicsDevice.Clear(Color.Fuchsia);
            GraphicsService.Instance.SpriteBatch.Begin();

            // TODO: Push up into views
            var objects = this.GameObjectManager.GetAllGameObjects();
            objects.Sort(CompareGameObjectsByDepth);
            foreach (var obj in objects)
            {
                obj.Draw(gameTime);
            }

            foreach (var view in _gameViews)
            {
                view.Draw(gameTime);
            }

            OnDraw(gameTime);
            GraphicsService.Instance.SpriteBatch.End();
        }

        protected virtual void RegisterMessageListeners() { }
        protected virtual void OnInitialize() { }
        protected virtual void OnUpdate(GameTime gameTime) { }
        protected virtual void OnDraw(GameTime gameTime) { }
        protected virtual IEnumerable<IGameView> LoadGameViews() { return Enumerable.Empty<IGameView>(); }
        protected abstract IEnumerable<GameObject> LoadGameObjects();

        private static int CompareGameObjectsByDepth(GameObject x, GameObject y)
        {
            if (x == null && y == null) return 0;
            if (x == null && y != null) return -1;
            if (y == null && x != null) return 1;

            // Higher depths (further away from camera) are smaller (and should be rendered first)
            return y.Depth - x.Depth;
        }
    }
}
