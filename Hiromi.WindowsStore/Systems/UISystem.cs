using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hiromi;
using Hiromi.Components;
using Hiromi.Messaging;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Hiromi.Systems
{
    public class UISystem : GameSystem
    {
        private SpriteBatch _batch;

        public UISystem()
        {
            _batch = new SpriteBatch(GraphicsService.Instance.GraphicsDevice);

            MessageService.Instance.AddListener<PointerExitMessage>(msg => OnPointerExit((PointerExitMessage)msg));
            MessageService.Instance.AddListener<PointerPressMessage>(msg => OnPointerPress((PointerPressMessage)msg));
            MessageService.Instance.AddListener<PointerReleaseMessage>(msg => OnPointerRelease((PointerReleaseMessage)msg));
        }

        protected override void OnDraw(GameTime gameTime)
        {
            _batch.Begin();
            foreach (var obj in this.GameObjects.Values)
            {
                var posComponent = obj.GetComponent<PositionComponent>();
                var spriteComponent = obj.GetComponent<SpriteComponent>();

                if (spriteComponent.IsVisible)
                {
                    _batch.Draw(spriteComponent.Sprite.Texture,
                        new Vector2(posComponent.Position.X * GraphicsService.Instance.GraphicsDevice.Viewport.Width,
                            posComponent.Position.Y * GraphicsService.Instance.GraphicsDevice.Viewport.Height),
                        Color.White);
                }
            }
            _batch.End();
        }

        private void OnPointerExit(PointerExitMessage msg)
        {
            if (this.GameObjects.Keys.Contains(msg.GameObjectId))
            {
                var obj = this.GameObjects[msg.GameObjectId];
                var button = obj.GetComponent<ButtonComponent>();
                var sprite = obj.GetComponent<SpriteComponent>();

                if (button.NonFocusSprite != null)
                {
                    sprite.Sprite = button.NonFocusSprite;
                }
            }
        }

        private void OnPointerPress(PointerPressMessage msg)
        {
            if (this.GameObjects.Keys.Contains(msg.GameObjectId))
            {
                var obj = this.GameObjects[msg.GameObjectId];
                var button = obj.GetComponent<ButtonComponent>();
                var sprite = obj.GetComponent<SpriteComponent>();

                MessageService.Instance.TriggerMessage(new ButtonPressMessage(obj.Id));
                if (button.FocusSprite != null)
                {
                    sprite.Sprite = button.FocusSprite;
                }
            }
        }

        private void OnPointerRelease(PointerReleaseMessage msg)
        {
            if (this.GameObjects.Keys.Contains(msg.GameObjectId))
            {
                var obj = this.GameObjects[msg.GameObjectId];
                var button = obj.GetComponent<ButtonComponent>();
                var sprite = obj.GetComponent<SpriteComponent>();

                if (button.NonFocusSprite != null)
                {
                    sprite.Sprite = button.NonFocusSprite;
                }
            }
        }

        protected override bool IsGameObjectForSystem(GameObject obj)
        {
            return obj.HasComponent<PositionComponent>()
                && obj.HasComponent<SpriteComponent>() 
                && obj.HasComponent<ButtonComponent>();
        }
    }
}
