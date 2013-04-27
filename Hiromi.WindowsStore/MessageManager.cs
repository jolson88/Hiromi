﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Context;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Hiromi
{
    public class MessageManager
    {
        private Dictionary<Type, List<object>> _messageListeners;
        private Dictionary<Type, Action<Message>> _messageSenders;
        private List<Queue<Message>> _messageQueues;
        private int _currentMessageQueue;

        public MessageManager()
        {
            _messageListeners = new Dictionary<Type, List<object>>();
            _messageSenders = new Dictionary<Type, Action<Message>>();
            _messageQueues = new List<Queue<Message>>() { new Queue<Message>(), new Queue<Message>() };

            var genAct = typeof(Action<>).MakeGenericType(typeof(int));
                
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

        // TODO: Change listeners to object (with casting) so parameter can be Action<T>
        public void AddListener<T>(Action<T> listener) where T : Message
        {
            if (!_messageListeners.Keys.Contains(typeof(T)))
            {
                _messageListeners[typeof(T)] = new List<object>();
                _messageSenders[typeof(T)] = CreateMessageSender<T>();
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
                _messageSenders[msg.GetType()](msg);
            }
        }

        private Action<Message> CreateMessageSender<T>() where T : Message
        {
            return new Action<Message>(param => {
                foreach (var listener in _messageListeners[typeof(T)])
                {
                    ((Action<T>)listener)((T)param);
                }
            });
        }
    }
}
