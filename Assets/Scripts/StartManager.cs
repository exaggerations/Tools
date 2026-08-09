using UnityEngine;
using XFramework.UIFramework;

public class StartManager : MonoBehaviour
{
    private PanelManager panelManager;

    private void Awake()
    {
        panelManager = new PanelManager();
    }

    void Start()
    {
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            GameObject canvasObj = new GameObject("Canvas");
            canvas = canvasObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            //canvasObj.AddComponent<CanvasScaler>();
            //canvasObj.AddComponent<GraphicRaycaster>();
            Debug.LogWarning("Canvas not found in scene. Created new Canvas with required components.");
        }

        panelManager.Push(new StartPanel());
    }
}
