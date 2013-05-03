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

        public SpriteRenderingNode(int gameObjectId, TransformationComponent transformComponent, RenderPass renderPass, SpriteComponent spriteComponent) 
            : base(gameObjectId, transformComponent, renderPass) 
        {
            _spriteComponent = spriteComponent;
            this.IsVisible = _spriteComponent.IsVisible;
        }

        protected override void OnDraw(GameTime gameTime, SpriteBatch batch)
        {
            if (this.TransformationComponent.TransformedByCamera)
            {
                // Remember, we need to "flip" the scale (as our game engine has Y+ up instead of down
                var scale = new Vector2(1, -1) * this.TransformationComponent.Scale;

                // We use Bounds instead of Position as Bounds takes the achor point into account
                batch.Draw(_spriteComponent.Texture,
                    new Vector2((int)this.TransformationComponent.Bounds.X, (int)this.TransformationComponent.Bounds.Y),
                    null,
                    Color.White,
                    0f,
                    Vector2.Zero,
                    scale,
                    SpriteEffects.None,
                    0f);
            }
            else
            {
                batch.Draw(_spriteComponent.Texture,
                    new Rectangle((int)this.TransformationComponent.Bounds.X,
                        (int)this.TransformationComponent.Bounds.Y,
                        (int)this.TransformationComponent.Bounds.Width,
                        (int)this.TransformationComponent.Bounds.Height),
                    null,
                    Color.White);
            }
        }
    }
}
