using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hiromi;
using Hiromi.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Hiromi
{
    // TODO: Add Touch support to PointerInputHandler
    public class PointerInputHandler
    {
        private MessageManager _messageManager;
        private SceneGraph _sceneGraph;
        private MouseState _oldMouseState;
        private int? _previousGameObjectUnderPointer = null;

        public PointerInputHandler(MessageManager messageManager, SceneGraph sceneGraph)
        {
            _sceneGraph = sceneGraph;
            _messageManager = messageManager;
        }

        public void Update(GameTime gameTime)
        {
            var newMouseState = Mouse.GetState();
            CalculateMouseMessages(newMouseState);
            _oldMouseState = newMouseState;
        }

        private void CalculateMouseMessages(MouseState newMouseState)
        {
            var gameObjectsUnderMouse = new List<int>();

            if (MouseStateHasChanged(newMouseState))
            {
                int? pickedGameObjectId = null;
                var objectPicked = _sceneGraph.Pick(new Vector2(newMouseState.X, newMouseState.Y), ref pickedGameObjectId);
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

                    if (LeftMouseButtonNewlyPressed(newMouseState))
                    {
                        _messageManager.QueueMessage(new PointerPressMessage(pickedGameObjectId.Value));
                    }
                    if (LeftMouseButtonNewlyReleased(newMouseState))
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
        }

        private bool MouseStateHasChanged(MouseState newMouseState)
        {
            return newMouseState.LeftButton != _oldMouseState.LeftButton ||
                newMouseState.X != _oldMouseState.X ||
                newMouseState.Y != _oldMouseState.Y;
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
