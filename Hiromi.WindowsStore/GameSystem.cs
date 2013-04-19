using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Hiromi.Messaging;
using Hiromi.Processing;

namespace Hiromi
{
    public abstract class GameSystem
    {
        protected Dictionary<int, GameObject> GameObjects;
        protected ProcessManager ProcessManager { get; set; }

        public GameSystem()
        {
            this.ProcessManager = new ProcessManager();
            this.GameObjects = new Dictionary<int, GameObject>();
            MessageService.Instance.AddListener<GameObjectLoadedMessage>(msg => OnGameObjectLoaded((GameObjectLoadedMessage)msg));
        }

        public void Update(GameTime gameTime)
        {
            this.ProcessManager.Update(gameTime);
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

        protected virtual void OnUpdate(GameTime gameTime) { }
        protected virtual void OnDraw(GameTime gameTime) { }
        protected abstract bool IsGameObjectForSystem(GameObject obj);
    }
}
