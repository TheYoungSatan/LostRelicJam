using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    public class SaveManager : MonoBehaviour, IManager, ISaveCallMethods
    {
        [SerializeField] private bool _dataTransfereable = true;

        public void Initialize()
        {
            Debug.Log("<color=orange>MANAGER: </color>saveManager, <color=green>INITIALIZED!</color>");
        }

        public void TransferData<T>(T data)
        {
            if (!_dataTransfereable) return;
            if (data.GetType() != typeof(SaveManager)) return;

            Debug.Log("<color=orange>MANAGER: </color>saveManager, <color=green>TRANSFERRING DATA!</color>");
            SetData(data as SaveManager);
        }

        private void SetData(SaveManager manager)
        {

        }

        public void Save()
        {
            throw new System.NotImplementedException();
        }

        public void Load()
        {
            throw new System.NotImplementedException();
        }
    }
}