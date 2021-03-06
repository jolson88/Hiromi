﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        }

        public void Update(GameTime gameTime)
        {
            var processQueue = _currentMessageQueue;

            // Make sure any new messages received from processing existing messages goes
            // to a different queue. If we don't do this, listeners could keep on firing messages
            // and we would get into an endless loop of messages to listen to. All new messages
            // will be processed on the next graphics frame.
            _currentMessageQueue = (_currentMessageQueue + 1) % _messageQueues.Count;

            while (_messageQueues[processQueue].Count > 0)
            {
                var msg = _messageQueues[processQueue].Dequeue();
                ProcessMessage(msg);
                if (msg.GetMessageVerbosity() == MessageVerbosity.Signal)
                {
                    System.Diagnostics.Debug.WriteLine(string.Format("[{0}] {1}", gameTime.TotalGameTime.TotalSeconds, msg.ToString()));
                }
            }
        }

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

        /// <summary>
        /// Creates a generic message sender (of type Message) that fires all
        /// listeners in a strongly-typed fashion.
        /// </summary>
        /// <typeparam name="T">The specific type of message being sent</typeparam>
        /// <returns>An Action that will receive a generic Message and fire each registered listener by casting to the known type of Message.</returns>
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
