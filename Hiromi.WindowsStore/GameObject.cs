using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Hiromi
{
    public class GameObject
    {
        public static int InvalidId = -1;

        public MessageManager MessageManager { get; set; }
        public ProcessManager ProcessManager { get; set; }
        public string Tag { get; set; }
        public int Id { get; set; }
        public int Depth { get; set; }

        private Dictionary<Type, GameObjectComponent> _components;
        private bool _isLoaded = false;

        public GameObject() : this(string.Empty) { }
        public GameObject(int depth) : this(string.Empty) { this.Depth = depth; }
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
            foreach (var component in _components.Values)
            {
                component.Update(gameTime);
            }
        }

        public void AddComponent(GameObjectComponent component)
        {
            component.GameObject = this;
            _components.Add(component.GetType(), component);

            if (_isLoaded)
            {
                // We are already loaded to load this component
                component.Loaded();
            }
        }

        public T GetComponent<T>() where T : GameObjectComponent
        {
            return (T)_components[typeof(T)];
        }

        public bool HasComponent<T>() where T : GameObjectComponent
        {
            return _components.Keys.Contains(typeof(T));
        }

        public void RemoveComponent<T>() where T : GameObjectComponent
        {
            _components.Remove(typeof(T));
        }
    }
}
