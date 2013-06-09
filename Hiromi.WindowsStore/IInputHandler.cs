using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Hiromi
{
    public interface IInputHandler
    {
        void OnKeyDown(Keys key);
        void OnKeyUp(Keys key);
        void OnPointerPress(Vector2 location);
        void OnPointerRelease(Vector2 location);
        void OnPointerMove(Vector2 location);
    }
}
