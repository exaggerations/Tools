using System.Collections.Generic;
using UnityEngine;

public class UIManager
{
    private Dictionary<UIType, GameObject> dicUI;

    public UIManager()
    {
        dicUI = new Dictionary<UIType, GameObject>();
    }

    public GameObject GetSingleUI(UIType uIType)
    {
        if (dicUI.ContainsKey(uIType))
            return dicUI[uIType];

        GameObject parent = GameObject.Find("Canvas");
        if (!parent)
        {
            Canvas canvas = Object.FindObjectOfType<Canvas>();
            if (canvas != null)
                parent = canvas.gameObject;
        }

        if (!parent)
        {
            Debug.LogError("Canvas not found in the scene.");
            return null;
        }

        GameObject prefab = Resources.Load<GameObject>(uIType.Path);
        if (prefab == null)
        {
            Debug.LogError($"Prefab not found at path: {uIType.Path}");
            return null;
        }

        GameObject ui = GameObject.Instantiate(prefab, parent.transform);
        ui.name = uIType.Name;
        dicUI.Add(uIType, ui);
        return ui;
    }

    public void DestroyUI(UIType uIType)
    {
        if (dicUI.ContainsKey(uIType))
        {
            GameObject ui = dicUI[uIType];
            GameObject.Destroy(ui);
            dicUI.Remove(uIType);
        }
    }
}
