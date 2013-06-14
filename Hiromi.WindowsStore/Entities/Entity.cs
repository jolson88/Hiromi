/* 
//------------------------------------------------
//
// This is a port of the awesome Entity System framework Artemis by Arni Arent and Tiago Costa - http://gamadu.com/artemis 
//
//------------------------------------------------
Copyright 2011 GAMADU.COM. All rights reserved.

Redistribution and use in source and binary forms, with or without modification, are
permitted provided that the following conditions are met:

   1. Redistributions of source code must retain the above copyright notice, this list of
      conditions and the following disclaimer.

   2. Redistributions in binary form must reproduce the above copyright notice, this list
      of conditions and the following disclaimer in the documentation and/or other materials
      provided with the distribution.

THIS SOFTWARE IS PROVIDED BY GAMADU.COM ``AS IS'' AND ANY EXPRESS OR IMPLIED
WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL GAMADU.COM OR
CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR
CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON
ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF
ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

The views and conclusions contained in the software and documentation are those of the
authors and should not be interpreted as representing official policies, either expressed
or implied, of GAMADU.COM. 
*/

using System;
using System.Collections.Generic;
using System.Linq;

namespace Hiromi.Entities
{
    public class Entity
    {
        private EntityManager _entityManager;
        private Dictionary<Type, IComponent> _components;

        /// <summary>
        /// The internal id for this entity within the framework. No other entity
        /// will have the same ID, but ID's are however reused so another entity may
        /// acquire this ID if the previous entity was deleted.
        /// </summary>
        public int EntityId { get; private set; }

        public EntityWorld World { get; private set; }


        internal Entity(EntityWorld world, int id)
        {

            this.World = world;
            this.EntityId = id;

            _entityManager = world.GetEntityManager();
            _components = new Dictionary<Type, IComponent>();

            Reset();
        }

        /// <summary>
        /// Make entity ready for re-use.
        /// </summary>
        protected void Reset()
        {
            _components.Clear();
        }

        /// <summary>
        /// Add a component to this entity.
        /// </summary>
        /// <typeparam name="TComponent">The type of component</typeparam>
        /// <param name="component">The component to add</param>
        /// <returns>The entity (used for method chaining)</returns>
        public Entity AddComponent<TComponent>(TComponent component) where TComponent : IComponent
        {
            if (_components.ContainsKey(typeof(TComponent)))
            {
                throw new ArgumentException("Entity already contains an instance of " + component.GetType().ToString());
            }

            _components.Add(typeof(TComponent), component);
            return this;
        }

        /// <summary>
        /// Removes the component from this entity.
        /// </summary>
        /// <typeparam name="TComponent">The type of component to remove</typeparam>
        /// <param name="component">The component to remove</param>
        /// <returns>The entity (used for method chaining)</returns>
        public Entity RemoveComponent<TComponent>(TComponent component) where TComponent : IComponent
        {
            if (_components.ContainsKey(typeof(TComponent)))
            {
                _components.Remove(typeof(TComponent));
            }

            return this;
        }

        /// <summary>
        /// Checks if the entity has been added to the world and has not been deleted from it.
        /// If the entity has been disabled this will still return true.
        /// </summary>
        /// <returns>True if active, false if not</returns>
        public bool IsActive()
        {
            return _entityManager.IsActive(this.EntityId);
        }

        /// <summary>
        /// Will check if the entity is enabled in the world.
        /// By default all entities that are added to world are enabled,
        /// this will only return false if an entity has been explicitly disabled.
        /// </summary>
        /// <returns>True if enabled, false if not</returns>
        public bool IsEnabled()
        {
            return _entityManager.IsEnabled(this.EntityId);
        }

        /// <summary>
        /// Retrieve a specific component from the entity.
        /// </summary>
        /// <typeparam name="TComponent">The type of component to retrieve</typeparam>
        /// <returns>The component instance</returns>
        public TComponent GetComponent<TComponent>() where TComponent : IComponent
        {
            if (!_components.ContainsKey(typeof(TComponent)))
            {
                throw new ArgumentException("Entity doesn't contain " + typeof(TComponent).ToString());
            }

            return (TComponent)_components[typeof(TComponent)];
        }

        /// <summary>
        /// Check if the entity has the specified type of component.
        /// </summary>
        /// <typeparam name="TComponent">The type of component to check</typeparam>
        /// <returns>True if entity contains component</returns>
        public bool HasComponent<TComponent>() where TComponent : IComponent
        {
            return _components.ContainsKey(typeof(TComponent));
        }

        /// <summary>
        /// Returns all components this entity has.
        /// </summary>
        /// <returns>The components</returns>
        public IEnumerable<IComponent> GetAllComponents()
        {
            return _components.Values;
        }

        /// <summary>
        /// Refresh all changes to components for this entity. After adding or
        /// removing components, you must call this method. It will update all
        /// relevant systems. It is typical to call this after adding components to a
        /// newly created entity.
        /// </summary>
        public void AddToWorld()
        {
            World.AddEntity(this);
        }

        /// <summary>
        /// This entity has changed, a component added or deleted.
        /// </summary>
        public void ChangedInWorld()
        {
            World.ChangedEntity(this);
        }

        /// <summary>
        /// Delete this entity from the world.
        /// </summary>
        public void DeleteFromWorld()
        {
            World.DeleteEntity(this);
        }

        /// <summary>
        /// (Re)enable the entity in the world, after it having being disabled.
        /// Won't do anything unless it was already disabled.
        /// </summary>
        public void Enable()
        {
            World.EnableEntity(this);
        }

        /// <summary>
        /// Disable the entity from being processed. Won't delete it, it will
        /// continue to exist but won't get processed.
        /// </summary>
        public void Disable()
        {
            World.DisableEntity(this);
        }


        public override String ToString()
        {
            return "Entity[" + EntityId + "]";
        }
    }
}
