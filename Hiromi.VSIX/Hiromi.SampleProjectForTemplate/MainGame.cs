using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Hiromi;
using Hiromi.SampleProjectForTemplate.Screens;

namespace Hiromi.SampleProjectForTemplate
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