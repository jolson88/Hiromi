using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Hiromi
{
    public abstract class GameSystem
    {
        protected Dictionary<int, GameObject> GameObjects;
        
        protected ProcessManager ProcessManager { get; set; }
        protected MessageManager MessageManager { get; set; }
        protected GameObjectManager GameObjectManager { get; set; }

        public GameSystem()
        {
            this.GameObjects = new Dictionary<int, GameObject>();
        }

        public void Initialize(ProcessManager processManager, MessageManager messageManager, GameObjectManager gameObjectManager)
        {
            this.ProcessManager = processManager;
            this.MessageManager = messageManager;
            this.GameObjectManager = gameObjectManager;

            this.MessageManager.AddListener<GameObjectLoadedMessage>(msg => OnGameObjectLoaded((GameObjectLoadedMessage)msg));
            this.OnInitialize();
        }

        public void Update(GameTime gameTime)
        {
            this.OnUpdate(gameTime);
        }

        public void Draw(GameTime gameTime)
        {
            this.OnDraw(gameTime);
        }

        private void OnGameObjectLoaded(GameObjectLoadedMessage msg)
        {
            if (IsGameObjectForSystem(msg.GameObject))
            {
                this.GameObjects.Add(msg.GameObject.Id, msg.GameObject);
            }
        }

        protected virtual void OnInitialize() { }
        protected virtual void OnUpdate(GameTime gameTime) { }
        protected virtual void OnDraw(GameTime gameTime) { }
        protected virtual bool IsGameObjectForSystem(GameObject obj) { return false; }
    }
}
