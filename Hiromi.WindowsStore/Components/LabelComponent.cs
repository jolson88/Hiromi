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
    public class LabelComponent : GameObjectComponent, IRenderingComponent
    {
        public string Text { get { return _text; } set { _text = value; OnTextChanged(); } }
        public SpriteFont Font { get; set; }
        public Color TextColor { get; set; }
        
        private string _text;

        public LabelComponent(string text, SpriteFont font, Color textColor)
        {
            _text = text;
            this.Font = font;
            this.TextColor = textColor;
        }

        public override void Loaded()
        {
            OnTextChanged();
            this.GameObject.MessageManager.TriggerMessage(new NewRenderingComponentMessage(this));
        }

        public SceneNode GetSceneNode()
        {
            return new LabelRenderingNode(this.GameObject.Id,
                this.GameObject.GetComponent<TransformationComponent>(),
                RenderPass.UserInterfacePass,
                this);
        }

        private void OnTextChanged()
        {
            var posComponent = this.GameObject.GetComponent<TransformationComponent>();

            var textSize = this.Font.MeasureString(this.Text);
            posComponent.Bounds.Width = textSize.X;
            posComponent.Bounds.Height = textSize.Y;
        }
    }
}
