using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SA
{
    public class SessionManager : MonoBehaviour
    {
        [Header("Simple Save.")]
        public SaveProfile saveProfile;
        [ReadOnlyInspector] public bool _hasExitFromGame;
        public int cur_gameScene_Index;

        public event Action OnSceneLoadedEvent;

        #region Callbacks.
        public static SessionManager singleton;
        private void Awake()
        {
            #region Init Singleton.
            if (singleton != null)
            {
                Destroy(gameObject);
            }
            else
            {
                singleton = this;
            }
            #endregion

            Init();
        }
        #endregion

        #region Load Main Menu.
        public void LoadScene_ReturnMainMenu()
        {
            UnLoadScene_CurGameLevel();

            LoadScene_MainMenu();
        }
        #endregion

        #region Load Game Level.
        public void LoadScene_PlayGame()
        {
            UnLoadScene_MainMenu();
            LoadScene_CurGameLevel();
        }

        public void LoadScene_RetryGame()
        {
            UnLoadScene_CurGameLevel();
            LeanTween.value(0, 1, 0.1f).setOnComplete(OnCompleteWait);

            void OnCompleteWait()
            {
                LoadScene_CurGameLevel();
            }
        }
        #endregion

        #region Load Scene.
        void LoadScene_MainMenu()
        {
            OnSceneLoadedEvent = OnMainMenuSceneLoaded;
            StartCoroutine(LoadSceneAsync_Additive((int)G_SceneTypeEnum.MainMenuScene));
        }

        void LoadScene_CurGameLevel()
        {
            OnSceneLoadedEvent = OnGameSceneLoaded;
            StartCoroutine(LoadSceneAsync_Additive(cur_gameScene_Index));
        }

        void LoadScene(G_SceneTypeEnum scene)
        {
            StartCoroutine(LoadSceneAsync((int)scene));
        }

        void LoadSceneAdditive(G_SceneTypeEnum scene)
        {
            StartCoroutine(LoadSceneAsync_Additive((int)scene));
        }

        IEnumerator LoadSceneAsync(int sceneIndex)
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneIndex);

            while (!asyncLoad.isDone)
            {
                yield return null;
            }

            OnSceneLoadedEvent.Invoke();
        }

        IEnumerator LoadSceneAsync_Additive(int sceneIndex)
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Additive);

            while (!asyncLoad.isDone)
            {
                yield return null;
            }

            OnSceneLoadedEvent.Invoke();
        }
        #endregion

        #region UnLoad Scene.
        void UnLoadScene_CurGameLevel()
        {
            SceneManager.UnloadSceneAsync(cur_gameScene_Index);
        }

        public void UnLoadScene_MainMenu()
        {
            SceneManager.UnloadSceneAsync((int)G_SceneTypeEnum.MainMenuScene);
        }
        #endregion
        
        #region Init.
        void Init()
        {
            LoadScene_FirstMainMenu();
        }

        void LoadScene_FirstMainMenu()
        {
            LoadScene_MainMenu();
        }
        #endregion
        
        #region Actions.
        void OnMainMenuSceneLoaded()
        {
            /// Set Active Scene as Main Menu.
            SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex((int)G_SceneTypeEnum.MainMenuScene));

            /// Change Audio To Main Menu.
            MusicManager.singleton.OnMainMenuSceneLoaded();

            /// Nullifly Event.
            OnSceneLoadedEvent = null;
        }

        void OnGameSceneLoaded()
        {
            /// Set Active Scene as Current Game Level.
            SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(cur_gameScene_Index));

            /// Change Audio To Game Level.
            MusicManager.singleton.OnGameSceneLoaded();

            /// Nullifly Event.
            OnSceneLoadedEvent = null;
        }
        #endregion
    }

    [System.Serializable]
    public class SaveProfile
    {
        [ReadOnlyInspector] public bool _isEdgeScrolling;
    }
}