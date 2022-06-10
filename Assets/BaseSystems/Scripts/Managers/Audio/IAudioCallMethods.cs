using UnityEngine;

namespace Managers
{
    public interface IAudioCallMethods
    {
        void PlaySFX(string key, bool isImportant = false);
        void EnqueAudioclip(AudioClip clip, string key, bool playClip = true);
    }
}