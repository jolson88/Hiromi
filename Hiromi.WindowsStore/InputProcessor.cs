using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hiromi;
using Hiromi.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Hiromi.Messaging;

namespace Hiromi
{
    public class InputProcessor
    {
        // TODO: Enable maximum touch points supported to be specified by game
        private static int MAXIMUM_SUPPORTED_TOUCH_POINTS = 4;
        private KeyboardState _oldKeyState;
        private MouseState _oldMouseState;
        private TouchCollection _oldTouchState;
        private bool _touchSupported = false;

        public IInputHandler InputHandler { get; set; }

        public InputProcessor()
        {
            _oldMouseState = Mouse.GetState();

            var touchCapabilities = TouchPanel.GetCapabilities();
            if (touchCapabilities.IsConnected)
            {
                _touchSupported = true;
                _oldTouchState = TouchPanel.GetState();
            }
        }

        public void Process()
        {
            ProcessKeyboardInput();
            ProcessPointerInput();
        }

        private void ProcessKeyboardInput()
        {
            var newKeyState = Keyboard.GetState();
            CalculateKeyDownMessages(newKeyState);
            CalculateKeyUpMessages(newKeyState);
            _oldKeyState = newKeyState;
        }

        private void ProcessPointerInput()
        {
            var newMouseState = Mouse.GetState();
            CalculateMouseMessages(newMouseState);
            _oldMouseState = newMouseState;

            // Don't do multi-touch support on Phone (first touch point will be a mouse message)
#if !WINDOWS_PHONE
            if (_touchSupported)
            {
                var newTouchState = TouchPanel.GetState();
                CalculateTouchMessages(newTouchState);
                _oldTouchState = newTouchState;
            }
#endif
        }

        private void CalculateMouseMessages(MouseState newMouseState)
        {
            if (MouseStateHasChanged(newMouseState))
            {
                if (LeftMouseButtonNewlyPressed(newMouseState))
                {
                    InputHandler.OnPointerPress(new Vector2(newMouseState.X, newMouseState.Y));
                }
                else if (LeftMouseButtonNewlyReleased(newMouseState))
                {
                    InputHandler.OnPointerRelease(new Vector2(newMouseState.X, newMouseState.Y));
                }
            }
        }

        private void CalculateTouchMessages(TouchCollection newTouchState)
        {
            if (TouchStateHasChanged(newTouchState))
            {
                // Current touch points
                for (int i = 0; i < Math.Min(newTouchState.Count, MAXIMUM_SUPPORTED_TOUCH_POINTS); i++)
                {
                    var touch = newTouchState[i];
                    if (PointerNewlyPressed(touch))
                    {
                        InputHandler.OnPointerPress(new Vector2(touch.Position.X, touch.Position.Y));
                    }
                    else if (PointerNewlyReleased(touch))
                    {
                        InputHandler.OnPointerRelease(new Vector2(touch.Position.X, touch.Position.Y));
                    }
                }

                //// Releasing of previous touch points
                foreach (var touch in _oldTouchState.Where(oldT => newTouchState.Where(newT => newT.Id == oldT.Id).Count() == 0))
                {
                    InputHandler.OnPointerRelease(new Vector2(touch.Position.X, touch.Position.Y));
                }
            }
        }

        private void CalculateKeyDownMessages(KeyboardState newKeyState)
        {
            var keys = newKeyState.GetPressedKeys().Where(key => !_oldKeyState.IsKeyDown(key));
            foreach (var key in keys)
            {
                InputHandler.OnKeyDown(key);
            }
        }

        private void CalculateKeyUpMessages(KeyboardState newKeyState)
        {
            var keys = _oldKeyState.GetPressedKeys().Where(key => !newKeyState.IsKeyDown(key));
            foreach (var key in keys)
            {
                InputHandler.OnKeyUp(key);
            }
        }

        private bool MouseStateHasChanged(MouseState newMouseState)
        {
            return newMouseState.LeftButton != _oldMouseState.LeftButton ||
                newMouseState.X != _oldMouseState.X ||
                newMouseState.Y != _oldMouseState.Y;
        }

        private bool TouchStateHasChanged(TouchCollection newTouchState)
        {
            if (newTouchState.Count != _oldTouchState.Count)
            {
                return true;
            }

            foreach (var touch in newTouchState)
            {
                var oldTouch = _oldTouchState.Where(oldT => oldT.Id == touch.Id).First();
                if (oldTouch == null)
                {
                    return true;
                }

                if (oldTouch.Position.X != touch.Position.X || oldTouch.Position.Y != touch.Position.Y ||
                    oldTouch.State != touch.State)
                {
                    return true;
                }
            }

            return false;
        }

        private bool PointerNewlyPressed(TouchLocation newTouch)
        {
            var oldTouch = _oldTouchState.Where(oldT => oldT.Id == newTouch.Id);
            return oldTouch.Count() == 0 || (newTouch.State == TouchLocationState.Pressed && oldTouch.First().State != TouchLocationState.Pressed);
        }

        private bool PointerNewlyReleased(TouchLocation newTouch)
        {
            var oldTouch = _oldTouchState.Where(oldT => oldT.Id == newTouch.Id);
            return oldTouch.Count() > 0 && newTouch.State == TouchLocationState.Released && oldTouch.First().State == TouchLocationState.Pressed;
        }

        private bool LeftMouseButtonNewlyPressed(MouseState newMouseState)
        {
            return newMouseState.LeftButton == ButtonState.Pressed && _oldMouseState.LeftButton == ButtonState.Released;
        }

        private bool LeftMouseButtonNewlyReleased(MouseState newMouseState)
        {
            return newMouseState.LeftButton == ButtonState.Released && _oldMouseState.LeftButton == ButtonState.Pressed;
        }
    }
}
