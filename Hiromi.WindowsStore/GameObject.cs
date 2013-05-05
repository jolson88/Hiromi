using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Hiromi.Components;

namespace Hiromi
{
    public class GameObject
    {
        public MessageManager MessageManager { get; set; }
        public ProcessManager ProcessManager { get; set; }
        public string Tag { get; set; }
        public int Id { get; set; }

        public TransformationComponent Transform { get; set; }

        private Dictionary<Type, GameObjectComponent> _components;
        private bool _isLoaded = false;

        public GameObject() : this(string.Empty) { }
        public GameObject(string tag)
        {
            _components = new Dictionary<Type, GameObjectComponent>();
            this.Tag = tag;
        }

        public void Loaded()
        {
            foreach (var component in _components.Values)
            {
                component.Loaded();
            }
            _isLoaded = true;
        }

        public void Update(GameTime gameTime)
        {
            // .ToList() so collection can be modified in updates (like components removing themselves)
            foreach (var component in _components.Values.ToList())
            {
                component.Update(gameTime);
            }
        }

        public void AddComponent<T>(T component) where T : GameObjectComponent
        {
            component.GameObject = this;
            if (this.GetComponent<T>() != null)
            {
                this.RemoveComponent<T>();
            }

            _components.Add(component.GetType(), component);
            if (component.GetType() == typeof(TransformationComponent))
            {
                this.Transform = component as TransformationComponent;
            }

            if (_isLoaded)
            {
                // We are already loaded to load this component
                component.Loaded();
            }
        }

        public T GetComponent<T>() where T : GameObjectComponent
        {
            if (_components.ContainsKey(typeof(T)))
            {
                return (T)_components[typeof(T)];
            }
            else
            {
                return null;
            }
        }

        public T GetComponentWithAwareness<T>() where T : class
        {
            foreach (var key in _components.Keys)
            {
                if (_components[key] is T)
                {
                    return (_components[key] as T);
                }
            }

            return null;
        }

        public bool HasComponent<T>() where T : GameObjectComponent
        {
            return _components.Keys.Contains(typeof(T));
        }

        public void RemoveComponent<T>() where T : GameObjectComponent
        {
            if (HasComponent<T>())
            {
                var component = GetComponent<T>();
                _components.Remove(typeof(T));
                component.Remove();
            }
        }
    }
}
