using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Hiromi
{
    public class GameObjectManager
    {
        private ProcessManager _processManager;
        private MessageManager _messageManager;
        private int _nextObjectId = 0;
        private List<GameObject> _objects;

        public GameObjectManager(ProcessManager processManager, MessageManager messageManager)
        {
            _processManager = processManager;
            _messageManager = messageManager;
            _objects = new List<GameObject>();

            _messageManager.AddListener<AddGameObjectRequestMessage>(OnNewGameObject);
        }

        public void AddGameObject(GameObject gameObject)
        {
            gameObject.ProcessManager = _processManager;
            gameObject.MessageManager = _messageManager;

            gameObject.Id = _nextObjectId;
            _nextObjectId++;
            _objects.Add(gameObject);
            gameObject.Loaded();
            _messageManager.TriggerMessage(new GameObjectLoadedMessage(gameObject));
        }

        public void RemoveGameObject(GameObject obj)
        {
            _objects.Remove(obj);
            _messageManager.TriggerMessage(new GameObjectRemovedMessage(obj.Id));
        }

        public List<GameObject> GetAllGameObjects()
        {
            return _objects;
        }

        public List<GameObject> GetAllGameObjectsWithTag(string tag)
        {
            return _objects.FindAll(obj => obj.Tag.ToUpper().Equals(tag.ToUpper()));
        }

        private void OnNewGameObject(AddGameObjectRequestMessage msg)
        {
            AddGameObject(msg.GameObject);
        }
    }
}
