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
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Hiromi.Entities
{
    public class EntityManager : Manager
    {
        private Dictionary<int, Entity> _entities;
        private BitArray _disabled;

        private int _activeCount;
        private long _addedCount;
        private long _createdCount;
        private long _deletedCount;
        private IdProvider _idProvider;

        public EntityManager()
        {
            _entities = new Dictionary<int, Entity>();
            _disabled = new BitArray(int.MaxValue);
            _idProvider = new IdProvider();
        }

        public Entity CreateEntityInstance()
        {
            Entity entity = new Entity(this.World, _idProvider.CheckOut());

            _createdCount++;
            return entity;
        }


        public override void EntityAdded(Entity e)
        {
            _activeCount++;
            _addedCount++;

            _entities.Add(e.EntityId, e);
        }

        
        public override void EntityEnabled(Entity e)
        {
            _disabled.Set(e.EntityId, false);
        }

        public override void EntityDisabled(Entity e)
        {
            _disabled.Set(e.EntityId, true);
        }

        public override void EntityDeleted(Entity e)
        {
            _entities.Remove(e.EntityId);
            _disabled.Set(e.EntityId, false);

            _idProvider.CheckIn(e.EntityId);
            _activeCount--;
            _deletedCount++;
        }
    
        /// <summary>
        /// Check if this entity is active.  Active means the entity is being actively processed.
        /// </summary>
        /// <param name="entityId">Id of the Entity to check.</param>
        /// <returns>True if active, False if not.</returns>
        public bool IsActive(int entityId)
        {
            return _entities[entityId] != null;
        }

        /// <summary>
        /// Check if this entity is enabled.
        /// </summary>
        /// <param name="entityId">Id of the Entity to check.</param>
        /// <returns>True if enabled, False if disabled.</returns>
        public bool IsEnabled(int entityId)
        {
            return !_disabled.Get(entityId);
        }

        /// <summary>
        /// Retrieve an entity with this id.
        /// </summary>
        /// <param name="entityId">The id of the entity to retrieve.</param>
        /// <returns>The entity.</returns>
        public Entity GetEntity(int entityId)
        {
            return _entities[entityId];
        }

        /// <summary>
        /// Get how many entities are active in this world.
        /// </summary>
        /// <returns>The number of active entities.</returns>
        public int GetActiveEntityCount()
        {
            return _activeCount;
        }

        /// <summary>
        /// Get how many entities have been created in the world since start.
        ///
        /// Note: A created entity may not have been added to the world, thus
        /// created count is always equal or larger than added count.
        /// </summary>
        /// <returns>The number of entities</returns>
        public long GetTotalCreated()
        {
            return _createdCount;
        }

        /// <summary>
        /// Get how many entities have been added to the world since start.
        /// </summary>
        /// <returns>The number of entities</returns>
        public long GetTotalAdded()
        {
            return _addedCount;
        }

        /// <summary>
        /// Get how many entities have been deleted from the world since start.
        /// </summary>
        /// <returns>The number of entities</returns>
        public long GetTotalDeleted()
        {
            return _deletedCount;
        }

        protected override void Initialize() { }

        /// <summary>
        /// Used only internally to generate distinct ids for entities and reuse them.
        /// </summary>
        private class IdProvider
        {
            private Stack<int> _availableIds;
            private int _nextAvailableId;

            public IdProvider()
            {
                _availableIds = new Stack<int>();
            }

            public int CheckOut()
            {
                if (_availableIds.Count > 0)
                {
                    return _availableIds.Pop();
                }

                return _nextAvailableId++;
            }

            public void CheckIn(int id)
            {
                _availableIds.Push(id);
            }
        }
    }
}
