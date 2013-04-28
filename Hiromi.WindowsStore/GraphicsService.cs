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

        public GraphicsDevice GraphicsDevice { get; set; }
        
        private GraphicsService() { }
    }
}
