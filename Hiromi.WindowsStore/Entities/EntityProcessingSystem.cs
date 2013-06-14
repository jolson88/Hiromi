/* 
//------------------------------------------------
//
// This is a port of the awesome Entity System framework Artemis by Arni Arent and Tiago Costa - http://gamadu.com/artemis 
// This is a modification/refactoring of the Artemis_CSharp port to bring back some of the simplicity of the original Java project.
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
    public abstract class EntityProcessingSystem : EntitySystem
    {
        /// <summary>
        /// Update an entity this system is interested in.
        /// </summary>
        /// <param name="entity">The entity to process</param>
        protected virtual void UpdateEntity(Entity entity) { }

        protected sealed override void UpdateEntities(IEnumerable<Entity> entities)
        {
            foreach (var entity in entities)
            {
                UpdateEntity(entity);
            }
        }

        /// <summary>
        /// Draw an entity this system is interested in.
        /// </summary>
        /// <param name="entity">The entity to process</param>
        protected virtual void DrawEntity(Entity entity) { }

        protected sealed override void DrawEntities(IEnumerable<Entity> entities)
        {
            foreach (var entity in entities)
            {
                DrawEntity(entity);
            }
        }

        protected override bool CheckProcessing()
        {
            return true;
        }
    }
}
