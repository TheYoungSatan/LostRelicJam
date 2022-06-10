using Handlers;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    [Serializable]
    public class AudioManager : MonoBehaviour, IManager, IAudioCallMethods
    {
        [SerializeField] private bool _dataTransfereable = true;

        [Serializable]
        private struct AudioClips { public string Key; public AudioClip Clip; }
        [SerializeField] private AudioClips[] _audioClips = new AudioClips[0];
        [SerializeField] private AudioSourceHandler _exampleHandler = null;
        [SerializeField] private int _poolNumber = 5;

        private List<AudioSourceHandler> _pool = new List<AudioSourceHandler>();
        private Dictionary<string, AudioClip> _clips = new Dictionary<string, AudioClip>(); // <key, clip>
        
        public void Initialize()
        {
            if (!_exampleHandler) _exampleHandler = gameObject.GetComponentInChildren<AudioSourceHandler>();
            if (!_exampleHandler) { Debug.Log("<color=orange>MANAGER: </color>audioManager, <color=red>NO AUDIOSOURCE EXAMPLE ASSIGNED/FOUND!</color>"); return; }

            CreatePool();
            CreateClipList();

            Debug.Log("<color=orange>MANAGER: </color>audioManager, <color=green>INITIALIZED!</color>");
        }

        public void TransferData<T>(T data)
        {
            if (!_dataTransfereable) return;
            if (data.GetType() != typeof(AudioManager)) return;

            Debug.Log("<color=orange>MANAGER: </color>audioManager, <color=green>TRANSFERRING DATA!</color>");
            SetData(data as AudioManager);
        }

        private void SetData(AudioManager manager)
        {
            int length = manager._audioClips.Length + _audioClips.Length;
            AudioClips[] clips = new AudioClips[length];

            for (int i = 0; i < length; i++)
            {
                if (i < _audioClips.Length)
                    clips[i] = _audioClips[i];
                else
                    clips[i] = manager._audioClips[i - manager._audioClips.Length];
            }

            List<AudioClips> uniqueClips = new List<AudioClips>();
            foreach (var clip in clips)
            {
                if (CheckData(clip, uniqueClips)) continue;
                uniqueClips.Add(clip);
            }

            _audioClips = uniqueClips.ToArray();
            CreateClipList();

            bool CheckData(AudioClips clip, List<AudioClips> uniqueClips)
            {
                foreach (var uc in uniqueClips)
                {
                    if (uc.Key == clip.Key) return true;
                }
                return false;
            }
        }        

        private void CreateClipList()
        {
            if (_audioClips == null || _audioClips.Length <= 0) return;
            foreach (var clip in _audioClips)
            {
                if (_clips.ContainsKey(clip.Key)) 
                {
                    _clips[clip.Key] = clip.Clip; 
                    continue;
                }

                _clips.Add(clip.Key, clip.Clip);
            }
        }

        private void CreatePool()
        {
            for (int i = 0; i < _poolNumber; i++)
            {
                CreatePoolObject(_exampleHandler.gameObject, "Audio_PoolObject_", _pool);
                _pool[i].SetParent(transform);
                _pool[i].Enabled = false;
            }
            Debug.Log($"<color=orange>MANAGER: </color> audioManager, <color=green>CREATED POOLOBJECT!</color>");
            _exampleHandler.Enabled = false;
        }

        private T CreatePoolObject<T>(GameObject source, string name = "poolObject", List<T> pool = null)
        {
            int i = pool.Count + 1;
            GameObject go = Instantiate(source, Vector3.zero, Quaternion.identity);
            go.name = name + i.ToString("00");
            T val = go.GetComponent<T>();
            pool.Add(val);

            return val;
        }

        public void PlaySFX(string key, bool isImportant = false)
        {
            if(!_clips.ContainsKey(key))
            {
                Debug.Log("<color=red>CANNOT PLAY SFX: </color>key does not exist!<color=green> SKIPPING!</color>");
                return;
            }

            AudioSourceHandler handler = FindInactivePoolSource(_clips[key]);

            handler.ImportantSound = isImportant;
            handler.SetSource(key, _clips[key]);
            handler.PlayClip();
        }

        public void EnqueAudioclip(AudioClip clip, string key, bool playClip = true)
        {
            if (!_clips.ContainsKey(key))
                _clips.Add(key, clip);
            else
                _clips[key] = clip;

            if(playClip)
                PlaySFX(key);
        }

        public void PauseSound()
        {
            Debug.Log("<color=orange>MANAGER: </color> audioManager, <color=green>PAUSED!</color>");
            foreach (var source in _pool)
                source.PauseClip();
        }

        public void UnPauseSound()
        {
            Debug.Log("<color=orange>MANAGER: </color> audioManager, <color=green>UNPAUSED!</color>");
            foreach (var source in _pool)
                source.UnPauseClip();
        }

        private AudioSourceHandler FindInactivePoolSource(AudioClip clip)
        {
            AudioSourceHandler rSource = null;
            foreach (var source in _pool)
            {
                if (!source.Enabled || source.Clip == clip) { rSource = source; break; }
            }
            if (!rSource) rSource = CreatePoolObject(_exampleHandler.gameObject, "Audio_PoolObject_", _pool);

            rSource.Enabled = true;
            return rSource;
        }
    }
}