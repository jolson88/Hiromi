using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Hiromi.Messaging
{
    public class MessageManager
    {
        private Dictionary<Type, List<Action<Message>>> _messageListeners;
        private List<Queue<Message>> _messageQueues;
        private int _currentMessageQueue;

        public MessageManager()
        {
            _messageListeners = new Dictionary<Type, List<Action<Message>>>();
            _messageQueues = new List<Queue<Message>>() { new Queue<Message>(), new Queue<Message>() };
        }

        public void Update(GameTime gameTime)
        {
            var processQueue = _currentMessageQueue;

            // Make sure any new messages coming in from processing these messages goes
            // to a different queue
            _currentMessageQueue = (_currentMessageQueue + 1) % _messageQueues.Count;

            while (_messageQueues[processQueue].Count > 0)
            {
                var msg = _messageQueues[processQueue].Dequeue();
                ProcessMessage(msg);
                if (msg.GetMessageVerbosity() == MessageVerbosity.Signal)
                {
                    System.Diagnostics.Debug.WriteLine(msg.ToString());
                }
            }
        }

        public void AddListener<T>(Action<Message> listener) where T : Message
        {
            if (!_messageListeners.Keys.Contains(typeof(T)))
            {
                _messageListeners[typeof(T)] = new List<Action<Message>>();
            }

            _messageListeners[typeof(T)].Add(listener);
        }

        public void QueueMessage(Message msg)
        {
            _messageQueues[_currentMessageQueue].Enqueue(msg);
        }

        public void TriggerMessage(Message msg)
        {
            // Immediately process
            ProcessMessage(msg);
        }

        private void ProcessMessage(Message msg)
        {
            if (_messageListeners.Keys.Contains(msg.GetType()))
            {
                foreach (var listener in _messageListeners[msg.GetType()])
                {
                    listener(msg);
                }
            }
        }
    }
}
