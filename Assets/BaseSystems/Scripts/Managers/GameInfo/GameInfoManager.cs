using UnityEngine;

namespace Managers
{
    public class GameInfoManager : MonoBehaviour, IManager, IGameInfoCallMethods
    {
        [SerializeField] private bool _dataTransfereable = true;

        public void Initialize()
        {
            Debug.Log("<color=orange>MANAGER: </color>gameInfoManager, <color=green>INITIALIZED!</color>");
        }

        public void TransferData<T>(T data)
        {
            if (!_dataTransfereable) return;
            if (data.GetType() != typeof(GameInfoManager)) return;

            Debug.Log("<color=orange>MANAGER: </color>gameInfoManager, <color=green>TRANSFERRING DATA!</color>");
            SetData(data as GameInfoManager);
        }

        private void SetData(GameInfoManager manager)
        {

        }
    }
}