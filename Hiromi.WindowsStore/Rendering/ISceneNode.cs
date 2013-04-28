using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hiromi.Components;
using Microsoft.Xna.Framework;

namespace Hiromi.Rendering
{
    public interface ISceneNode
    {
        ISceneNode Parent { get; set; }
        bool IsVisible { get; set; }
        int GameObjectId { get; set; }
        RenderPass RenderPass { get; set; }
        PositionComponent PositionComponent { get; set; }

        void Initialize(MessageManager messageManager);
        void AddChild(ISceneNode child);
        void RemoveChild(int gameObjectId);
        void Update(GameTime gameTime);
        void Draw(GameTime gameTime, SceneGraph scene);
        bool Pick(Vector2 pointerLocation, ref int? gameObjectId);
    }
}
