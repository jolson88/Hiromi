using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Hiromi.Rendering;
using Hiromi;

namespace Hiromi.Components
{
    public class BackgroundComponent : GameObjectComponent, IRenderingComponent
    {
        public Texture2D Texture { get; set; }

        public BackgroundComponent(Texture2D texture)
        {
            this.Texture = texture;
        }

        public override void Loaded()
        {
            this.GameObject.MessageManager.TriggerMessage(new NewRenderingComponentMessage(this));
        }

        public SceneNode GetSceneNode()
        {
            return new BackgroundRenderingNode(this.GameObject.Id,
                null, // No position component for backgrounds
                RenderPass.BackgroundPass,
                this);
        }
    }
}
