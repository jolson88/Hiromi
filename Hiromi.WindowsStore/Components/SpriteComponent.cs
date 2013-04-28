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
    public enum SpriteKind
    {
        GameObject = 0,
        Background = 1,
        UserInterface = 2
    }

    public class SpriteComponent : GameObjectComponent, IRenderingComponent
    {
        public bool IsVisible { get; set; }
        public Texture2D Texture { get; set; }
        public SpriteKind SpriteKind { get; set; }
        public bool TransformedByCamera { get; set; }

        public SpriteComponent(Texture2D texture) : this(texture, SpriteKind.GameObject) { }
        public SpriteComponent(Texture2D texture, SpriteKind spriteKind, bool transformedByCamera = true)
        {
            this.Texture = texture;
            this.SpriteKind = spriteKind;
            this.IsVisible = true;
            this.TransformedByCamera = transformedByCamera;
        }

        public override void Loaded()
        {
            this.GameObject.MessageManager.TriggerMessage(new NewRenderingComponentMessage(this));
        }

        public SceneNode GetSceneNode()
        {
            var pass = RenderPass.GameObjectPass;
            if (this.SpriteKind == SpriteKind.Background)
            {
                pass = RenderPass.BackgroundPass;
            }
            else if (this.SpriteKind == SpriteKind.UserInterface)
            {
                pass = RenderPass.UserInterfacePass;
            }

            return new SpriteRenderingNode(this.GameObject.Id,
                this.GameObject.GetComponent<TransformationComponent>(),
                pass,
                this);
        }
    }
}
