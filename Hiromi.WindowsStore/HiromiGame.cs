using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Hiromi
{
    public abstract class HiromiGame : Game
    {
        GraphicsDeviceManager _graphics;
        GameScreen _currentScreen;

        public HiromiGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        public void LoadScreen(GameScreen newScreen)
        {
            _currentScreen = newScreen;
            _currentScreen.Load();

            // TODO: Replace with _currentState.MessageManager.Register(this) when change is made
            _currentScreen.MessageManager.AddListener<RequestChangeStateMessage>(OnRequestChangeState);
            _currentScreen.MessageManager.QueueMessage(new StateChangedMessage(_currentScreen));
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();
            this.Window.ClientSizeChanged += Window_ClientSizeChanged;

            LoadScreen(GetInitialScreen());
        }

        protected void Window_ClientSizeChanged(object sender, EventArgs e)
        {
            _graphics.PreferredBackBufferWidth = this.Window.ClientBounds.Width;
            _graphics.PreferredBackBufferHeight = this.Window.ClientBounds.Height;
            _graphics.ApplyChanges();
            _graphics.GraphicsDevice.Viewport = new Viewport(0, 0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);
            _currentScreen.MessageManager.QueueMessage(new ScreenSizeChangedMessage());
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            GraphicsService.Instance.GraphicsDevice = this.GraphicsDevice;
            ContentService.Instance.Content = this.Content;
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {

        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            _currentScreen.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            _currentScreen.Draw(gameTime);
            base.Draw(gameTime);
        }

        private void OnRequestChangeState(RequestChangeStateMessage msg)
        {
            LoadScreen(msg.State);
        }

        protected abstract GameScreen GetInitialScreen();
    }
}
