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
    public abstract class EntitySystem : IEntityObserver
    {
        public EntityWorld World { get; set; }
        public bool IsPassive { get; set; }

        private List<Entity> _activeEntities;

        public EntitySystem()
        {
            _activeEntities = new List<Entity>();
        }

        /// <summary>
        /// Retrieve the entities the system is currently interested in.
        /// </summary>
        /// <returns>The list of interested entities</returns>
        public IEnumerable<Entity> GetActiveEntities()
        {
            return _activeEntities;
        }

        /// <summary>
        /// Any implementing entity system can implement this method and the logic
        /// to update the given entities of the system.
        /// </summary>
        /// <param name="entities">The entities for this system to update</param>
        protected virtual void UpdateEntities(IEnumerable<Entity> entities) { }

        /// <summary>
        /// Any implementing entity system can implement this method and the logic
        /// to draw the given entities of the system.
        /// </summary>
        /// <param name="entities">The entities for this system to draw</param>
        protected virtual void DrawEntities(IEnumerable<Entity> entities) { }

        /// <summary>
        /// Determines whether the system should be processed or not.
        /// </summary>
        /// <returns>True if the system should be processed, False if not.</returns>
        protected abstract bool CheckProcessing();

        /// <summary>
        /// Called before updating of entities begins.
        /// </summary>
        protected virtual void BeginUpdate() { }

        /// <summary>
        /// Called after the updating of entities ends.
        /// </summary>
        protected virtual void EndUpdate() { }

        /// <summary>
        /// Called before drawing of entities begins.
        /// </summary>
        protected virtual void BeginDraw() { }

        /// <summary>
        /// Called after the drawing of entities ends.
        /// </summary>
        protected virtual void EndDraw() { }
        
        public void Update()
        {
            if (CheckProcessing())
            {
                BeginUpdate();
                UpdateEntities(_activeEntities);
                EndUpdate();
            }
        }

        public void Draw()
        {
            if (CheckProcessing())
            {
                BeginDraw();
                DrawEntities(_activeEntities);
                EndDraw();
            }
        }

        /// <summary>
        /// Override to implement code that gets executed when systems are initialized.
        /// </summary>
        public virtual void Initialize() { }

        /// <summary>
        /// Called if the system has received a entity it is interested in, e.g. created or a component was added to it.
        /// </summary>
        /// <param name="entity">The entity that was added to the system</param>
        protected virtual void Inserted(Entity entity) { }

        /// <summary>
        /// Called if a entity was removed from this system, e.g. deleted or had one of it's components removed.
        /// </summary>
        /// <param name="entity">The entity that was removed from the system</param>
        protected virtual void Removed(Entity entity) { }

        /// <summary>
        /// Override so system can report whether it is interested in this entity or not (usually based on what components the entity has).
        /// </summary>
        /// <param name="entity">The entity to check.</param>
        /// <returns>True if the system is interested, false if not.</returns>
        protected virtual bool InterestedInEntity(Entity entity) { return false; }

        /// <summary>
        /// Will check if the entity is of interest to this system.
        /// </summary>
        /// <param name="entity">The entity to check.</param>
        protected void CheckInterest(Entity entity)
        {
            var interested = InterestedInEntity(entity);
            var contains = _activeEntities.Contains(entity);

            if (interested && !contains)
            {
                InsertIntoSystem(entity);
            }
            else if (!interested && contains)
            {
                RemoveFromSystem(entity);
            }

        }

        private void RemoveFromSystem(Entity entity)
        {
            _activeEntities.Remove(entity);
            Removed(entity);
        }

        private void InsertIntoSystem(Entity entity)
        {
            _activeEntities.Add(entity);
            Inserted(entity);
        }

        public void EntityAdded(Entity e)
        {
            CheckInterest(e);
        }

        public void EntityChanged(Entity e)
        {
            CheckInterest(e);
        }

        public void EntityDeleted(Entity e)
        {
            if (_activeEntities.Contains(e))
            {
                RemoveFromSystem(e);
            }
        }

        public void EntityDisabled(Entity e)
        {
            if (_activeEntities.Contains(e))
            {
                RemoveFromSystem(e);
            }
        }

        public void EntityEnabled(Entity e)
        {
            CheckInterest(e);
        }
    }
}
