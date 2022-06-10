using System;
using System.Collections.Generic;
using Managers;
using UnityEngine;

public class BaseMainManager : MonoBehaviour, IAudioCallMethods, IPauseCallMethods, IGameInfoCallMethods, ISaveCallMethods, ISceneLoadCallMethods
{
    public IManager[] Managers => _managers;

    public static BaseMainManager INSTANCE { get { return instance; } }
    private static BaseMainManager instance;

    private IManager[] _managers;
    private Dictionary<Type, IManager> managers = new Dictionary<Type, IManager>();

    private void Awake()
    {
        _managers = GetComponentsInChildren<IManager>();

        if (!instance) instance = this;
        TransferManagerData();

        if (instance != this)
        {
            Debug.Log($"<color=orange>MANAGER: </color> mainManager, <color=blue>DESTROYED</color>: not the instance in the scene");
            Destroy(this.gameObject);
            return;
        }

        Debug.Log($"<color=orange>MANAGER: </color> mainManager, <color=green>INITIALIZING</color> {_managers.Length} <color=green>MANAGERS!</color>");
        InitializeManagers();

        DontDestroyOnLoad(instance.gameObject);
    }

    private void TransferManagerData()
    {
        if (instance == this) return;
        Debug.Log($"<color=orange>MANAGER: </color> mainManager, <color=green>TRANSFERRING DATA TO</color> {_managers.Length} <color=green>MANAGERS!</color>");
        foreach (IManager m in instance.Managers)
        {
            foreach (var sm in _managers)
            {
                if (sm.GetType() != m.GetType()) continue;
                m.TransferData(sm);
            }
        }
    }

    private void InitializeManagers()
    {
        foreach (var m in _managers)
        {
            m.Initialize();
            managers.Add(m.GetType(), m);
        }
    }

    private IManager GetManager(Type type)
    {
        if (!managers.ContainsKey(type)) return null;
        var manager = managers[type];
        return manager;
    }

    #region AudioManager
    public void PlaySFX(string key, bool isImportant = false)
    {
        var manager = GetManager(typeof(AudioManager)) as AudioManager;
        if (!manager) return;

        manager.PlaySFX(key, isImportant);
    }

    public void EnqueAudioclip(AudioClip clip, string key, bool playClip = true)
    {
        var manager = GetManager(typeof(AudioManager)) as AudioManager;
        if (!manager) return;

        manager.EnqueAudioclip(clip, key, playClip);
    }
    #endregion

    #region SceneLoadManager
    public void LoadNextScene()
    {
        var manager = GetManager(typeof(SceneLoadManager)) as SceneLoadManager;
        if (!manager) return;

        manager.LoadNextScene();
    }

    public void LoadScene(string scene)
    {
        var manager = GetManager(typeof(SceneLoadManager)) as SceneLoadManager;
        if (!manager) return;

        manager.LoadScene(scene);
    }
    #endregion

    #region PauseManager
    public void Pause()
    {
        var manager = GetManager(typeof(PauseManager)) as PauseManager;
        if (!manager) return;

        manager.Pause();
    }

    public void UnPause()
    {
        var manager = GetManager(typeof(PauseManager)) as PauseManager;
        if (!manager) return;

        manager.UnPause();
    }
    #endregion

    #region SaveManager
    public void Save()
    {
        var manager = GetManager(typeof(SaveManager)) as SaveManager;
        if (!manager) return;

        manager.Save();
    }

    public void Load()
    {
        var manager = GetManager(typeof(SaveManager)) as SaveManager;
        if (!manager) return;

        manager.Load();
    }
    #endregion
}