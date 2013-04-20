using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hiromi;
using Hiromi.Components;
using Hiromi.Messaging;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Hiromi.Systems
{
    public class UISystem : GameSystem
    {
        public UISystem()
        {
            MessageService.Instance.AddListener<PointerExitMessage>(msg => OnPointerExit((PointerExitMessage)msg));
            MessageService.Instance.AddListener<PointerPressMessage>(msg => OnPointerPress((PointerPressMessage)msg));
            MessageService.Instance.AddListener<PointerReleaseMessage>(msg => OnPointerRelease((PointerReleaseMessage)msg));
        }

        private void OnPointerExit(PointerExitMessage msg)
        {
            if (this.GameObjects.Keys.Contains(msg.GameObjectId))
            {
                var obj = this.GameObjects[msg.GameObjectId];
                var button = obj.GetComponent<ButtonComponent>();
                var sprite = obj.GetComponent<SpriteComponent>();

                if (button.NonFocusTexture != null)
                {
                    sprite.Texture = button.NonFocusTexture;
                }
            }
        }

        private void OnPointerPress(PointerPressMessage msg)
        {
            if (this.GameObjects.Keys.Contains(msg.GameObjectId))
            {
                var obj = this.GameObjects[msg.GameObjectId];
                var button = obj.GetComponent<ButtonComponent>();
                var sprite = obj.GetComponent<SpriteComponent>();

                MessageService.Instance.TriggerMessage(new ButtonPressMessage(obj.Id));
                if (button.FocusTexture != null)
                {
                    sprite.Texture = button.FocusTexture;
                }
            }
        }

        private void OnPointerRelease(PointerReleaseMessage msg)
        {
            if (this.GameObjects.Keys.Contains(msg.GameObjectId))
            {
                var obj = this.GameObjects[msg.GameObjectId];
                var button = obj.GetComponent<ButtonComponent>();
                var sprite = obj.GetComponent<SpriteComponent>();

                if (button.NonFocusTexture != null)
                {
                    sprite.Texture = button.NonFocusTexture;
                }
            }
        }

        protected override bool IsGameObjectForSystem(GameObject obj)
        {
            return obj.HasComponent<PositionComponent>()
                && obj.HasComponent<SpriteComponent>() 
                && obj.HasComponent<ButtonComponent>();
        }
    }
}
