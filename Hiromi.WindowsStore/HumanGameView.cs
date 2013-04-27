using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Hiromi.Rendering;

namespace Hiromi
{
    public class HumanGameView : IGameView
    {
        protected MessageManager MessageManager { get; private set; }
        protected GameObjectManager GameObjectManager { get; private set; }
        protected ProcessManager ProcessManager { get; private set; }
        protected SceneGraph SceneGraph { get; private set; }

        public void Initialize(GameObjectManager gameObjectManager, MessageManager messageManager)
        {
            this.ProcessManager = new ProcessManager();
            this.GameObjectManager = gameObjectManager;
            this.MessageManager = messageManager;
            this.SceneGraph = new SceneGraph(messageManager);

            OnInitialize();
        }

        public GameViewKind GetKind()
        {
            return GameViewKind.Human;
        }

        public void Draw(GameTime gameTime)
        {
            this.SceneGraph.Draw(gameTime);
            OnDraw(gameTime);
        }

        public void Update(GameTime gameTime)
        {
            this.ProcessManager.Update(gameTime);
            this.SceneGraph.Update(gameTime);
            OnUpdate(gameTime);
        }

        protected virtual void OnInitialize() { }
        protected virtual void OnDraw(GameTime gameTime) { }
        protected virtual void OnUpdate(GameTime gameTime) { }
    }
}
