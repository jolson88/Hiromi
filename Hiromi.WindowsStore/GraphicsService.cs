using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Hiromi
{
    public class GraphicsService
    {
        private static GraphicsService _instance;
        public static GraphicsService Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GraphicsService();
                }
                return _instance;
            }
        }

        public GraphicsDevice GraphicsDevice 
        { 
            get 
            { 
                return _graphicsDevice; 
            } 
            set 
            { 
                _graphicsDevice = value; 
                this.DesignedScreenSize = new Vector2(value.Viewport.Width, value.Viewport.Height);
                this.Blank = new Texture2D(value, 1, 1, false, SurfaceFormat.Color);
                this.Blank.SetData(new[] { Color.White });
            } 
        }

        public Vector2 DesignedScreenSize { get; set; }
        public Texture2D Blank { get; set; }

        private GraphicsDevice _graphicsDevice;

        private GraphicsService() { }
    }
}
