using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Hiromi
{
    public enum BehaviorState
    {
        Uninitialized = 0,
        Initialized
    }

    public class GameObjectBehavior
    {
        public GameObject GameObject { get; set; }
        public BehaviorState State { get; set; }

        public GameObjectBehavior()
        {
            this.State = BehaviorState.Uninitialized;
        }

        public void Initialize()
        {
            OnInitialize();
            this.State = BehaviorState.Initialized;
        }

        public void Update(GameTime gameTime)
        {
            OnUpdate(gameTime);
        }

        protected virtual void OnInitialize() { }
        protected virtual void OnUpdate(GameTime gameTime) { }
    }
}
