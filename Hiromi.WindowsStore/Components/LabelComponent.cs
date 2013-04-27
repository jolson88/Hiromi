using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Hiromi.Components
{
    public class LabelComponent : GameObjectComponent
    {
        public string Text { get { return _text; } set { _text = value; OnTextChanged(); } }
        public SpriteFont Font { get; set; }
        public Color TextColor { get; set; }

        private string _text;

        public LabelComponent(string text, SpriteFont font) : this(text, font, Color.White) { }
        public LabelComponent(string text, SpriteFont font, Color textColor)
        {
            _text = text;
            this.Font = font;
            this.TextColor = textColor;
        }

        public override void Loaded()
        {
            OnTextChanged();
        }

        public override void Draw(GameTime gameTime)
        {
            // TODO: Move to LabelRenderingNode (new SceneNode)
            var posComponent = this.GameObject.GetComponent<PositionComponent>();
            GraphicsService.Instance.SpriteBatch.DrawString(this.Font, this.Text,
                new Vector2(posComponent.Bounds.X * GraphicsService.Instance.GraphicsDevice.Viewport.Width,
                    posComponent.Bounds.Y * GraphicsService.Instance.GraphicsDevice.Viewport.Height),
                this.TextColor);
        }

        private void OnTextChanged()
        {
            var posComponent = this.GameObject.GetComponent<PositionComponent>();

            var textSize = this.Font.MeasureString(this.Text);
            posComponent.Bounds.Width = textSize.X / GraphicsService.Instance.GraphicsDevice.Viewport.Width;
            posComponent.Bounds.Height = textSize.Y / GraphicsService.Instance.GraphicsDevice.Viewport.Height;
        }
    }
}
