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
    public class ButtonComponent : GameObjectComponent, IRenderAwareComponent
    {
        public RenderPass RenderPass { get { return RenderPass.UserInterfacePass; } }
        public int GameObjectId { get { return this.GameObject.Id; } }
        public TransformationComponent Transform { get { return this.GameObject.Transform; } }

        public Texture2D CurrentTexture { get; set; }
        public Texture2D FocusTexture { get; set; }
        public Texture2D NonFocusTexture { get; set; }

        public ButtonComponent(Texture2D nonFocusTexture, Texture2D focusTexture)
        {
            this.CurrentTexture = nonFocusTexture;
            this.FocusTexture = focusTexture;
            this.NonFocusTexture = nonFocusTexture;
        }

        protected override void OnLoaded()
        {
            this.GameObject.MessageManager.AddListener<PointerExitMessage>(OnPointerExit);
            this.GameObject.MessageManager.AddListener<PointerPressMessage>(OnPointerPress);
            this.GameObject.MessageManager.AddListener<PointerReleaseMessage>(OnPointerRelease);
        }

        public void Draw(GameTime gameTime, SpriteBatch batch)
        {
            // Remember, we need to "flip" the scale (as our game engine has Y+ up instead of down
            var scale = new Vector2(1, -1) * this.Transform.Scale;

            // We use Bounds instead of Position as Bounds takes the achor point into account
            batch.Draw(this.CurrentTexture,
                new Vector2((int)this.Transform.Bounds.X, (int)this.Transform.Bounds.Y),
                null,
                Color.White,
                0f,
                Vector2.Zero,
                scale,
                SpriteEffects.None,
                0f);
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
