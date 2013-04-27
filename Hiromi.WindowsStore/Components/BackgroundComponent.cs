using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Hiromi.Components
{
    public class BackgroundComponent : GameObjectComponent
    {
        public Texture2D Texture { get; set; }

        public BackgroundComponent(Texture2D texture)
        {
            this.Texture = texture;
        }

        public override void Draw(GameTime gameTime)
        {
            // TODO: Move into BackgroundNode (new SceneNode)
            GraphicsService.Instance.SpriteBatch.Draw(this.Texture,
                new Rectangle(0, 0, GraphicsService.Instance.GraphicsDevice.Viewport.Width, GraphicsService.Instance.GraphicsDevice.Viewport.Height),
                Color.White);
        }
    }
}
