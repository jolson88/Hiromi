using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Hiromi.Messaging;

namespace Hiromi.Processing
{
    public class BoundsCheckingProcess : Process
    {
        protected override void OnUpdate(GameTime gameTime)
        {
            var viewport = GraphicsService.Instance.GraphicsDevice.Viewport;
            foreach (var obj in GameObjectService.Instance.GetAllGameObjects())
            {
                if (!obj.Bounds.Intersects(viewport.Bounds))
                {
                    MessageService.Instance.QueueMessage(new OffScreenMessage(obj.Id));
                }
            }
        }
    }
}
