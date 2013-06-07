using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Hiromi.Components
{
    // ******** LEGACY: Will move to Entities.Components when appropriate

    /*
    public class SpriteComponent : GameObjectComponent
    {
        public int GameObjectId { get { return this.GameObject.Id; } }
        public TransformationComponent Transform { get { return this.GameObject.Transform; } }
        public float Alpha { get { return _color.A; } set { _color.A = (byte)(value * 255); } }
        public bool IsVisible { get { return this.Alpha < 1.0f; } }
        public Texture2D Texture { get; set; }

        private Color _color;

        public SpriteComponent(Texture2D texture)
        {
            this.Texture = texture;
            this.Alpha = 0f;
            _color = Color.White;
        }

        public void Draw(GameTime gameTime, SpriteBatch batch)
        {
            var origin = new Vector2(this.GameObject.Transform.Bounds.Width / 2f, this.GameObject.Transform.Bounds.Height / 2f);

            // Scaling happens at Top-Left in Draw call. So we also have to offset half the difference (so the scaling factor is offset)
            // This ensures the center of the sprite remains centered and it scales around the origin properly
            // Remember, we need to "flip" the scale (as our game engine has Y+ up instead of down
            var scale = new Vector2(1, -1) * (this.GameObject.Transform.Bounds.Width / this.Texture.Width);
            var scaleOffset = new Vector2((this.GameObject.Transform.Bounds.Width - this.Texture.Width) / 2f,
                                    -(this.GameObject.Transform.Bounds.Height - this.Texture.Height) / 2f);

            // We use Bounds instead of Position as Bounds takes the achor point into account
            batch.Draw(this.Texture,
                new Vector2((int)this.GameObject.Transform.Bounds.Left + origin.X, (int)this.GameObject.Transform.Bounds.Top - origin.Y) + scaleOffset,
                null,
                _color,
                this.GameObject.Transform.Rotation,
                origin,
                scale,
                SpriteEffects.None,
                0f);
        }
    }
     */
}
