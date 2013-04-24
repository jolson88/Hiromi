using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Hiromi;

namespace Hiromi.Components
{
    public class ButtonComponent : GameObjectComponent
    {
        public Texture2D CurrentTexture { get; set; }
        public Texture2D FocusTexture { get; set; }
        public Texture2D NonFocusTexture { get; set; }

        public ButtonComponent() : this(null, null) { }
        public ButtonComponent(Texture2D nonFocusTexture, Texture2D focusTexture)
        {
            this.CurrentTexture = nonFocusTexture;
            this.FocusTexture = focusTexture;
            this.NonFocusTexture = nonFocusTexture;
        }

        public override void Loaded()
        {
            this.GameObject.MessageManager.AddListener<PointerExitMessage>(msg => OnPointerExit((PointerExitMessage)msg));
            this.GameObject.MessageManager.AddListener<PointerPressMessage>(msg => OnPointerPress((PointerPressMessage)msg));
            this.GameObject.MessageManager.AddListener<PointerReleaseMessage>(msg => OnPointerRelease((PointerReleaseMessage)msg));
        }

        public override void Draw(GameTime gameTime)
        {
            var posComponent = this.GameObject.GetComponent<PositionComponent>();
            GraphicsService.Instance.SpriteBatch.Draw(this.CurrentTexture,
                        new Vector2(posComponent.Bounds.X * GraphicsService.Instance.GraphicsDevice.Viewport.Width,
                            posComponent.Bounds.Y * GraphicsService.Instance.GraphicsDevice.Viewport.Height),
                        Color.White);
        }

        private void OnPointerExit(PointerExitMessage msg)
        {
            if (this.GameObject.Id == msg.GameObjectId)
            {
                if (this.NonFocusTexture != null)
                {
                    this.CurrentTexture = this.NonFocusTexture;
                }
            }
        }

        private void OnPointerPress(PointerPressMessage msg)
        {
            if (this.GameObject.Id == msg.GameObjectId)
            {
                this.GameObject.MessageManager.TriggerMessage(new ButtonPressMessage(this.GameObject.Id));
                if (this.FocusTexture != null)
                {
                    this.CurrentTexture = this.FocusTexture;
                }
            }
        }

        private void OnPointerRelease(PointerReleaseMessage msg)
        {
            if (this.GameObject.Id == msg.GameObjectId)
            {
                if (this.NonFocusTexture != null)
                {
                    this.CurrentTexture = this.NonFocusTexture;
                }
            }
        }
    }
}
