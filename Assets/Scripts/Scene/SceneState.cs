using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XFramework.UIFramework;

namespace XFramework
{
    public abstract class SceneState 
    {
        protected PanelManager panelManager;

        protected string sceneName="";

        protected GameRoot Game { get=>GameRoot.Instance; }

        public string SceneName { get => sceneName; }

        public SceneState()
        {
            panelManager = new PanelManager();
        }

        public virtual void OnEnter()
        {
            Debug.Log($"进入场景:{sceneName}");
        }

        public virtual void OnUpdate()
        {
            //Debug.Log($"更新场景:{sceneName}");
        }
        public virtual void OnExit()
        {
            Debug.Log($"退出场景:{sceneName}");
            panelManager.PopAll();
        }
    }
}
