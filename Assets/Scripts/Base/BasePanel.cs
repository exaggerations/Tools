using System.Transactions;
using Unity.VisualScripting;
using UnityEngine;
using XFramework.UIFramework;

public abstract class BasePanel
{
    private Transform activePanel;
    private PanelManager panelManager;

    public  UIType UIType { get; private set; }

    public BasePanel(UIType uiType)
    {
        UIType = uiType;
    }

    public Transform ActivePanel { get => activePanel; 
        set => activePanel = value;
    }

    public void Init(PanelManager panelManager)
    {
        this.panelManager = panelManager;
    }

    public void Push(BasePanel nextPanel)
    {
        panelManager?.Push(nextPanel);
    }

    public void Pop()
    {
        panelManager?.Pop();
    }

    protected virtual void InitEvent() { }

    public virtual void OnStart()
    {

        //to change
        activePanel.gameObject.SetActive(true);
        activePanel.SetAsLastSibling();
        //activePanel.SetSiblingIndex(activePanel.parent.childCount - 1);
        if(!UIType.IsInit)
        {
            InitEvent();
            UIType.IsInit = true;
        }

    }

    public virtual void Onenable()
    {
        activePanel.GetOrAddComponent<CanvasGroup>().blocksRaycasts = true;
    }

    public virtual void OnDisable()
    {

        activePanel.GetOrAddComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public virtual void OnDestroy(bool isDestroy = false)
    {
        panelManager?.DestroyUI(UIType, isDestroy);
    }

    public virtual void OnChange(BasePanel basePanel)
    {
        
    }

}
