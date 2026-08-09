using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using XFramework.Extend;

namespace XFramework
{
    public class SceneController : MonoBehaviour
    {
        SceneState sceneState;

        bool isReady;

        string sceneName;

        private string LoadPanelName { get => GameRoot.Instance.loadingPanelName; }

        public SceneController()
        {
            isReady = false;
        }

        public void SetScene(SceneState state,bool reload = true)
        {
            isReady = false;
            state?.OnExit();
            sceneState = state;
            sceneName = state.SceneName;

            if(reload)
            {
                LoadScene();
            }else
                state?.OnEnter();
        }

        protected void LoadScene()
        {
            SceneManager.LoadScene( sceneName );
            SceneManager.sceneLoaded += SceneLoaded;
        }

        protected void LoadSceneAsync(bool loadPanel)
        {
            GameRoot.Instance.StartCoroutine(AsyncLoad(loadPanel));
            SceneManager.sceneLoaded += SceneLoaded;
        }

        protected void SceneLoaded(Scene scene,LoadSceneMode mode)
        {
            sceneState?.OnEnter();
            isReady = true;
            SceneManager.sceneLoaded -= SceneLoaded;
            Debug.Log($"{sceneName} loaded");
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        protected virtual IEnumerator AsyncLoad(bool loadPanel)
        {
            AsyncOperation operation;

            operation = SceneManager.LoadSceneAsync(sceneName);

            if (loadPanel)
            {
                GameObject panel = GameObject.Find(LoadPanelName);
                if (panel == null)
                {
                    Debug.LogError($"{LoadPanelName}Ãæ°å²»´æÔÚ");
                    yield break;
                }
                panel.transform.PanelApprerance(true);
                operation.allowSceneActivation = false;
                Slider slider = panel.GetComponentInChildren<Slider>();
                slider.value = 0;
                float progressValue;

                while (!operation.isDone)
                {
                    if (operation.progress < 0.9f)
                    {
                        progressValue = operation.progress;
                    }
                    else
                    {
                        progressValue = 1.0f;
                    }

                    slider.value = progressValue;
                    if (progressValue >= 0.9f)
                    {
                        slider.value = 1f;
                        operation.allowSceneActivation = true;
                    }

                    yield return null;
                }
                panel.transform.PanelApprerance(false);
            }
            else
                operation.allowSceneActivation = true;
        }
    }
}
