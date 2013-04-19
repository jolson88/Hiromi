using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hiromi.Messaging;
using Hiromi.Components;
using Microsoft.Xna.Framework;

namespace Hiromi.Systems
{
    public class SimplePhysicsSystem : GameSystem
    {
        protected override void OnUpdate(GameTime gameTime)
        {
            foreach (var obj in this.GameObjects.Values)
            {
                var pos = obj.GetComponent<PositionComponent>();
                var phys = obj.GetComponent<SimplePhysicsComponent>();

                pos.Position += phys.Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
        }

        protected override bool IsGameObjectForSystem(GameObject obj)
        {
            return obj.HasComponent<PositionComponent>() && obj.HasComponent<SimplePhysicsComponent>();
        }
    }
}
