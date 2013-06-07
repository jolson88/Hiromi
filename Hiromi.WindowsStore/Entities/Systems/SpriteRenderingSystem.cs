using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Hiromi.Entities.Components;

namespace Hiromi.Entities.Systems
{
    public class SpriteRenderingSystem : EntityProcessingSystem
    {
        private SpriteBatch _batch;
        private Camera _camera;

        public SpriteRenderingSystem(Camera camera)
        {
            _camera = camera;
        }

        public override void Initialize()
        {
            _batch = new SpriteBatch(GraphicsService.Instance.GraphicsDevice);
        }

        protected override void BeginDraw()
        {
            _batch.Begin(_camera);
        }

        protected override void DrawEntity(Entity entity)
        {
            var transform = entity.GetComponent<TransformComponent>();
            var sprite = entity.GetComponent<SpriteComponent>();

            _batch.Draw(sprite.Texture, 
                new Rectangle((int)transform.Position.X, (int)transform.Position.Y, sprite.Texture.Width, sprite.Texture.Height), 
                Color.White);
        }

        protected override void EndDraw()
        {
            _batch.End();
        }

        protected override bool InterestedInEntity(Entity entity)
        {
            var hasTransform = entity.HasComponent<TransformComponent>();
            var hasSprite = entity.HasComponent<SpriteComponent>();

            return entity.HasComponent<TransformComponent>() && entity.HasComponent<SpriteComponent>();
        }
    }
}
