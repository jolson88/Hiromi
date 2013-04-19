using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hiromi.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Hiromi.Systems
{
    public class BackgroundRenderingSystem : GameSystem
    {
        private SpriteBatch _batch;

        public BackgroundRenderingSystem()
        {
            _batch = new SpriteBatch(GraphicsService.Instance.GraphicsDevice);
        }

        protected override void OnDraw(GameTime gameTime)
        {
            GraphicsService.Instance.GraphicsDevice.Clear(Color.CornflowerBlue);
            _batch.Begin();

            foreach (var obj in this.GameObjects.Values)
            {
                var bg = obj.GetComponent<BackgroundComponent>();
                _batch.Draw(bg.Background.Texture,
                    new Rectangle(0, 0, GraphicsService.Instance.GraphicsDevice.Viewport.Width, GraphicsService.Instance.GraphicsDevice.Viewport.Height),
                    Color.White);
            }

            _batch.End();
        }

        protected override bool IsGameObjectForSystem(GameObject obj)
        {
            return obj.HasComponent<BackgroundComponent>();
        }
    }
}
