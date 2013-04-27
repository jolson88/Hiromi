using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hiromi;
using Hiromi.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Hiromi.Rendering
{
    public class ButtonRenderingNode : SceneNode
    {
        private ButtonComponent _buttonComponent;

        public ButtonRenderingNode(int gameObjectId, PositionComponent positionComponent, RenderPass renderPass, ButtonComponent buttonComponent)
            : base(gameObjectId, positionComponent, renderPass)
        {
            _buttonComponent = buttonComponent;
        }

        protected override void OnInitialize()
        {
            this.MessageManager.AddListener<RenderingComponentChangedMessage>(OnRenderingComponentChanged);
        }

        protected override void OnDraw(GameTime gameTime)
        {
            // We use Bounds instead of Position as Bounds takes the achor point into account
            GraphicsService.Instance.SpriteBatch.Draw(_buttonComponent.CurrentTexture,
                new Vector2(this.PositionComponent.Bounds.X * GraphicsService.Instance.GraphicsDevice.Viewport.Width,
                    this.PositionComponent.Bounds.Y * GraphicsService.Instance.GraphicsDevice.Viewport.Height),
                Color.White);
        }

        private void OnRenderingComponentChanged(RenderingComponentChangedMessage msg)
        {
            if (msg.GameObject.Id == this.GameObjectId)
            {
                _buttonComponent = msg.GameObject.GetComponent<ButtonComponent>();
            }
        }
    }
}
