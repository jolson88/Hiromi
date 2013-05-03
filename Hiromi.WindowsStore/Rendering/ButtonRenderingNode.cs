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
//    public class ButtonRenderingNode : SceneNode
//    {
//        private ButtonComponent _buttonComponent;

//        public ButtonRenderingNode(int gameObjectId, TransformationComponent positionComponent, RenderPass renderPass, ButtonComponent buttonComponent)
//            : base(gameObjectId, positionComponent, renderPass)
//        {
//            _buttonComponent = buttonComponent;
//        }

//        protected override void OnDraw(GameTime gameTime, SpriteBatch batch)
//        {
//            if (this.TransformationComponent.TransformedByCamera)
//            {
//                // Remember, we need to "flip" the scale (as our game engine has Y+ up instead of down
//                var scale = new Vector2(1, -1) * this.TransformationComponent.Scale;

//                // We use Bounds instead of Position as Bounds takes the achor point into account
//                batch.Draw(_buttonComponent.CurrentTexture,
//                    new Vector2((int)this.TransformationComponent.Bounds.X, (int)this.TransformationComponent.Bounds.Y),
//                    null,
//                    Color.White,
//                    0f,
//                    Vector2.Zero,
//                    scale,
//                    SpriteEffects.None,
//                    0f);
//            }
//            else
//            {
//                batch.Draw(_buttonComponent.CurrentTexture,
//                    new Rectangle((int)this.TransformationComponent.Bounds.X,
//                        (int)this.TransformationComponent.Bounds.Y,
//                        (int)this.TransformationComponent.Bounds.Width,
//                        (int)this.TransformationComponent.Bounds.Height),
//                    null,
//                    Color.White);
//            }
//        }
//    }
//}
