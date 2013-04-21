using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Hiromi.Messaging;
using Hiromi.Processing;
using Hiromi.Systems;

namespace Hiromi
{
    public abstract class Screen
    {
        public MessageManager MessageManager { get; set; }
        public GameObjectManager GameObjectManager { get; set; }

        private List<GameSystem> _systems;
        
        public Screen()
        {
            this.MessageManager = new MessageManager();
            this.GameObjectManager = new GameObjectManager(this.MessageManager);
        }

        public void Load()
        {
            _systems = new List<GameSystem>();
            _systems.Add(new GeneralInputSystem());
            _systems.Add(new UISystem());
            _systems.Add(new BackgroundRenderingSystem());
            _systems.Add(new SimplePhysicsSystem());
            _systems.Add(new SpriteRendererSystem());
            _systems.AddRange(LoadGameSystems());

            foreach (var obj in LoadGameObjects())
            {
                this.GameObjectManager.AddGameObject(obj);
            }

            foreach (var sys in _systems)
            {
                sys.Initialize(this.MessageManager, this.GameObjectManager);
            }

            this.RegisterMessageListeners();
        }

        public void Update(GameTime gameTime)
        {
            this.MessageManager.Update(gameTime);

            foreach (var sys in _systems)
            {
                sys.Update(gameTime);
            }
        }

        public void Draw(GameTime gameTime)
        {
            foreach (var sys in _systems)
            {
                sys.Draw(gameTime);
            }
        }

        protected virtual void RegisterMessageListeners() { }
        protected abstract List<GameSystem> LoadGameSystems();
        protected abstract List<GameObject> LoadGameObjects();
    }
}
