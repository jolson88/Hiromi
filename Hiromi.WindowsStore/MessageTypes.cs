using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Hiromi.Components;

namespace Hiromi
{
    public class DisableAdsMessage : Message
    {
    }

    public class EnableAdsMessage : Message
    {
    }

    public class ScreenSizeChangedMessage : Message
    {
    }

    // ************************************
    // **
    // **        STATE MESSAGES
    // **
    // ************************************
    public class RequestChangeStateMessage : Message
    {
        public GameState State { get; set; }

        public RequestChangeStateMessage(GameState state)
        {
            this.State = state;
        }
    }

    public class StateChangedMessage : Message
    {
        public GameState State { get; set; }

        public StateChangedMessage(GameState state)
        {
            this.State = state;
        }
    }

    public class GameStartedMessage : Message
    {
        public override string ToString()
        {
            return "[System] Starting game";
        }
    }


    // ************************************
    // **
    // **   OBJECT + COMPONENT MESSAGES
    // **
    // ************************************
    public class AddGameObjectRequestMessage : Message
    {
        public GameObject GameObject { get; set; }

        public AddGameObjectRequestMessage(GameObject obj)
        {
            this.GameObject = obj;
        }
    }

    public class GameObjectLoadedMessage : Message
    {
        public GameObject GameObject { get; set; }

        public GameObjectLoadedMessage(GameObject obj)
        {
            this.GameObject = obj;
        }
    }

    public class GameObjectRemovedMessage : Message
    {
        public int GameObjectId { get; set; }

        public GameObjectRemovedMessage(int gameObjectId)
        {
            this.GameObjectId = gameObjectId;
        }
    }

    public class GameObjectMovedMessage : Message
    {
        public GameObject GameObject { get; set; }

        public GameObjectMovedMessage(GameObject obj)
        {
            this.GameObject = obj;
        }
    }


    // ************************************
    // **
    // **         CAMERA MESSAGES
    // **
    // ************************************
    public class ZoomCameraMessage : Message
    {
        public float ZoomFactor { get; private set; }

        public ZoomCameraMessage(float zoomFactor)
        {
            this.ZoomFactor = zoomFactor;
        }
    }

    public class RotateCameraMessage : Message
    {
        public float RotationInRadians { get; private set; }

        public RotateCameraMessage(float rotationInRadians)
        {
            this.RotationInRadians = rotationInRadians;
        }
    }

    /// <summary>
    /// Offset the point that the Camera is looking at (while preserving the point the Camera is looking at)
    /// </summary>
    public class NudgeCameraMessage : Message
    {
        public Vector2 Translation { get; private set; }

        public NudgeCameraMessage(Vector2 translation)
        {
            this.Translation = translation;
        }
    }


    // ************************************
    // **
    // **         AUDIO MESSAGES
    // **
    // ************************************
    public class PlaySoundEffectMessage : Message
    {
        public SoundEffect SoundEffect { get; private set; }
        public float Volume { get; private set; }

        public PlaySoundEffectMessage(SoundEffect soundEffect) : this(soundEffect, 0.35f) { }
        public PlaySoundEffectMessage(SoundEffect soundEffect, float volume)
        {
            this.SoundEffect = soundEffect;
            this.Volume = volume;
        }

        public override MessageVerbosity GetMessageVerbosity()
        {
            return MessageVerbosity.Noise;
        }
    }

    public class PlaySongMessage : Message
    {
        public Song Song { get; private set; }
        public float Volume { get; private set; }
        public bool IsRepeating { get; private set; }

        public PlaySongMessage(Song song) : this(song, 0.3f, false) { }
        public PlaySongMessage(Song song, float volume, bool isRepeating)
        {
            this.Song = song;
            this.Volume = volume;
            this.IsRepeating = isRepeating;
        }

        public override MessageVerbosity GetMessageVerbosity()
        {
            return MessageVerbosity.Noise;
        }
    }


    // ************************************
    // **
    // **        INPUT MESSAGES
    // **
    // ************************************
    public class KeyDownMessage : Message
    {
        public Keys Key { get; set; }
        public KeyDownMessage(Keys key) { this.Key = key; }

        public override MessageVerbosity GetMessageVerbosity()
        {
            return MessageVerbosity.Noise;
        }
    }

    public class KeyUpMessage : Message
    {
        public Keys Key { get; set; }
        public KeyUpMessage(Keys key) { this.Key = key; }

        public override MessageVerbosity GetMessageVerbosity()
        {
            return MessageVerbosity.Noise;
        }
    }

    public class PointerEnterMessage : Message
    {
        public int GameObjectId { get; set; }
        public PointerEnterMessage(int gameObjectId) { this.GameObjectId = gameObjectId; }

        public override MessageVerbosity GetMessageVerbosity()
        {
            return MessageVerbosity.Noise;
        }
    }

    public class PointerExitMessage : Message
    {
        public int GameObjectId { get; set; }
        public PointerExitMessage(int gameObjectId) { this.GameObjectId = gameObjectId; }

        public override MessageVerbosity GetMessageVerbosity()
        {
            return MessageVerbosity.Noise;
        }
    }

    public class PointerPressMessage : Message
    {
        public int GameObjectId { get; set; }
        public PointerPressMessage(int gameObjectId) { this.GameObjectId = gameObjectId; }

        public override MessageVerbosity GetMessageVerbosity()
        {
            return MessageVerbosity.Noise;
        }
    }

    public class PointerReleaseMessage : Message
    {
        public int GameObjectId { get; set; }
        public PointerReleaseMessage(int gameObjectId) { this.GameObjectId = gameObjectId; }

        public override MessageVerbosity GetMessageVerbosity()
        {
            return MessageVerbosity.Noise;
        }
    }

    public class ButtonPressMessage : Message
    {
        public int GameObjectId { get; set; }
        public ButtonPressMessage(int gameObjectId) { this.GameObjectId = gameObjectId; }

        public override MessageVerbosity GetMessageVerbosity()
        {
            return MessageVerbosity.Noise;
        }
    }
}
