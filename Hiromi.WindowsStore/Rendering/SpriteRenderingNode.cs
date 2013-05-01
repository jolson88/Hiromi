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
        private SpriteComponent _spriteComponent;

        public SpriteRenderingNode(int gameObjectId, TransformationComponent positionComponent, RenderPass renderPass, SpriteComponent spriteComponent) 
            : base(gameObjectId, positionComponent, renderPass) 
        {
            _spriteComponent = spriteComponent;
            this.IsVisible = _spriteComponent.IsVisible;
        }

        protected override void OnDraw(GameTime gameTime, SpriteBatch batch)
        {
            // We use Bounds instead of Position as Bounds takes the achor point into account
            batch.Draw(_spriteComponent.Texture,
                new Rectangle((int)(this.TransformationComponent.Bounds.X * GraphicsService.Instance.GraphicsDevice.Viewport.Width),
                    (int)(this.TransformationComponent.Bounds.Y * GraphicsService.Instance.GraphicsDevice.Viewport.Height),
                    (int)(this.TransformationComponent.Bounds.Width * GraphicsService.Instance.GraphicsDevice.Viewport.Width),
                    (int)(this.TransformationComponent.Bounds.Height * GraphicsService.Instance.GraphicsDevice.Viewport.Height)),
                null,
                Color.White);
        }
    }
}
