using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Hiromi.Rendering;

namespace Hiromi.Components
{
    // TODO: Remove when BackgroundComponent is extracted back out
    public enum SpriteKind
    {
        GameObject = 0,
        Background = 1,
        UserInterface = 2
    }

    public class SpriteComponent : GameObjectComponent, IRenderAwareComponent
    {
        public int GameObjectId { get { return this.GameObject.Id; } }
        public RenderPass RenderPass { get { return RenderPass.GameObjectPass; } }
        public TransformationComponent Transform { get { return this.GameObject.Transform; } }
        public bool IsVisible { get; set; }
        public Texture2D Texture { get; set; }

        public SpriteComponent(Texture2D texture)
        {
            this.Texture = texture;
            this.IsVisible = true;
        }

        public void Draw(GameTime gameTime, SpriteBatch batch)
        {
            if (this.GameObject.Transform.TransformedByCamera)
            {
                // Remember, we need to "flip" the scale (as our game engine has Y+ up instead of down
                var scale = new Vector2(1, -1) * this.GameObject.Transform.Scale;

                // We use Bounds instead of Position as Bounds takes the achor point into account
                batch.Draw(this.Texture,
                    new Vector2((int)this.GameObject.Transform.Bounds.X, (int)this.GameObject.Transform.Bounds.Y),
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
                batch.Draw(this.Texture,
                    new Rectangle((int)this.GameObject.Transform.Bounds.X,
                        (int)this.GameObject.Transform.Bounds.Y,
                        (int)this.GameObject.Transform.Bounds.Width,
                        (int)this.GameObject.Transform.Bounds.Height),
                    null,
                    Color.White);
            }
        }
    }
}
