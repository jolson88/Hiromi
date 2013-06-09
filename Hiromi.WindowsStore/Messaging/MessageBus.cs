using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Hiromi.Messaging
{
    public class MessageBus
    {
        private Dictionary<Type, List<ListenerLookup>> _messageListeners;
        private List<Queue<object>> _messageQueues;
        private int _currentMessageQueue;

        public MessageBus()
        {
            _messageListeners = new Dictionary<Type, List<ListenerLookup>>();
            _messageQueues = new List<Queue<object>>() { new Queue<object>(), new Queue<object>() };
        }

        public void ProcessMessages()
        {
            var processQueue = _currentMessageQueue;

            // Make sure any new messages coming in from processing these messages goes
            // to a different queue
            _currentMessageQueue = (_currentMessageQueue + 1) % _messageQueues.Count;

            while (_messageQueues[processQueue].Count > 0)
            {
                var msg = _messageQueues[processQueue].Dequeue();
                ProcessMessage(msg);
            }
        }

        public void Register(object obj)
        {
            var typeInfo = obj.GetType().GetTypeInfo();
            var methodAndTypes = from method in typeInfo.DeclaredMethods
                                 where method.GetParameters().Count() == 1 && method.GetCustomAttribute<SubscribeAttribute>() != null
                                 select Tuple.Create(method, method.GetParameters().First().ParameterType);

            foreach (var methodAndType in methodAndTypes)
            {
                AddListener(obj, methodAndType.Item1, methodAndType.Item2);
            }
        }

        public void QueueMessage(object msg)
        {
            _messageQueues[_currentMessageQueue].Enqueue(msg);
        }

        public void TriggerMessage(object msg)
        {
            // Immediately process
            ProcessMessage(msg);
        }

        private void ProcessMessage(object msg)
        {
            if (_messageListeners.Keys.Contains(msg.GetType()))
            {
                foreach (var lookup in _messageListeners[msg.GetType()])
                {
                    lookup.Listener.Invoke(lookup.Instance, new object[] { msg });
                }
            }
        }

        private void AddListener(object instance, MethodInfo methodInfo, Type msgType)
        {
            if (!_messageListeners.Keys.Contains(msgType))
            {
                _messageListeners[msgType] = new List<ListenerLookup>();
            }

            _messageListeners[msgType].Add(new ListenerLookup(instance, methodInfo));
        }

        private class ListenerLookup
        {
            public object Instance { get; set; }
            public MethodInfo Listener { get; set; }

            public ListenerLookup(object instance, MethodInfo listener)
            {
                this.Instance = instance;
                this.Listener = listener;
            }
        }
    }
}
