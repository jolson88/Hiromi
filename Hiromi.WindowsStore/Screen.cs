using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Hiromi.Systems;

namespace Hiromi
{
    public abstract class Screen
    {
        public ProcessManager ProcessManager { get; set; }
        public MessageManager MessageManager { get; set; }
        public GameObjectManager GameObjectManager { get; set; }

        private List<GameSystem> _systems;
        
        public Screen()
        {
            this.ProcessManager = new ProcessManager();
            this.MessageManager = new MessageManager();
            this.GameObjectManager = new GameObjectManager(this.ProcessManager, this.MessageManager);
        }

        public void Load()
        {
            _systems = new List<GameSystem>();
            _systems.AddRange(LoadGameSystems());

            foreach (var obj in LoadGameObjects())
            {
                this.GameObjectManager.AddGameObject(obj);
            }

            foreach (var sys in _systems)
            {
                sys.Initialize(this.ProcessManager, this.MessageManager, this.GameObjectManager);
            }

            this.RegisterMessageListeners();
        }

        public void Update(GameTime gameTime)
        {
            this.ProcessManager.Update(gameTime);
            this.MessageManager.Update(gameTime);

            foreach (var sys in _systems)
            {
                sys.Update(gameTime);
            }

            foreach (var obj in this.GameObjectManager.GetAllGameObjects())
            {
                obj.Update(gameTime);
            }
        }

        public void Draw(GameTime gameTime)
        {
            GraphicsService.Instance.GraphicsDevice.Clear(Color.Fuchsia);
            GraphicsService.Instance.SpriteBatch.Begin();

            foreach (var sys in _systems)
            {
                sys.Draw(gameTime);
            }

            // Draw game objects furthest away first
            var objects = this.GameObjectManager.GetAllGameObjects();
            objects.Sort(CompareGameObjectsByDepth);
            foreach (var obj in objects)
            {
                obj.Draw(gameTime);
            }

            GraphicsService.Instance.SpriteBatch.End();
        }

        protected virtual void RegisterMessageListeners() { }
        protected virtual IEnumerable<GameSystem> LoadGameSystems() { return Enumerable.Empty<GameSystem>(); }
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
