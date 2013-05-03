//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Hiromi;
//using Hiromi.Components;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;

//namespace Hiromi.Rendering
//{
//    public class LabelRenderingNode : SceneNode
//    {
//        private LabelComponent _labelComponent;

//        public LabelRenderingNode(int gameObjectId, TransformationComponent positionComponent, RenderPass renderPass, LabelComponent labelComponent)
//            : base(gameObjectId, positionComponent, renderPass)
//        {
//            _labelComponent = labelComponent;
//        }

//        protected override void OnDraw(GameTime gameTime, SpriteBatch batch)
//        {
//            batch.DrawString(_labelComponent.Font, _labelComponent.Text,
//                new Vector2(this.TransformationComponent.Bounds.Left,
//                    this.TransformationComponent.Bounds.Top),
//                _labelComponent.TextColor,
//                0f,
//                Vector2.Zero,
//                new Vector2(1,-1),
//                SpriteEffects.None,
//                0f);
//        }
//    }
//}
