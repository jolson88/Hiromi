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
    public class LabelRenderingNode : SceneNode
    {
        private LabelComponent _labelComponent;

        public LabelRenderingNode(int gameObjectId, PositionComponent positionComponent, RenderPass renderPass, LabelComponent labelComponent)
            : base(gameObjectId, positionComponent, renderPass)
        {
            _labelComponent = labelComponent;
        }

        protected override void OnDraw(GameTime gameTime, SceneGraph scene)
        {
            scene.SpriteBatch.DrawString(_labelComponent.Font, _labelComponent.Text,
                new Vector2(this.PositionComponent.Bounds.X * GraphicsService.Instance.GraphicsDevice.Viewport.Width,
                    this.PositionComponent.Bounds.Y * GraphicsService.Instance.GraphicsDevice.Viewport.Height),
                _labelComponent.TextColor);
        }
    }
}
