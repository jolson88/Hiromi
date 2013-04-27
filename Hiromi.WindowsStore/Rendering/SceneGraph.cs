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
        private MessageManager _messageManager;
        private ISceneNode _rootNode;
        private Dictionary<int, ISceneNode> _gameObjectLookup;

        public SceneGraph(MessageManager messageManager)
        {
            _messageManager = messageManager;
            _rootNode = new RootNode();
            _gameObjectLookup = new Dictionary<int, ISceneNode>();

            _messageManager.AddListener<GameObjectMovedMessage>(msg => OnGameObjectMoved((GameObjectMovedMessage)msg));
            _messageManager.AddListener<GameObjectRemovedMessage>(msg => OnGameObjectRemoved((GameObjectRemovedMessage)msg));
            _messageManager.AddListener<NewRenderingComponentMessage>(msg => OnNewRenderingComponent((NewRenderingComponentMessage)msg));
        }

        public void AddChild(ISceneNode child)
        {
            _rootNode.AddChild(child);
            _gameObjectLookup.Add(child.GameObjectId, child);
        }

        public void RemoveChild(int gameObjectId)
        {
            _rootNode.RemoveChild(gameObjectId);
        }

        public void Update(GameTime gameTime)
        {
            _rootNode.Update(gameTime);
        }

        public void Draw(GameTime gameTime)
        {
            _rootNode.Draw(gameTime);
        }

        private void OnGameObjectMoved(GameObjectMovedMessage msg)
        {
            var node = _gameObjectLookup[msg.GameObject.Id];
            node.PositionComponent = msg.GameObject.GetComponent<PositionComponent>();
        }

        private void OnGameObjectRemoved(GameObjectRemovedMessage msg)
        {
            RemoveChild(msg.GameObjectId);
        }

        private void OnNewRenderingComponent(NewRenderingComponentMessage msg)
        {
            var node = msg.RenderingComponent.GetSceneNode();
            node.Initialize(_messageManager);
            AddChild(node);
        }
    }
}
