using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hiromi;
using Hiromi.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Hiromi.Rendering;

namespace Hiromi
{
    public class GeneralInputSystem
    {
        private MessageManager _messageManager;
        private SceneGraph _sceneGraph;
        private Dictionary<int, GameObject> _gameObjects;
        private MouseState _oldMouseState;
        private KeyboardState _oldKeyState;
        private int? _previousGameObjectUnderPointer = null;

        public GeneralInputSystem(MessageManager messageManager, SceneGraph sceneGraph)
        {
            _sceneGraph = sceneGraph;
            _messageManager = messageManager;
            _gameObjects = new Dictionary<int, GameObject>();
            _messageManager.AddListener<NewGameObjectMessage>(OnNewGameObject);
        }

        public void Update(GameTime gameTime)
        {
            var newMouseState = Mouse.GetState();
            var newKeyState = Keyboard.GetState();
            CalculateMouseMessages(newMouseState);
            CalculateKeyDownMessages(newKeyState);
            CalculateKeyUpMessages(newKeyState);
            _oldKeyState = newKeyState;
            _oldMouseState = newMouseState;
        }

        private void CalculateKeyDownMessages(KeyboardState newKeyState)
        {
            var keys = newKeyState.GetPressedKeys().Where(key => !_oldKeyState.IsKeyDown(key));
            foreach (var key in keys)
            {
                _messageManager.QueueMessage(new KeyDownMessage(key));
            }
        }

        private void CalculateKeyUpMessages(KeyboardState newKeyState)
        {
            var keys = _oldKeyState.GetPressedKeys().Where(key => !newKeyState.IsKeyDown(key));
            foreach (var key in keys)
            {
                _messageManager.QueueMessage(new KeyUpMessage(key));
            }
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
                    if (pickedGameObjectId != _previousGameObjectUnderPointer)
                    {
                        _messageManager.QueueMessage(new PointerEnterMessage(pickedGameObjectId.Value));
                    }
                    else
                    {
                        _messageManager.QueueMessage(new PointerExitMessage(_previousGameObjectUnderPointer.Value));
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

        private bool IsGameObjectForSystem(GameObject obj)
        {
            return obj.HasComponent<PositionComponent>();
        }

        private void OnNewGameObject(NewGameObjectMessage msg)
        {
            if (msg.GameObject.HasComponent<PositionComponent>())
            {
                _gameObjects.Add(msg.GameObject.Id, msg.GameObject);
            }
        }
    }
}
