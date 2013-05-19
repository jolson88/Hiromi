using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Hiromi
{
    public class GameObjectComponentRemovedEventArgs : EventArgs
    {
        public GameObject GameObject { get; set; }

        public GameObjectComponentRemovedEventArgs(GameObject obj)
        {
            this.GameObject = obj;
        }
    }

    public class GameObjectComponent
    {
        public EventHandler<GameObjectComponentRemovedEventArgs> Removed { get; set; }
        public GameObject GameObject { get; set; }

        public virtual void Loaded() 
        {
            this.OnLoaded();
        }
        
        public virtual void Update(GameTime gameTime) 
        {
            OnUpdate(gameTime);
        }
        
        public virtual void Remove() 
        {
            this.OnRemove();

            if (this.Removed != null)
            {
                this.Removed(this, new GameObjectComponentRemovedEventArgs(this.GameObject));
            }
        }

        protected virtual void OnLoaded() { }
        protected virtual void OnUpdate(GameTime gameTime) { }
        protected virtual void OnRemove() { }
    }
}
