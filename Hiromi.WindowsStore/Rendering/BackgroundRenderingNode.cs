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
    public class BackgroundRenderingNode : SceneNode
    {
        private BackgroundComponent _backgroundComponent;

        public BackgroundRenderingNode(int gameObjectId, PositionComponent positionComponent, RenderPass renderPass, BackgroundComponent backgroundComponent)
            : base(gameObjectId, positionComponent, renderPass)
        {
            _backgroundComponent = backgroundComponent;
        }

        protected override void OnInitialize()
        {
            this.MessageManager.AddListener<RenderingComponentChangedMessage>(OnRenderingComponentChanged);
        }

        protected override void OnDraw(GameTime gameTime, SceneGraph scene)
        {
            scene.SpriteBatch.Draw(_backgroundComponent.Texture,
                new Rectangle(0, 0, GraphicsService.Instance.GraphicsDevice.Viewport.Width, GraphicsService.Instance.GraphicsDevice.Viewport.Height),
                Color.White);
        }

        private void OnRenderingComponentChanged(RenderingComponentChangedMessage msg)
        {
            if (msg.GameObject.Id == this.GameObjectId)
            {
                _backgroundComponent = msg.GameObject.GetComponent<BackgroundComponent>();
            }
        }
    }
}
