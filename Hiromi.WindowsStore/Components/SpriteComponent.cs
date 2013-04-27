using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Hiromi.Rendering;

namespace Hiromi.Components
{
    public class SpriteComponent : GameObjectComponent, IRenderingComponent
    {
        private bool _isVisible;
        private Texture2D _texture;

        public bool IsVisible { get { return _isVisible; } set { _isVisible = value; OnRenderingComponentChanged(); } }
        public Texture2D Texture { get { return _texture; } set { _texture = value; OnRenderingComponentChanged(); } }

        public SpriteComponent(Texture2D texture)
        {
            _texture = texture;
            _isVisible = true;
        }

        public override void Loaded()
        {
            this.GameObject.MessageManager.TriggerMessage(new NewRenderingComponentMessage(this));
        }

        public SceneNode GetSceneNode()
        {
            return new SpriteRenderingNode(this.GameObject.Id,
                this.GameObject.GetComponent<PositionComponent>(),
                RenderPass.GameObjectPass,
                this);
        }

        private void OnRenderingComponentChanged()
        {
            this.GameObject.MessageManager.TriggerMessage(new RenderingComponentChangedMessage(this.GameObject, this));
        }
    }
}
