using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Hiromi.Messaging;

namespace Hiromi
{
    public abstract class Screen
    {
        private InputProcessor _inputProcessor;

        public Color BackgroundColor { get; set; }
        public ProcessManager ProcessManager { get; set; }
        public MessageBus MessageBus { get; set; }

        public Screen()
        {
            _inputProcessor = new InputProcessor();
            _inputProcessor.InputHandler = new NullInputHandler();

            this.BackgroundColor = Color.Black;
            this.ProcessManager = new ProcessManager();
            this.MessageBus = new MessageBus();
        }

        public void SetInputHandler(IInputHandler handler)
        {
            _inputProcessor.InputHandler = handler;
        }

        public void Load()
        {
            this.OnInitialize();
        }

        public void Update(GameTime gameTime)
        {
            _inputProcessor.Process();
            this.MessageBus.ProcessMessages();
            this.ProcessManager.Update(gameTime);
            OnUpdate(gameTime);
        }

        public void Draw(GameTime gameTime)
        {
            GraphicsService.Instance.GraphicsDevice.Clear(this.BackgroundColor);
            OnDraw(gameTime);
        }

        public virtual Screen GetPreviousGameScreen() { return null; }
        protected virtual void OnInitialize() { }
        protected virtual void OnUpdate(GameTime gameTime) { }
        protected virtual void OnDraw(GameTime gameTime) { }
    }
}
