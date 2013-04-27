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

        private SceneGraph _sceneGraph;

        // TODO: Move message manager and game object manager into Initialize method to enforce called by GameState
        // This is to gaurantee derived classes don't forget to call this constructor (or force these parameters to spread throughout the codebase)
        public HumanGameView(MessageManager messageManager, GameObjectManager gameObjectManager)
        {
            this.MessageManager = messageManager;
            this.GameObjectManager = gameObjectManager;
            this.ProcessManager = new ProcessManager();
            _sceneGraph = new SceneGraph(this.MessageManager);
        }

        public GameViewKind GetKind()
        {
            return GameViewKind.Human;
        }

        public void Draw(GameTime gameTime)
        {
            _sceneGraph.Draw(gameTime);
            OnDraw(gameTime);
        }

        public void Update(GameTime gameTime)
        {
            this.ProcessManager.Update(gameTime);
            _sceneGraph.Update(gameTime);
            OnUpdate(gameTime);
        }

        protected virtual void OnDraw(GameTime gameTime) { }
        protected virtual void OnUpdate(GameTime gameTime) { }
    }
}
