using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Hiromi;
using Hiromi.Components;

namespace Hiromi.Rendering
{
    public class SceneNode : ISceneNode
    {
        public bool IsVisible { get; set; }
        public ISceneNode Parent { get; set; }
        public int GameObjectId { get; set; }
        public RenderPass RenderPass { get; set; }
        public PositionComponent PositionComponent { get; set; }

        protected MessageManager MessageManager { get; set; }

        private List<ISceneNode> _children;

        public SceneNode(int gameObjectId, PositionComponent positionComponent, RenderPass renderPass)
        {
            this.IsVisible = true;
            this.GameObjectId = gameObjectId;
            this.PositionComponent = positionComponent;
            this.RenderPass = renderPass;
            _children = new List<ISceneNode>();
        }

        public void Initialize(MessageManager messageManager)
        {
            this.MessageManager = messageManager;
            OnInitialize();
        }

        public void AddChild(ISceneNode child)
        {
            _children.Add(child);
            child.Parent = this;
        }

        public void RemoveChild(int actorId)
        {
            var nodesToRemove = new List<ISceneNode>();
            foreach (var node in _children)
            {
                if (node.GameObjectId == actorId)
                {
                    nodesToRemove.Add(node);
                }
            }
            _children.RemoveAll(node => nodesToRemove.Contains(node));
        }

        public void Update(GameTime gameTime)
        {
            OnUpdate(gameTime);
            foreach (var node in _children)
            {
                node.Update(gameTime);
            }
        }

        public void Draw(GameTime gameTime)
        {
            if (IsVisible)
            {
                OnDraw(gameTime);
                foreach (var node in _children)
                {
                    node.Draw(gameTime);
                }
            }
        }

        public bool Pick(Vector2 pointerLocation, ref int? gameObjectId)
        {
            if (PointerOverSceneNode(pointerLocation))
            {
                gameObjectId = this.GameObjectId;
                return true;
            }
            foreach (var child in _children)
            {
                if (child.Pick(pointerLocation, ref gameObjectId))
                {
                    return true;
                }
            }

            gameObjectId = null;
            return false;
        }

        private bool PointerOverSceneNode(Vector2 pointerLocation)
        {
            // Need to convert pixel coordinates from mouse into screen coordinates
            if (this.PositionComponent != null)
            {
                return this.PositionComponent.Bounds.Contains((float)pointerLocation.X / GraphicsService.Instance.GraphicsDevice.Viewport.Width,
                    (float)pointerLocation.Y / GraphicsService.Instance.GraphicsDevice.Viewport.Height);
            }
            return false;
        }

        protected virtual void OnInitialize() { }
        protected virtual void OnUpdate(GameTime gameTime) { }
        protected virtual void OnDraw(GameTime gameTime) { }
    }
}
