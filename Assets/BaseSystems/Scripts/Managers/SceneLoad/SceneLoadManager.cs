using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class SceneLoadManager : MonoBehaviour, IManager, ISceneLoadCallMethods
    {
        [SerializeField] private bool _dataTransfereable = true;

        public void Initialize()
        {
            Debug.Log("<color=orange>MANAGER: </color>sceneLoadManager, <color=green>INITIALIZED!</color>");
        }

        public void TransferData<T>(T data)
        {
            if (!_dataTransfereable) return;
            if (data.GetType() != typeof(SceneLoadManager)) return;

            Debug.Log("<color=orange>MANAGER: </color>sceneLoadManager, <color=green>TRANSFERRING DATA!</color>");
            SetData(data as SceneLoadManager);
        }

        private void SetData(SceneLoadManager manager)
        {

        }

        public void LoadScene(string scene)
        {
            if (SceneManager.GetSceneByName(scene) == null) return;
            SceneManager.LoadScene(scene);
        }

        public void LoadNextScene()
        {
            return;
        }
    }
}