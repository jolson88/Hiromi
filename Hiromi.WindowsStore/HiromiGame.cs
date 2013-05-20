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
#if WINDOWS_PHONE
        DrawableAd _ad;
#endif
        GameStateManager _stateManager;
        GraphicsDeviceManager _graphics;

        public HiromiGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
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

        protected abstract GameState GetInitialState();
        protected abstract Vector2 GetDesignedScreenSize();
    }
}
