using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Hiromi
{
    // TODO: Future Improvements: A baked-in command console, generic playing of sounds (via event), rendering via scene graph, etc.
    public class HumanGameView : IGameView
    {
        protected MessageManager _messageManager;
        protected GameObjectManager _gameObjectManager;
        protected ProcessManager _processManager;

        public HumanGameView(MessageManager messageManager, GameObjectManager gameObjectManager)
        {
            _messageManager = messageManager;
            _gameObjectManager = gameObjectManager;
            _processManager = new ProcessManager();
        }

        public GameViewKind GetKind()
        {
            return GameViewKind.Human;
        }

        public void Draw(GameTime gameTime)
        {
            OnDraw(gameTime);
        }

        public void Update(GameTime gameTime)
        {
            _processManager.Update(gameTime);
            OnUpdate(gameTime);
        }

        protected virtual void OnDraw(GameTime gameTime) { }
        protected virtual void OnUpdate(GameTime gameTime) { }
    }
}
