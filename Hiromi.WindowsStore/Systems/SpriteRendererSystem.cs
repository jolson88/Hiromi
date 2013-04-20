using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Hiromi.Components;

namespace Hiromi.Systems
{
    public class SpriteRendererSystem : GameSystem
    {
        private SpriteBatch _batch;

        public SpriteRendererSystem()
        {
            _batch = new SpriteBatch(GraphicsService.Instance.GraphicsDevice);
        }

        protected override void OnDraw(GameTime gameTime)
        {
            _batch.Begin();
            foreach (var obj in this.GameObjects.Values)
            {
                var posComponent = obj.GetComponent<PositionComponent>();
                var spriteComponent = obj.GetComponent<SpriteComponent>();

                if (spriteComponent.IsVisible)
                {
                    // We use Bounds instead of Position as Bounds takes the achor point into account
                    _batch.Draw(spriteComponent.Texture,
                        new Vector2(posComponent.Bounds.X * GraphicsService.Instance.GraphicsDevice.Viewport.Width,
                            posComponent.Bounds.Y * GraphicsService.Instance.GraphicsDevice.Viewport.Height),
                        Color.White);
                }
            }
            _batch.End();
        }

        protected override bool IsGameObjectForSystem(GameObject obj)
        {
            return obj.HasComponent<PositionComponent>() &&
                obj.HasComponent<SpriteComponent>();
        }
    }
}
