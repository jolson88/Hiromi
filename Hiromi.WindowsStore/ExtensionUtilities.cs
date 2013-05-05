using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Hiromi
{
    public static class ExtensionUtilities
    {
        public static void DrawLine(this SpriteBatch batch, float width, Color color, Vector2 point1, Vector2 point2)
        {
            float angle = (float)Math.Atan2(point2.Y - point1.Y, point2.X - point1.X);
            float length = Vector2.Distance(point1, point2);
 
            batch.Draw(GraphicsService.Instance.Blank, point1, null, color,
                        angle, Vector2.Zero, new Vector2(length, -width),
                        SpriteEffects.None, 0);
        }

        public static void DrawPoint(this SpriteBatch batch, float size, Color color, Vector2 point)
        {
            batch.Draw(GraphicsService.Instance.Blank, point, null, color, 0, Vector2.Zero, size, SpriteEffects.None, 0);
        }
    }
}
