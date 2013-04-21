﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hiromi;
using Hiromi.Components;
using Hiromi.Messaging;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Hiromi.Systems
{
    public class GeneralInputSystem : GameSystem
    {
        private MouseState _oldMouseState;
        private KeyboardState _oldKeyState;
        private List<int> _previousGameObjectsUnderMouse;

        public GeneralInputSystem()
        {
            _previousGameObjectsUnderMouse = new List<int>();
        }

        protected override void OnUpdate(GameTime gameTime)
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
                this.MessageManager.QueueMessage(new KeyDownMessage(key));
            }
        }

        private void CalculateKeyUpMessages(KeyboardState newKeyState)
        {
            var keys = _oldKeyState.GetPressedKeys().Where(key => !newKeyState.IsKeyDown(key));
            foreach (var key in keys)
            {
                this.MessageManager.QueueMessage(new KeyUpMessage(key));
            }
        }

        private void CalculateMouseMessages(MouseState newMouseState)
        {
            var gameObjectsUnderMouse = new List<int>();

            if (MouseStateHasChanged(newMouseState))
            {
                foreach (var obj in this.GameObjects.Values)
                {
                    if (MouseOverGameObject(newMouseState, obj))
                    {
                        gameObjectsUnderMouse.Add(obj.Id);
                        if (!MousePreviouslyOverGameObject(obj))
                        {
                            this.MessageManager.QueueMessage(new PointerEnterMessage(obj.Id));
                        }
                        if (LeftMouseButtonNewlyPressed(newMouseState))
                        {
                            this.MessageManager.QueueMessage(new PointerPressMessage(obj.Id));
                        }
                        if (LeftMouseButtonNewlyReleased(newMouseState))
                        {
                            this.MessageManager.QueueMessage(new PointerReleaseMessage(obj.Id));
                        }
                    }
                    else if (MousePreviouslyOverGameObject(obj))
                    {
                        // No longer over this game object
                        this.MessageManager.QueueMessage(new PointerExitMessage(obj.Id));
                    }
                }

                _previousGameObjectsUnderMouse = gameObjectsUnderMouse;
            }
        }

        private bool MouseStateHasChanged(MouseState newMouseState)
        {
            return newMouseState.LeftButton != _oldMouseState.LeftButton ||
                newMouseState.X != _oldMouseState.X ||
                newMouseState.Y != _oldMouseState.Y;
        }

        private bool MouseOverGameObject(MouseState mouseState, GameObject obj)
        {
            // Need to convert pixel coordinates from mouse into screen coordinates
            var pos = obj.GetComponent<PositionComponent>();
            return pos.Bounds.Contains((float)mouseState.X / GraphicsService.Instance.GraphicsDevice.Viewport.Width,
                (float)mouseState.Y / GraphicsService.Instance.GraphicsDevice.Viewport.Height);
        }

        private bool MousePreviouslyOverGameObject(GameObject obj)
        {
            return _previousGameObjectsUnderMouse.Contains(obj.Id);
        }

        private bool LeftMouseButtonNewlyPressed(MouseState newMouseState)
        {
            return newMouseState.LeftButton == ButtonState.Pressed && _oldMouseState.LeftButton == ButtonState.Released;
        }

        private bool LeftMouseButtonNewlyReleased(MouseState newMouseState)
        {
            return newMouseState.LeftButton == ButtonState.Released && _oldMouseState.LeftButton == ButtonState.Pressed;
        }

        protected override bool IsGameObjectForSystem(GameObject obj)
        {
            return obj.HasComponent<PositionComponent>();
        }
    }
}
