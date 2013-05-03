using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Hiromi.Components;

namespace Hiromi.Rendering
{
    public class SceneGraph
    {
        public SpriteBatch SpriteBatch { get; set; }
        public SpriteBatch NonTransformedSpriteBatch { get; set; }

        private MessageManager _messageManager;
        private Dictionary<int, List<ISceneNode>> _gameObjectLookup;
        private Camera _camera;
        private Dictionary<RenderPass, List<ISceneNode>> _nodes;

        public SceneGraph(MessageManager messageManager)
        {
            this.SpriteBatch = new SpriteBatch(GraphicsService.Instance.GraphicsDevice);
            this.NonTransformedSpriteBatch = new SpriteBatch(GraphicsService.Instance.GraphicsDevice);

            _messageManager = messageManager;
            _gameObjectLookup = new Dictionary<int, List<ISceneNode>>();
            
            _nodes = new Dictionary<RenderPass, List<ISceneNode>>();
            foreach (var val in Enum.GetValues(typeof(RenderPass)))
            {
                _nodes.Add((RenderPass)val, new List<ISceneNode>());
            }

            _messageManager.AddListener<GameObjectLoadedMessage>(OnGameObjectLoaded);
            _messageManager.AddListener<GameObjectMovedMessage>(OnGameObjectMoved);
            _messageManager.AddListener<GameObjectRemovedMessage>(OnGameObjectRemoved);
            _messageManager.AddListener<NewRenderingComponentMessage>(OnNewRenderingComponent);

            _camera = new Camera(_messageManager);
        }

        public void AddNode(ISceneNode node)
        {
            _nodes[node.RenderPass].Add(node);
            
            if (!_gameObjectLookup.Keys.Contains(node.GameObjectId))
            {
                _gameObjectLookup.Add(node.GameObjectId, new List<ISceneNode>());
            }
            _gameObjectLookup[node.GameObjectId].Add(node);
        }

        public void RemoveNode(ISceneNode node)
        {
            System.Diagnostics.Debug.Assert(_gameObjectLookup.Keys.Contains(node.GameObjectId));

            foreach (var val in Enum.GetValues(typeof(RenderPass)))
            {
                _nodes[(RenderPass)val].Remove(node);
            }
            _gameObjectLookup[node.GameObjectId].Remove(node);
        }

        public void Draw(GameTime gameTime)
        {
            // When we transform via camera, we need to flip rasterizer (counter clockwise) since we are flipping Y component to Y+ up (instead of down)
            this.SpriteBatch.Begin(SpriteSortMode.BackToFront, null, null, null, RasterizerState.CullCounterClockwise, null, _camera.TransformationMatrix);
            this.NonTransformedSpriteBatch.Begin();

            // Draw the scene in render pass order
            for (int i = 0; i <= (int)RenderPass.LassPass; i++)
            {
                foreach (var node in _nodes[(RenderPass)i])
                {
                    node.Draw(gameTime, this);
                }
            }

            this.NonTransformedSpriteBatch.End();
            this.SpriteBatch.End();
        }

        public bool Pick(Vector2 pointerLocation, ref int? gameObjectId)
        {
            // Account for camera transformation;
            var transformedPointer = Vector2.Transform(pointerLocation, Matrix.Invert(_camera.TransformationMatrix));

            // Reverse drawing order to find top-most game object picked
            for (int i = (int)RenderPass.LassPass; i >= (int)RenderPass.GameObjectPass; i--)
            {
                foreach (var node in _nodes[(RenderPass)i])
                {
                    if (node.Pick(transformedPointer, ref gameObjectId))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private void OnGameObjectLoaded(GameObjectLoadedMessage msg)
        {
            var cameraAware = msg.GameObject.GetComponentWithAwareness<ICameraAware>();
            if (cameraAware != null)
            {
                cameraAware.ActiveCamera = _camera;
            }
        }

        private void OnGameObjectMoved(GameObjectMovedMessage msg)
        {
            // We don't care about objects that aren't rendered
            if (_gameObjectLookup.ContainsKey(msg.GameObject.Id))
            {
                foreach (var node in _gameObjectLookup[msg.GameObject.Id])
                {
                    node.TransformationComponent = msg.GameObject.GetComponent<TransformationComponent>();
                }
            }
        }

        private void OnGameObjectRemoved(GameObjectRemovedMessage msg)
        {
            if (_gameObjectLookup.ContainsKey(msg.GameObjectId))
            {
                // .ToList() to bring in local copy as we will be removing from the list we are iterating over
                foreach (var node in _gameObjectLookup[msg.GameObjectId].ToList())
                {
                    RemoveNode(node);
                }
            }
        }

        private void OnNewRenderingComponent(NewRenderingComponentMessage msg)
        {
            var node = msg.RenderingComponent.GetSceneNode();
            node.Initialize(_messageManager);
            AddNode(node);
        }
    }
}
