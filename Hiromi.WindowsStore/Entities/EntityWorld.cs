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
    public class EntityWorld
    {
        /// <summary>
        /// The time (in seconds) that have elapsed since the last frame
        /// </summary>
        public double DeltaTime { get; set; }

        private EntityManager _entityManager;

        private List<Entity> _addedEntities;
        private List<Entity> _changedEntities;
        private List<Entity> _deletedEntities;
        private List<Entity> _enabledEntities;
        private List<Entity> _disabledEntities;

        private Dictionary<Type, EntitySystem> _systemsLookup;
        private List<EntitySystem> _systemsList;

        public EntityWorld()
        {
            _systemsLookup = new Dictionary<Type, EntitySystem>();
            _systemsList = new List<EntitySystem>();

            _addedEntities = new List<Entity>();
            _changedEntities = new List<Entity>();
            _deletedEntities = new List<Entity>();
            _enabledEntities = new List<Entity>();
            _disabledEntities = new List<Entity>();

            _entityManager = new EntityManager();
            _entityManager.World = this;
        }

        public void Initialize()
        {
            foreach (var entitySystem in _systemsList)
            {
                entitySystem.Initialize();
            }
        }

        public EntityManager GetEntityManager()
        {
            return _entityManager;
        }

        public void AddEntity(Entity e)
        {
            _addedEntities.Add(e);
        }

        public void ChangedEntity(Entity e)
        {
            _changedEntities.Add(e);
        }

        public void DeleteEntity(Entity e)
        {
            if (!_deletedEntities.Contains(e))
            {
                _deletedEntities.Add(e);
            }
        }

        public void EnableEntity(Entity e)
        {
            _enabledEntities.Add(e);
        }

        public void DisableEntity(Entity e)
        {
            _disabledEntities.Add(e);
        }

        public Entity CreateEntity()
        {
            return _entityManager.CreateEntityInstance();
        }

        public Entity GetEntity(int entityId)
        {
            return _entityManager.GetEntity(entityId);
        }

        public IEnumerable<EntitySystem> GetSystems()
        {
            return _systemsList;
        }

        public TSystem SetSystem<TSystem>(TSystem entitySystem, bool passive = false) where TSystem : EntitySystem
        {
            entitySystem.World = this;
            entitySystem.IsPassive = passive;

            _systemsLookup.Add(typeof(TSystem), entitySystem);
            _systemsList.Add(entitySystem);

            return entitySystem;
        }

        public void DeleteSystem(EntitySystem entitySystem)
        {
            if (_systemsLookup.ContainsKey(entitySystem.GetType()))
            {
                _systemsLookup.Remove(entitySystem.GetType());
            }

            _systemsList.Remove(entitySystem);
        }

        public TSystem GetSystem<TSystem>() where TSystem : EntitySystem
        {
            return (TSystem)_systemsLookup[typeof(TSystem)];
        }

        public void Update(double elapsedTimeInSeconds)
        {
            this.DeltaTime = elapsedTimeInSeconds;

            Check(_addedEntities, (observer, entity) =>
            {
                observer.EntityAdded(entity);
            });

            Check(_changedEntities, (observer, entity) =>
            {
                observer.EntityChanged(entity);
            });

            Check(_disabledEntities, (observer, entity) =>
            {
                observer.EntityDisabled(entity);
            });

            Check(_enabledEntities, (observer, entity) =>
            {
                observer.EntityEnabled(entity);
            });

            Check(_deletedEntities, (observer, entity) =>
            {
                observer.EntityDeleted(entity);
            });

            foreach (var entitySystem in _systemsList)
            {
                if (!entitySystem.IsPassive)
                {
                    entitySystem.Update();
                }
            }
        }

        public void Draw(double elapsedTimeInSeconds)
        {
            this.DeltaTime = elapsedTimeInSeconds;

            foreach (var entitySystem in _systemsList)
            {
                if (!entitySystem.IsPassive)
                {
                    entitySystem.Draw();
                }
            }
        }

        private void Check(List<Entity> entities, Action<IEntityObserver, Entity> perform)
        {

            foreach (var entity in entities)
            {
                foreach (var entitySystem in _systemsList)
                {
                    perform(entitySystem, entity);
                }
            }
            entities.Clear();
        }
    }

}
