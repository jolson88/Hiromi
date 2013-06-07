using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Hiromi;

namespace Hiromi.Components
{
    /*
    public class LabelComponent : GameObjectComponent
    {
        public int GameObjectId { get { return this.GameObject.Id; } }
        public TransformationComponent Transform { get { return this.GameObject.Transform; } }

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

        protected override void OnLoaded()
        {
            OnTextChanged();
        }

        public void Draw(GameTime gameTime, SpriteBatch batch)
        {
            batch.DrawString(this.Font, this.Text,
                new Vector2(this.Transform.Bounds.Left,
                    this.Transform.Bounds.Top),
                this.TextColor,
                0f,
                Vector2.Zero,
                new Vector2(1, -1),
                SpriteEffects.None,
                0f);
        }

        private void OnTextChanged()
        {
            var textSize = this.Font.MeasureString(this.Text);
            this.Transform.OriginalWidth = textSize.X;
            this.Transform.OriginalHeight = textSize.Y;
            //this.Transform.Bounds.Width = textSize.X;
            //this.Transform.Bounds.Height = textSize.Y;
        }
    }
     */
}
