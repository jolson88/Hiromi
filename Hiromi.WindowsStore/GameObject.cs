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
        public bool IsVisible { get; set; }
        public Sprite Sprite { get { return _sprite; } set { _sprite = value; CalculateBounds(); } }

        /// <summary>
        /// The position of the GameObject in Screen Coordinates ((0,0) through (1,1))
        /// </summary>
        public Vector2 Position { get { return _position; } set { _position = value; CalculateBounds(); } }

        /// <summary>
        /// The bounding box of the GameObject in Pixel Coordinates ((0,0) through (ViewportWidth,ViewportHeight))
        /// </summary>
        public Rectangle Bounds { get; set; }
        public ProcessManager ProcessManager { get; set; }

        private Sprite _sprite;
        private Vector2 _position;
        private Dictionary<Type, GameObjectBehavior> _behaviors;       

        public GameObject()
        {
            _behaviors = new Dictionary<Type, GameObjectBehavior>();
            this.IsVisible = true;
            this.Tag = string.Empty;
            this.Position = Vector2.Zero;
            this.ProcessManager = new ProcessManager();
            this.Bounds = new Rectangle(0, 0, 0, 0);
            this.OnInitialize();
        }

        public void AddBehavior(GameObjectBehavior behavior)
        {
            behavior.GameObject = this;
            _behaviors.Add(behavior.GetType(), behavior);
        }

        public List<GameObjectBehavior> GetAllBehaviors()
        {
            return _behaviors.Values.ToList();
        }

        public T GetBehavior<T>() where T : GameObjectBehavior
        {
            return (T)_behaviors[typeof(T)];
        }

        public void Update(GameTime gameTime)
        {
            this.ProcessManager.Update(gameTime);
            foreach (var behavior in _behaviors.Values)
            {
                behavior.Update(gameTime);
            }
        }

        private void CalculateBounds()
        {
            // Remember: position is in screen coordinates. Bounding box should be in pixel coordinate. 
            // Also remember to account for "center" offset of sprite if present.
            var xOffset = this.Sprite != null ? this.Sprite.Center.X : 0;
            var yOffset = this.Sprite != null ? this.Sprite.Center.Y : 0;
            var width = this.Sprite != null ? this.Sprite.Texture.Width : 0;
            var height = this.Sprite != null ? this.Sprite.Texture.Height : 0;
            this.Bounds = new Rectangle((int)(this.Position.X * GraphicsService.Instance.GraphicsDevice.Viewport.Width - xOffset), 
                (int)(this.Position.Y * GraphicsService.Instance.GraphicsDevice.Viewport.Height - yOffset), 
                width, 
                height);
        }

        protected virtual void OnInitialize() { }
    }
}
