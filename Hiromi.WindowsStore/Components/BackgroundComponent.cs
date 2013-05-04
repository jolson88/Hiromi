using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hiromi;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Hiromi.Components
{
    public class BackgroundComponent: GameObjectComponent, IRenderAwareComponent
    {
        public int GameObjectId { get { return this.GameObject.Id; } }
        public RenderPass RenderPass { get { return RenderPass.BackgroundPass; } }
        public TransformationComponent Transform { get { return this.GameObject.Transform; } }
        
        public Texture2D Texture { get; set; }

        public BackgroundComponent(Texture2D texture)
        {
            this.Texture = texture;
        }

        public void Draw(GameTime gameTime, SpriteBatch batch)
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
