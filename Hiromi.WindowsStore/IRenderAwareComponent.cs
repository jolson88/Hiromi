using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Hiromi.Components;

namespace Hiromi
{
    public interface IRenderAwareComponent
    {
        int GameObjectId { get; }
        RenderPass RenderPass { get; }
        TransformationComponent Transform { get; }
        void Draw(GameTime gameTime, SpriteBatch batch);
    }
}
