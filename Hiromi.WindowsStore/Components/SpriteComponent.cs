using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Hiromi.Components
{
    public class SpriteComponent : GameObjectComponent
    {
        public bool IsVisible { get; set; }
        public Texture2D Texture { get; set; }

        public SpriteComponent(Texture2D texture)
        {
            this.Texture = texture;
            this.IsVisible = true;
        }

        public override void Draw(GameTime gameTime)
        {
            var posComponent = this.GameObject.GetComponent<PositionComponent>();
            if (this.IsVisible)
            {
                // We use Bounds instead of Position as Bounds takes the achor point into account
                GraphicsService.Instance.SpriteBatch.Draw(this.Texture,
                    new Vector2(posComponent.Bounds.X * GraphicsService.Instance.GraphicsDevice.Viewport.Width,
                        posComponent.Bounds.Y * GraphicsService.Instance.GraphicsDevice.Viewport.Height),
                    Color.White);
            }
        }
    }
}
