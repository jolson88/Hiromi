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
        public string Tag { get; set; }
        public int Id { get; set; }

        private Dictionary<Type, GameObjectComponent> _components;

        public GameObject()
        {
            _components = new Dictionary<Type, GameObjectComponent>();
            this.Tag = string.Empty;
        }

        public void Loaded()
        {
            foreach (var component in _components.Values)
            {
                component.Loaded();
            }
        }

        public void AddComponent(GameObjectComponent component)
        {
            component.GameObject = this;
            _components.Add(component.GetType(), component);
        }

        public T GetComponent<T>() where T : GameObjectComponent
        {
            return (T)_components[typeof(T)];
        }

        public bool HasComponent<T>() where T : GameObjectComponent
        {
            return _components.Keys.Contains(typeof(T));
        }
    }
}
