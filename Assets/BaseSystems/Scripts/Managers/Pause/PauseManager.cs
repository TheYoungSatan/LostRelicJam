using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Managers
{
    public class PauseManager : MonoBehaviour, IManager, IPauseCallMethods
    {
        [SerializeField] private bool _dataTransfereable = true;

        public UnityEvent OnPause;
        public UnityEvent OnResume;

        public void Initialize()
        {
            Debug.Log("<color=orange>MANAGER: </color>pauseManager, <color=green>INITIALIZED!</color>");
        }

        public void Pause() { OnPause?.Invoke(); }
        public void UnPause() { OnResume?.Invoke(); }

        public void TransferData<T>(T data)
        {
            if (!_dataTransfereable) return;
            if (data.GetType() != typeof(PauseManager)) return;

            Debug.Log("<color=orange>MANAGER: </color>pauseManager, <color=green>TRANSFERRING DATA!</color>");
            SetData(data as PauseManager);
        }

        private void SetData(PauseManager manager)
        {
            return;
        }
    }
}