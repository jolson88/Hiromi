using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Hiromi
{
    public abstract class GameScreen
    {
        public ProcessManager ProcessManager { get; set; }
        public MessageManager MessageManager { get; set; }

        public GameScreen()
        {
            this.ProcessManager = new ProcessManager();
            this.MessageManager = new MessageManager();
        }

        public void Load()
        {
            this.OnInitialize();

            // TODO: Remove this once Message Listeners are registered dynamically (replace with this.MessageManager.Register(this))
            this.RegisterMessageListeners();
        }

        public void Update(GameTime gameTime)
        {
            this.ProcessManager.Update(gameTime);
            this.MessageManager.Update(gameTime);
            OnUpdate(gameTime);
        }

        public void Draw(GameTime gameTime)
        {
            GraphicsService.Instance.GraphicsDevice.Clear(Color.Black);
            OnDraw(gameTime);
        }

        public virtual GameScreen GetPreviousGameScreen() { return null; }
        protected virtual void RegisterMessageListeners() { }
        protected virtual void OnInitialize() { }
        protected virtual void OnUpdate(GameTime gameTime) { }
        protected virtual void OnDraw(GameTime gameTime) { }
    }
}
