using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Hiromi.Messaging;

namespace Hiromi
{
    public class GameObjectManager
    {
        private MessageManager _messageManager;
        private int _nextObjectId = 0;
        private List<GameObject> _objects;

        public GameObjectManager(MessageManager messageManager)
        {
            _messageManager = messageManager;
            _objects = new List<GameObject>();
        }

        public void AddGameObject(GameObject gameObject)
        {
            gameObject.Id = _nextObjectId;
            _nextObjectId++;
            _objects.Add(gameObject);

            _messageManager.QueueMessage(new GameObjectLoadedMessage(gameObject));
        }

        public List<GameObject> GetAllGameObjects()
        {
            return _objects;
        }

        public List<GameObject> GetAllGameObjectsWithTag(string tag)
        {
            return _objects.FindAll(obj => obj.Tag.ToUpper().Equals(tag.ToUpper()));
        }
    }
}
