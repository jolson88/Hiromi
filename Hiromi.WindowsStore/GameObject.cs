using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Hiromi.Processing;

namespace Hiromi
{
    public class GameObject
    {
        public string Tag { get; set; }
        public int Id { get; set; }

        private Dictionary<Type, IComponent> _components;

        public GameObject()
        {
            _components = new Dictionary<Type, IComponent>();
            this.Tag = string.Empty;
        }

        public void AddComponent(IComponent component)
        {
            _components.Add(component.GetType(), component);
        }

        public T GetComponent<T>() where T : IComponent
        {
            return (T)_components[typeof(T)];
        }

        public bool HasComponent<T>() where T : IComponent
        {
            return _components.Keys.Contains(typeof(T));
        }
    }
}
