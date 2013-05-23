using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

#if WINDOWS_PHONE
using Microsoft.Advertising;
using Microsoft.Advertising.Mobile.Xna;
#endif

namespace Hiromi
{
    public abstract class HiromiGame : Game
    {
        public Texture2D PauseImage { get { return GetPauseImage(); } }

#if WINDOWS_PHONE
        DrawableAd _ad;
#endif
        GameStateManager _stateManager;
        GraphicsDeviceManager _graphics;
        IAdRenderer _adRenderer;

        public HiromiGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        public void SetAdRenderer(IAdRenderer adRenderer)
        {
            _adRenderer = adRenderer;
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
        }

        void Window_ClientSizeChanged(object sender, EventArgs e)
        {
            _graphics.PreferredBackBufferWidth = this.Window.ClientBounds.Width;
            _graphics.PreferredBackBufferHeight = this.Window.ClientBounds.Height;
            _graphics.ApplyChanges();
            _graphics.GraphicsDevice.Viewport = new Viewport(0, 0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);
            _stateManager.ScreenSizeChanged();

        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            GraphicsService.Instance.GraphicsDevice = this.GraphicsDevice;
            GraphicsService.Instance.DesignedScreenSize = this.GetDesignedScreenSize();
            ContentService.Instance.Content = this.Content;
            _stateManager = new GameStateManager(this, GetInitialState());
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {

        }

        public void InitializeAds(string applicationId, string unitId, Rectangle location)
        {
#if WINDOWS_PHONE
            AdGameComponent.Initialize(this, applicationId);
            this.Components.Add(AdGameComponent.Current);
            _ad = AdGameComponent.Current.CreateAd(unitId, location);
            _ad.ErrorOccurred += ad_ErrorOccurred;
            _ad.Visible = false;
#endif
        }

        public void EnableAds()
        {
#if WINDOWS_PHONE
            AdGameComponent.Current.Enabled = true;
            _ad.Visible = true;
#else
            if (_adRenderer != null)
            {
                _adRenderer.EnableAds();
            }
#endif
        }

#if WINDOWS_PHONE
        void ad_ErrorOccurred(object sender, AdErrorEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine(e.Error.Message);
        }
#endif

        public void DisableAds()
        {
#if WINDOWS_PHONE
            AdGameComponent.Current.Enabled = false;
            _ad.Visible = true;
#else
            if (_adRenderer != null)
            {
                _adRenderer.DisableAds();
            }
#endif
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            _stateManager.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            _stateManager.Draw(gameTime);
            base.Draw(gameTime);
        }

        protected abstract Texture2D GetPauseImage();
        protected abstract GameState GetInitialState();
        protected abstract Vector2 GetDesignedScreenSize();
    }
}
