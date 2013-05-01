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

        public ButtonRenderingNode(int gameObjectId, TransformationComponent positionComponent, RenderPass renderPass, ButtonComponent buttonComponent)
            : base(gameObjectId, positionComponent, renderPass)
        {
            _buttonComponent = buttonComponent;
        }

        protected override void OnDraw(GameTime gameTime, SpriteBatch batch)
        {
            batch.Draw(_buttonComponent.CurrentTexture,
                new Rectangle((int)(this.TransformationComponent.Bounds.X * GraphicsService.Instance.GraphicsDevice.Viewport.Width),
                    (int)(this.TransformationComponent.Bounds.Y * GraphicsService.Instance.GraphicsDevice.Viewport.Height),
                    (int)(this.TransformationComponent.Bounds.Width * GraphicsService.Instance.GraphicsDevice.Viewport.Width),
                    (int)(this.TransformationComponent.Bounds.Height * GraphicsService.Instance.GraphicsDevice.Viewport.Height)),
                null,
                Color.White);
        }
    }
}
