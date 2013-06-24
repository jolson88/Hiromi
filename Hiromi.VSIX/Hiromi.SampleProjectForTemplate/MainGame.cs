using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Hiromi;
using $safeprojectname$.Screens;

namespace $safeprojectname$
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class MainGame : HiromiGame
    {
        protected override Screen GetInitialScreen()
        {
            return new PlayScreen();
        }
    }
}