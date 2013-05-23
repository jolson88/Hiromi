using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace Hiromi
{
    public class SoundManager
    {
        private MessageManager _messageManager { get; set; }
        private Dictionary<string, SoundEffectInstance> _cachedEffects;

        public SoundManager(MessageManager messageManager)
        {
            _cachedEffects = new Dictionary<string, SoundEffectInstance>();
            _messageManager = messageManager;
            _messageManager.AddListener<PlaySoundEffectMessage>(OnPlaySoundEffect);
            _messageManager.AddListener<PlaySongMessage>(OnPlaySong);
        }

        private void OnPlaySoundEffect(PlaySoundEffectMessage msg)
        {
            var instance = GetInstance(msg.SoundEffect);
            instance.Volume = msg.Volume;
            instance.Play();
        }

        private void OnPlaySong(PlaySongMessage msg)
        {
            MediaPlayer.Volume = msg.Volume;
            MediaPlayer.IsRepeating = msg.IsRepeating;
            MediaPlayer.Play(msg.Song);
        }

        private SoundEffectInstance GetInstance(SoundEffect effect)
        {
#if WINDOWS_PHONE
            // Names of resources for some reason don't work on XNA 4.0 targetting Phone 7.1
            // For now (first release), just don't cache sounds.
            return effect.CreateInstance();
#else
            if (!_cachedEffects.ContainsKey(effect.Name))
            {
                _cachedEffects.Add(effect.Name, effect.CreateInstance());
            }

            return _cachedEffects[effect.Name];
#endif
        }
    }
}
