using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hiromi;
using Hiromi.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Hiromi.Systems
{
    public class UISystem : GameSystem
    {
        private SpriteBatch _batch;

        protected override void OnInitialize()
        {
            _batch = new SpriteBatch(GraphicsService.Instance.GraphicsDevice);

            this.MessageManager.AddListener<PointerExitMessage>(msg => OnPointerExit((PointerExitMessage)msg));
            this.MessageManager.AddListener<PointerPressMessage>(msg => OnPointerPress((PointerPressMessage)msg));
            this.MessageManager.AddListener<PointerReleaseMessage>(msg => OnPointerRelease((PointerReleaseMessage)msg));
        }

        protected override void OnDraw(GameTime gameTime)
        {
            _batch.Begin();

            foreach (var obj in this.GameObjects.Values)
            {
                if (IsButton(obj))
                {
                    var posComponent = obj.GetComponent<PositionComponent>();
                    var buttonComponent = obj.GetComponent<ButtonComponent>();

                    _batch.Draw(buttonComponent.CurrentTexture,
                                new Vector2(posComponent.Bounds.X * GraphicsService.Instance.GraphicsDevice.Viewport.Width,
                                    posComponent.Bounds.Y * GraphicsService.Instance.GraphicsDevice.Viewport.Height),
                                Color.White);
                }
                else if (IsLabel(obj))
                {
                    var posComponent = obj.GetComponent<PositionComponent>();
                    var labelComponent = obj.GetComponent<LabelComponent>();

                    _batch.DrawString(labelComponent.Font, labelComponent.Text,
                        new Vector2(posComponent.Bounds.X * GraphicsService.Instance.GraphicsDevice.Viewport.Width,
                            posComponent.Bounds.Y * GraphicsService.Instance.GraphicsDevice.Viewport.Height),
                        labelComponent.TextColor);
                }
            }

            _batch.End();
        }

        private void OnPointerExit(PointerExitMessage msg)
        {
            if (this.GameObjects.Keys.Contains(msg.GameObjectId))
            {
                var obj = this.GameObjects[msg.GameObjectId];
                if (IsButton(obj))
                {
                    var button = obj.GetComponent<ButtonComponent>();
                    if (button.NonFocusTexture != null)
                    {
                        button.CurrentTexture = button.NonFocusTexture;
                    }
                }
            }
        }

        private void OnPointerPress(PointerPressMessage msg)
        {
            if (this.GameObjects.Keys.Contains(msg.GameObjectId))
            {
                var obj = this.GameObjects[msg.GameObjectId];
                if (IsButton(obj))
                {
                    this.MessageManager.TriggerMessage(new ButtonPressMessage(obj.Id));

                    var button = obj.GetComponent<ButtonComponent>();
                    if (button.FocusTexture != null)
                    {
                        button.CurrentTexture = button.FocusTexture;
                    }
                }
            }
        }

        private void OnPointerRelease(PointerReleaseMessage msg)
        {
            if (this.GameObjects.Keys.Contains(msg.GameObjectId))
            {
                var obj = this.GameObjects[msg.GameObjectId];
                if (IsButton(obj))
                {
                    var button = obj.GetComponent<ButtonComponent>();
                    if (button.NonFocusTexture != null)
                    {
                        button.CurrentTexture = button.NonFocusTexture;
                    }
                }
            }
        }

        protected override bool IsGameObjectForSystem(GameObject obj)
        {
            return IsButton(obj) || IsLabel(obj);
        }

        private bool IsButton(GameObject obj)
        {
            return obj.HasComponent<PositionComponent>()
                && obj.HasComponent<ButtonComponent>();
        }

        private bool IsLabel(GameObject obj)
        {
            return obj.HasComponent<PositionComponent>()
                && obj.HasComponent<LabelComponent>();
        }
    }
}
