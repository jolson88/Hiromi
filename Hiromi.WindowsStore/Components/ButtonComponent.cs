using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Hiromi;
using Hiromi.Rendering;

namespace Hiromi.Components
{
    public class ButtonComponent : GameObjectComponent //, IRenderingComponent
    {
        public Texture2D CurrentTexture { get; set; }
        public Texture2D FocusTexture { get; set; }
        public Texture2D NonFocusTexture { get; set; }

        public ButtonComponent(Texture2D nonFocusTexture, Texture2D focusTexture)
        {
            this.CurrentTexture = nonFocusTexture;
            this.FocusTexture = focusTexture;
            this.NonFocusTexture = nonFocusTexture;
        }

        public override void Loaded()
        {
            this.GameObject.MessageManager.AddListener<PointerExitMessage>(OnPointerExit);
            this.GameObject.MessageManager.AddListener<PointerPressMessage>(OnPointerPress);
            this.GameObject.MessageManager.AddListener<PointerReleaseMessage>(OnPointerRelease);

            //this.GameObject.MessageManager.TriggerMessage(new NewRenderingComponentMessage(this));
        }

        //public SceneNode GetSceneNode()
        //{
        //    return new ButtonRenderingNode(this.GameObject.Id,
        //        this.GameObject.GetComponent<TransformationComponent>(),
        //        RenderPass.UserInterfacePass,
        //        this);
        //}

        private void OnPointerExit(PointerExitMessage msg)
        {
            if (this.GameObject.Id == msg.GameObjectId)
            {
                if (this.NonFocusTexture != null)
                {
                    this.CurrentTexture = this.NonFocusTexture;
                }
            }
        }

        private void OnPointerPress(PointerPressMessage msg)
        {
            if (this.GameObject.Id == msg.GameObjectId)
            {
                this.GameObject.MessageManager.TriggerMessage(new ButtonPressMessage(this.GameObject.Id));
                if (this.FocusTexture != null)
                {
                    this.CurrentTexture = this.FocusTexture;
                }
            }
        }

        private void OnPointerRelease(PointerReleaseMessage msg)
        {
            if (this.GameObject.Id == msg.GameObjectId)
            {
                if (this.NonFocusTexture != null)
                {
                    this.CurrentTexture = this.NonFocusTexture;
                }
            }
        }
    }
}
