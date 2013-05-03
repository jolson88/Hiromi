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
        private RootNode _rootNode;
        private Dictionary<int, ISceneNode> _gameObjectLookup;
        private Camera _camera;

        public SceneGraph(MessageManager messageManager)
        {
            this.SpriteBatch = new SpriteBatch(GraphicsService.Instance.GraphicsDevice);
            this.NonTransformedSpriteBatch = new SpriteBatch(GraphicsService.Instance.GraphicsDevice);

            _messageManager = messageManager;
            _rootNode = new RootNode();
            _gameObjectLookup = new Dictionary<int, ISceneNode>();

            _messageManager.AddListener<GameObjectLoadedMessage>(OnGameObjectLoaded);
            _messageManager.AddListener<GameObjectMovedMessage>(OnGameObjectMoved);
            _messageManager.AddListener<GameObjectRemovedMessage>(OnGameObjectRemoved);
            _messageManager.AddListener<NewRenderingComponentMessage>(OnNewRenderingComponent);

            _camera = new Camera(_messageManager);
        }

        public void AddNode(ISceneNode node)
        {
            _rootNode.AddNode(node);
            _gameObjectLookup.Add(node.GameObjectId, node);
        }

        public void RemoveNode(ISceneNode node)
        {
            _rootNode.RemoveNode(node);
            _gameObjectLookup.Remove(node.GameObjectId);
        }

        public void Update(GameTime gameTime)
        {
            _rootNode.Update(gameTime);
        }

        public void Draw(GameTime gameTime)
        {
            // When we transform via camera, we need to flip rasterizer (counter clockwise) since we are flipping Y component to Y+ up (instead of down)
            this.SpriteBatch.Begin(SpriteSortMode.BackToFront, null, null, null, RasterizerState.CullCounterClockwise, null, _camera.TransformationMatrix);
            this.NonTransformedSpriteBatch.Begin();

            _rootNode.Draw(gameTime, this);

            this.NonTransformedSpriteBatch.End();
            this.SpriteBatch.End();
        }

        public bool Pick(Vector2 pointerLocation, ref int? gameObjectId)
        {
            // Account for camera transformation;
            var transformedPointer = Vector2.Transform(pointerLocation, Matrix.Invert(_camera.TransformationMatrix));
            return _rootNode.Pick(transformedPointer, ref gameObjectId);
        }

        private void OnGameObjectLoaded(GameObjectLoadedMessage msg)
        {
            var cameraAware = msg.GameObject.GetComponentWithInterface<ICameraAware>();
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
                var node = _gameObjectLookup[msg.GameObject.Id];
                node.TransformationComponent = msg.GameObject.GetComponent<TransformationComponent>();
            }
        }

        private void OnGameObjectRemoved(GameObjectRemovedMessage msg)
        {
            RemoveNode(_gameObjectLookup[msg.GameObjectId]);
        }

        private void OnNewRenderingComponent(NewRenderingComponentMessage msg)
        {
            var node = msg.RenderingComponent.GetSceneNode();
            node.Initialize(_messageManager);
            AddNode(node);
        }
    }
}
