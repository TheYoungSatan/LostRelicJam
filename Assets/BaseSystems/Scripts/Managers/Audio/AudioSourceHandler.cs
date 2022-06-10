using System.Collections;
using UnityEngine;

namespace Handlers
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioSourceHandler : MonoBehaviour
    {
        public bool Enabled 
        { 
            get { return enabled; } 
            set {
                gameObject.SetActive(value);
                enabled = value;
            } 
        }
        private new bool enabled = false;

        public bool ImportantSound;

        public AudioClip Clip => _source.clip;

        private AudioSource _source = null;
        private Coroutine _currentRoutine = null;

        private string _currentKey;
        private bool _paused;

        private void Awake()
        {
            if (TryGetComponent(out AudioSource source)) _source = source;
            else _source = gameObject.AddComponent<AudioSource>();
        }

        public void SetParent(Transform trans = null) => transform.parent = trans;

        public void SetSource(string key, AudioClip clip, bool loop = false, Vector3 position = default, float spatialBlend = 0)
        {
            _currentKey = key;
            _source.clip = clip;
            _source.loop = loop;
            _source.spatialBlend = spatialBlend;
            transform.position = position;
        }

        public void PlayClip() 
        {
            if (!_source.clip)
            {
                Debug.Log("<color=red>CANNOT PLAY CLIP: </color>clip is empty.<color=green> SKIPPING!</color>");
                return;
            }

            if (_source.isPlaying)
            {
                StopCoroutine(_currentRoutine);
                _currentRoutine = null;
                _source.Stop();
            }

            Debug.Log($"<color=blue>SOURCE PLAYING: </color>{_currentKey}, <color=green>PLAYING CLIP!</color>");
            _source.Play();
            _currentRoutine = StartCoroutine(AutoDisable());
        }

        public void PauseClip()
        {
            if (!_source.isPlaying || !_source.clip) return;

            if(!ImportantSound)
                _source.Pause();
            _paused = true;
        }

        public void UnPauseClip()
        {
            if (!_source.clip) return;

            if (!ImportantSound)
                _source.UnPause();

            _paused = false;
        }

        public IEnumerator AutoDisable()
        {
            yield return null;
            yield return new WaitUntil(() => !_source.isPlaying && (!_paused || ImportantSound));

            Enabled = false;
            _source.clip = null;
            Debug.Log($"<color=blue>SOURCE PLAYING: </color>{_currentKey}, <color=green>DISABLED SUCCESFULLY!</color>");
        }
    }
}