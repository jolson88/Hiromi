using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hiromi.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Hiromi.Rendering
{
    public interface ISceneNode
    {
        bool IsVisible { get; set; }
        int GameObjectId { get; set; }
        RenderPass RenderPass { get; set; }
        TransformationComponent TransformationComponent { get; set; }

        void Initialize(MessageManager messageManager);
        void Draw(GameTime gameTime, SceneGraph scene);
        bool Pick(Vector2 pointerLocation, ref int? gameObjectId);
    }
}
