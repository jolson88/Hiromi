using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hiromi;
using Hiromi.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace Hiromi
{
    public class PointerInputHandler
    {
        private MessageManager _messageManager;
        
        private SceneGraph _sceneGraph;
        private MouseState _oldMouseState;
        private TouchCollection _oldTouchState;
        private bool _touchSupported = false;
        private int? _previousGameObjectUnderPointer = null;

        public PointerInputHandler(MessageManager messageManager, SceneGraph sceneGraph)
        {
            _sceneGraph = sceneGraph;
            _messageManager = messageManager;

            _oldMouseState = Mouse.GetState();

            var touchCapabilities = TouchPanel.GetCapabilities();
            if (touchCapabilities.IsConnected)
            {
                _touchSupported = true;
                _oldTouchState = TouchPanel.GetState();
            }
        }

        public void Update(GameTime gameTime)
        {
            var newMouseState = Mouse.GetState();
            CalculateMouseMessages(newMouseState);
            _oldMouseState = newMouseState;

            if (_touchSupported)
            {
                var newTouchState = TouchPanel.GetState();
                CalculateTouchMessages(newTouchState);
                _oldTouchState = newTouchState;
            }
        }

        private void CalculateMouseMessages(MouseState newMouseState)
        {
            if (MouseStateHasChanged(newMouseState))
            {
                CalculatePointerMessages(new Vector2(newMouseState.X, newMouseState.Y),
                    LeftMouseButtonNewlyPressed(newMouseState),
                    LeftMouseButtonNewlyReleased(newMouseState));
            }
        }

        private void CalculateTouchMessages(TouchCollection newTouchState)
        {
            if (TouchStateHasChanged(newTouchState))
            {
                // Current touch points
                foreach (var touch in newTouchState)
                {
                    CalculatePointerMessages(new Vector2(touch.Position.X, touch.Position.Y),
                        PointerNewlyPressed(touch),
                        PointerNewlyReleased(touch));
                }

                //// Releasing of previous touch points
                foreach (var touch in _oldTouchState.Where(oldT => newTouchState.Where(newT => newT.Id == oldT.Id).Count() == 0))
                {
                    CalculatePointerMessages(new Vector2(touch.Position.X, touch.Position.Y),
                        false,
                        true);
                }
            }
        }

        private void CalculatePointerMessages(Vector2 pointerLocation, bool pointerNewlyPressed, bool pointerNewlyReleased)
        {
            var gameObjectsUnderMouse = new List<int>();

            int? pickedGameObjectId = null;
            var objectPicked = _sceneGraph.Pick(pointerLocation, ref pickedGameObjectId);
            if (objectPicked)
            {
                if (pickedGameObjectId != _previousGameObjectUnderPointer && _previousGameObjectUnderPointer.HasValue)
                {
                    _messageManager.QueueMessage(new PointerExitMessage(_previousGameObjectUnderPointer.Value));
                }

                if (_previousGameObjectUnderPointer == null || pickedGameObjectId.Value != _previousGameObjectUnderPointer.Value)
                {
                    _previousGameObjectUnderPointer = pickedGameObjectId;
                    _messageManager.QueueMessage(new PointerEnterMessage(pickedGameObjectId.Value));
                }

                if (pointerNewlyPressed)
                {
                    _messageManager.QueueMessage(new PointerPressMessage(pickedGameObjectId.Value));
                }
                if (pointerNewlyReleased)
                {
                    _messageManager.QueueMessage(new PointerReleaseMessage(pickedGameObjectId.Value));
                }
            }
            else if (_previousGameObjectUnderPointer >= 0)
            {
                _messageManager.QueueMessage(new PointerExitMessage(_previousGameObjectUnderPointer.Value));
                _previousGameObjectUnderPointer = null;
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
            return oldTouch.Count() > 0 &&  newTouch.State == TouchLocationState.Released && oldTouch.First().State == TouchLocationState.Pressed;
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
