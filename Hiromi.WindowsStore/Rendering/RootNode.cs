using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hiromi.Components;
using Microsoft.Xna.Framework;

namespace Hiromi.Rendering
{
    public class RootNode
    {
        public ISceneNode Parent { get; set; }
        public bool IsVisible { get; set; }
        public int GameObjectId { get; set; }
        public RenderPass RenderPass { get; set; }
        public TransformationComponent PositionComponent { get; set; }

        protected MessageManager _messageManager;

        private Dictionary<RenderPass, ISceneNode> _childrenByPass;

        public RootNode()
        {
            _childrenByPass = new Dictionary<RenderPass, ISceneNode>();
            foreach (var val in Enum.GetValues(typeof(RenderPass)))
            {
                _childrenByPass.Add((RenderPass)val, new SceneNode(GameObject.InvalidId, null, (RenderPass)val));
            }
        }

        public void Initialize(MessageManager messageManager)
        {
            _messageManager = messageManager;
        }

        public void AddChild(ISceneNode child)
        {
            _childrenByPass[child.RenderPass].AddChild(child);
        }

        public void RemoveChild(int gameObjectId)
        {
            foreach (var val in Enum.GetValues(typeof(RenderPass)))
            {
                _childrenByPass[(RenderPass)val].RemoveChild(gameObjectId);
            }
        }

        public void Update(GameTime gameTime)
        {
            foreach (var val in Enum.GetValues(typeof(RenderPass)))
            {
                _childrenByPass[(RenderPass)val].Update(gameTime);
            }
        }

        public void Draw(GameTime gameTime, SceneGraph scene)
        {
            // Draw the scene in render pass order
            for (int i = 0; i <= (int)RenderPass.LassPass; i++)
            {
                _childrenByPass[(RenderPass)i].Draw(gameTime, scene);
            }
        }

        public bool Pick(Vector2 pointerLocation, ref int? gameObjectId)
        {
            // Reverse drawing order to find top-most game object picked
            for (int i = (int)RenderPass.LassPass; i >= (int)RenderPass.GameObjectPass; i--)
            {
                if (_childrenByPass[(RenderPass)i].Pick(pointerLocation, ref gameObjectId))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
