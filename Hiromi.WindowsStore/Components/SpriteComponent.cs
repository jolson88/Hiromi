﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Hiromi.Components
{
    public class SpriteComponent : GameObjectComponent, IRenderAwareComponent
    {
        public int GameObjectId { get { return this.GameObject.Id; } }
        public RenderPass RenderPass { get { return RenderPass.GameObjectPass; } }
        public TransformationComponent Transform { get { return this.GameObject.Transform; } }
        public bool IsVisible { get; set; }
        public Texture2D Texture { get; set; }

        public SpriteComponent(Texture2D texture)
        {
            this.Texture = texture;
            this.IsVisible = true;
        }

        public void Draw(GameTime gameTime, SpriteBatch batch)
        {
            // Remember, we need to "flip" the scale (as our game engine has Y+ up instead of down
            var scale = new Vector2(1, -1) * this.GameObject.Transform.Scale;
            var origin = new Vector2(this.GameObject.Transform.Bounds.Width / 2, this.GameObject.Transform.Bounds.Height / 2);

            // We use Bounds instead of Position as Bounds takes the achor point into account
            batch.Draw(this.Texture,
                new Vector2((int)this.GameObject.Transform.Bounds.X + origin.X, (int)this.GameObject.Transform.Bounds.Y - origin.Y),
                null,
                Color.White,
                this.GameObject.Transform.Rotation,
                origin,
                scale,
                SpriteEffects.None,
                0f);
        }
    }
}
