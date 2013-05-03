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
        public bool IsVisible { get; set; }
        public int GameObjectId { get; set; }
        public RenderPass RenderPass { get; set; }
        public TransformationComponent PositionComponent { get; set; }

        protected MessageManager _messageManager;

        private Dictionary<RenderPass, List<ISceneNode>> _childrenByPass;

        public RootNode()
        {
            _childrenByPass = new Dictionary<RenderPass, List<ISceneNode>>();
            foreach (var val in Enum.GetValues(typeof(RenderPass)))
            {
                _childrenByPass.Add((RenderPass)val, new List<ISceneNode>());
            }
        }

        public void Initialize(MessageManager messageManager)
        {
            _messageManager = messageManager;
        }

        public void AddNode(ISceneNode node)
        {
            _childrenByPass[node.RenderPass].Add(node);
        }

        public void RemoveNode(ISceneNode node)
        {
            foreach (var val in Enum.GetValues(typeof(RenderPass)))
            {
                _childrenByPass[(RenderPass)val].Remove(node);
            }
        }

        public void Update(GameTime gameTime)
        {
            // Update the scene in render pass order
            for (int i = 0; i <= (int)RenderPass.LassPass; i++)
            {
                foreach (var node in _childrenByPass[(RenderPass)i])
                {
                    node.Update(gameTime);
                }
            }
        }

        public void Draw(GameTime gameTime, SceneGraph scene)
        {
            // Draw the scene in render pass order
            for (int i = 0; i <= (int)RenderPass.LassPass; i++)
            {
                foreach (var node in _childrenByPass[(RenderPass)i])
                {
                    node.Draw(gameTime, scene);
                }
            }
        }

        public bool Pick(Vector2 pointerLocation, ref int? gameObjectId)
        {
            // Reverse drawing order to find top-most game object picked
            for (int i = (int)RenderPass.LassPass; i >= (int)RenderPass.GameObjectPass; i--)
            {
                foreach (var node in _childrenByPass[(RenderPass)i])
                {
                    if (node.Pick(pointerLocation, ref gameObjectId))
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
