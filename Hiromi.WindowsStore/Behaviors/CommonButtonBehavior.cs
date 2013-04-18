using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hiromi;
using Hiromi.Messaging;

namespace Hiromi.Behaviors
{
    public class CommonButtonBehavior : GameObjectBehavior
    {
        private Sprite _focusSprite;
        private Sprite _nonFocusSprite;

        public CommonButtonBehavior() : this(null, null) { }
        public CommonButtonBehavior(Sprite nonFocusSprite, Sprite focusSprite)
        {
            _nonFocusSprite = nonFocusSprite;
            _focusSprite = focusSprite;
        }

        protected override void OnInitialize()
        {
            MessageService.Instance.AddListener<PointerExitMessage>(msg => OnPointerExit((PointerExitMessage)msg));
            MessageService.Instance.AddListener<PointerPressMessage>(msg => OnPointerPress((PointerPressMessage)msg));
            MessageService.Instance.AddListener<PointerReleaseMessage>(msg => OnPointerRelease((PointerReleaseMessage)msg));
        }

        private void OnPointerExit(PointerExitMessage msg)
        {
            if (msg.GameObjectId == this.GameObject.Id)
            {
                if (_nonFocusSprite != null)
                {
                    this.GameObject.Sprite = _nonFocusSprite;
                }
            }
        }

        private void OnPointerPress(PointerPressMessage msg)
        {
            if (msg.GameObjectId == this.GameObject.Id)
            {
                MessageService.Instance.TriggerMessage(new ButtonPressMessage(this.GameObject.Id));
                if (_focusSprite != null)
                {
                    this.GameObject.Sprite = _focusSprite;
                }
            }
        }

        private void OnPointerRelease(PointerReleaseMessage msg)
        {
            if (msg.GameObjectId == this.GameObject.Id)
            {
                if (_nonFocusSprite != null)
                {
                    this.GameObject.Sprite = _nonFocusSprite;
                }
            }
        }
    }
}
