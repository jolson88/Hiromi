using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hiromi;
using Hiromi.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Hiromi.Rendering
{
    public class SpriteRenderingNode : SceneNode
    {
        private SpriteComponent _spriteComponent;

        public SpriteRenderingNode(int gameObjectId, PositionComponent positionComponent, RenderPass renderPass, SpriteComponent spriteComponent) 
            : base(gameObjectId, positionComponent, renderPass) 
        {
            _spriteComponent = spriteComponent;
        }

        protected override void OnInitialize()
        {
            this.MessageManager.AddListener<RenderingComponentChangedMessage>(OnRenderingComponentChanged);
        }

        protected override void OnDraw(GameTime gameTime)
        {
            // We use Bounds instead of Position as Bounds takes the achor point into account
            GraphicsService.Instance.SpriteBatch.Draw(_spriteComponent.Texture,
                new Vector2(this.PositionComponent.Bounds.X * GraphicsService.Instance.GraphicsDevice.Viewport.Width,
                    this.PositionComponent.Bounds.Y * GraphicsService.Instance.GraphicsDevice.Viewport.Height),
                Color.White);
        }

        private void OnRenderingComponentChanged(RenderingComponentChangedMessage msg)
        {
            _spriteComponent = msg.GameObject.GetComponent<SpriteComponent>();
            this.IsVisible = _spriteComponent.IsVisible;
        }
    }
}
