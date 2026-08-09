using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XFramework.Extend;
using XFramework.UIFramework;

namespace XFramework
{
    public class GameRoot : MonoBehaviour
    {
        private static GameRoot _instance;
        public static GameRoot Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<GameRoot>();
                    if (_instance == null)
                    {
                        GameObject go = new GameObject("GameRoot");
                        _instance = go.AddComponent<GameRoot>();
                    }
                }
                return _instance;
            }
        }

        private PanelManager panelManager;

        [Header("加载场景时显示的进度条面板名称")]
        public string loadingPanelName = "AsyLoadingPanel";

        public PanelManager PanelManager { get => panelManager; }

        protected virtual void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
                return;
            }
           
        }

        // Start is called before the first frame update
     protected  virtual void Start()
        {
            GameObject asyncLoadPanel = GameObject.Find(loadingPanelName);
            asyncLoadPanel?.transform.PanelApprerance(false);
            LoadScene(new StartScene(),false);

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void LoadScene(SceneState sceneState,bool reload = true)
        {
            
        }
    }
}
