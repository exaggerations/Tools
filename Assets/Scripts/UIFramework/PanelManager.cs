using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XFramework.UIFramework
{
    public class PanelManager
    {

        private Dictionary<string, GameObject> dicUI;

        private Dictionary<string,BasePanel> dicPanel;

        private Stack<BasePanel> stackPanel;

        string canvasName;

        private GameObject canvas;

        //private UIManager uiManager;
        //private BasePanel panel;

        public PanelManager()
        {
            dicUI = new Dictionary<string, GameObject>();
            dicPanel = new Dictionary<string, BasePanel>();
            stackPanel = new Stack<BasePanel>();
            canvasName = "Canvas";
            canvas = GameObject.Find(canvasName);
            //uiManager = new UIManager();
        }

        public PanelManager(string canvasName)
        {
            dicUI = new Dictionary<string, GameObject>();
            dicPanel = new Dictionary<string, BasePanel>();
            stackPanel = new Stack<BasePanel>();
            this.canvasName = canvasName;
            canvas = GameObject.Find(canvasName);
        }

        private GameObject GetSingleUI(UIType ui)
        {
            if(dicUI.ContainsKey(ui.Path))
            {
                ui.IsInit = true;
                return dicUI[ui.Path];
            }

            if(canvas == null)
            {
                canvas = GameObject.Find(canvasName);
                if(canvas == null)
                {
                    Debug.LogError("Canvas not found");
                    return null;
                }
            }
#if UNITY_EDITOR
            GameObject panelObj = GameObject.Instantiate<GameObject>(UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>($"Assets/Resources/{ui.Path}.prefab"),canvas.transform);
#else

#endif 

            panelObj.name = ui.Name;
            dicUI.Add(ui.Path, panelObj);
            return panelObj;
        }

        public void DestroyUI(UIType ui,bool isDestroy = false)
        {
            if (dicUI.ContainsKey(ui.Path))
            {
                if(isDestroy)
                {
                    GameObject.Destroy(dicUI[ui.Path]);
                    dicUI.Remove(ui.Path);
                    dicPanel.Remove(ui.Path);
                }
                else
                {
                    dicUI[ui.Path].SetActive(false);
                }
            }
        }

        public void Push(BasePanel nextPanel)
        {
            if (stackPanel.Count > 0)
            {
                //panel = stackPanel.Peek();
                //panel.OnPause();
                stackPanel.Peek().OnDisable();
            }

            if(!dicPanel.ContainsKey(nextPanel.UIType.Path))
            {
                dicPanel.Add(nextPanel.UIType.Path, nextPanel);
                GameObject obj = GetSingleUI(nextPanel.UIType);
                nextPanel.ActivePanel = obj.transform;
                nextPanel.Init(this);
            }
            else
            {
                BasePanel panel = dicPanel[nextPanel.UIType.Path];
                panel.OnChange(nextPanel);
                nextPanel = panel;
            }


             nextPanel.OnStart();
            if(stackPanel.Count > 0)
            {
                if(stackPanel.Peek() != nextPanel)
                {
                    stackPanel.Push(nextPanel);
                }
            }
            else
            {
                stackPanel.Push(nextPanel);
            }
        }

        public void Pop()
        {
            if (stackPanel.Count > 0)
            {
                stackPanel.Pop().OnDestroy();
            }
            if(stackPanel.Count > 0)
            {
                stackPanel.Peek().OnStart();
            }
        }

        public void PopAll()
        {
            var values = new List<BasePanel>(dicPanel.Values);
            while (values.Count > 0)
            {
               values[0].OnDestroy(true);
                values.RemoveAt(0);
            }
        }
    }
}
