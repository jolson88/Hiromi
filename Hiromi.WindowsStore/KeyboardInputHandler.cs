using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hiromi;
using Hiromi.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Hiromi.Messaging;

namespace Hiromi
{
    public class KeyboardInputHandler
    {
        private MessageManager _messageManager;
        private KeyboardState _oldKeyState;

        public KeyboardInputHandler(MessageManager messageManager)
        {
            _messageManager = messageManager;
        }

        public void Update(GameTime gameTime)
        {
            var newKeyState = Keyboard.GetState();
            CalculateKeyDownMessages(newKeyState);
            CalculateKeyUpMessages(newKeyState);
            _oldKeyState = newKeyState;
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
    }
}
