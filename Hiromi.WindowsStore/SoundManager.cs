using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Audio;

namespace Hiromi
{
    public class SoundManager
    {
        private MessageManager MessageManager { get; set; }

        public SoundManager(MessageManager messageManager)
        {
            this.MessageManager = messageManager;

            this.MessageManager.AddListener<PlaySoundEffectMessage>(OnPlaySoundEffect);
            // TODO: Temporary, restore the volume levels
            SoundEffect.MasterVolume = 0.25f;
        }

        private void OnPlaySoundEffect(PlaySoundEffectMessage msg)
        {
            var played = msg.SoundEffect.Play();
            if (!played)
            {
                System.Diagnostics.Debug.WriteLine("Unable to play sound effect: " + msg.SoundEffect.Name);
            }
        }
    }
}
