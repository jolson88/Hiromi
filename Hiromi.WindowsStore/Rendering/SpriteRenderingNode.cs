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
    public class SpriteRenderingNode : SceneNode
    {
        private SpriteBatch _localBatch;
        private SpriteComponent _spriteComponent;

        public SpriteRenderingNode(int gameObjectId, PositionComponent positionComponent, RenderPass renderPass, SpriteComponent spriteComponent) 
            : base(gameObjectId, positionComponent, renderPass) 
        {
            _spriteComponent = spriteComponent;
            this.IsVisible = _spriteComponent.IsVisible;

            if (!spriteComponent.TransformedByCamera)
            {
                // Use personal sprite batch so camera transformations don't effect this image
                _localBatch = new SpriteBatch(GraphicsService.Instance.GraphicsDevice);
            }
        }

        protected override void OnDraw(GameTime gameTime, SceneGraph scene)
        {
            var batch = scene.SpriteBatch;
            if (_localBatch != null)
            {
                _localBatch.Begin();
                batch = _localBatch;
            }

            // We use Bounds instead of Position as Bounds takes the achor point into account
            batch.Draw(_spriteComponent.Texture,
                new Rectangle((int)(this.PositionComponent.Bounds.X * GraphicsService.Instance.GraphicsDevice.Viewport.Width),
                    (int)(this.PositionComponent.Bounds.Y * GraphicsService.Instance.GraphicsDevice.Viewport.Height),
                    (int)(this.PositionComponent.Bounds.Width * GraphicsService.Instance.GraphicsDevice.Viewport.Width),
                    (int)(this.PositionComponent.Bounds.Height * GraphicsService.Instance.GraphicsDevice.Viewport.Height)),
                null,
                Color.White);

            if (_localBatch != null)
            {
                _localBatch.End();
            }
        }
    }
}
