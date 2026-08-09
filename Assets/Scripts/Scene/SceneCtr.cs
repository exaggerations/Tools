using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using XFramework.Extend;

namespace XFramework
{
    public class SceneCtr : MonoBehaviour
    {
        SceneState sceneState;

        bool isReady;

        string sceneName;

        private string LoadPanelName { get => GameRoot.Instance.loadingPanelName; }

        public SceneCtr()
        {
            isReady = false;
        }

        public void SetScene(SceneState state,bool reload = true)
        {
            isReady = false;
            state?.OnExit();
            sceneState = state;
            sceneName = state.SceneName;

            if(reload )
            {
               
            }
        }

        protected void LoadScene()
        {
            SceneManager.LoadScene( sceneName );
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
