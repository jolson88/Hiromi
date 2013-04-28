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
        public bool IsVisible { get; set; }
        public Texture2D Texture { get; set; }

        public SpriteComponent(Texture2D texture)
        {
            this.Texture = texture;
            this.IsVisible = true;
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
    }
}
