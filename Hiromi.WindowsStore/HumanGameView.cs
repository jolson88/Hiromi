using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Hiromi
{
    public class HumanGameView : IGameView
    {
        protected MessageManager MessageManager { get; private set; }
        protected GameObjectManager GameObjectManager { get; private set; }
        protected ProcessManager ProcessManager { get; private set; }
        protected SceneGraph SceneGraph { get; private set; }

        private PointerInputHandler _pointerInputHandler;
        private KeyboardInputHandler _keyboardInputHandler;
        private SoundManager _soundManager;

        public void Initialize(GameObjectManager gameObjectManager, MessageManager messageManager)
        {
            this.ProcessManager = new ProcessManager();
            this.GameObjectManager = gameObjectManager;
            this.MessageManager = messageManager;
            this.SceneGraph = new SceneGraph(messageManager);

            _pointerInputHandler = new PointerInputHandler(this.MessageManager, this.SceneGraph);
            _keyboardInputHandler = new KeyboardInputHandler(this.MessageManager);
            _soundManager = new SoundManager(this.MessageManager);
    
            // Initialize with current state (so previous state from previous view (like a mouse click) doesn't trigger again here)
            _pointerInputHandler.Update(new GameTime());
            _keyboardInputHandler.Update(new GameTime());

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
            _pointerInputHandler.Update(gameTime);
            _keyboardInputHandler.Update(gameTime);

            this.ProcessManager.Update(gameTime);
            OnUpdate(gameTime);
        }

        protected virtual void OnInitialize() { }
        protected virtual void OnDraw(GameTime gameTime) { }
        protected virtual void OnUpdate(GameTime gameTime) { }
    }
}
