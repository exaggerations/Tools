
using UnityEngine;


namespace XFramework.Extend
{
    public static class UnityExtentionMethod 
    {
       
        public static GameObject FindChildGameObject(this GameObject parent, string childName)
        {
            if(parent == null)
            {
                Debug.LogError("Parent GameObject is null.");
                return null;
            }

            if(parent.name == childName)
            {
                return parent;
            }
            Transform transform = parent.transform;
            for(int i =0; i < transform.childCount; i++)
            {
                GameObject child = transform.GetChild(i).gameObject;
               if(child.name == childName)
                {
                    return child;
                }
                else
                {
                    GameObject found = FindChildGameObject(child, childName);
                    if(found != null)
                    {
                        return found;
                    }
                }
            }
            Debug.LogWarning($"Child GameObject with name '{childName}' not found under parent '{parent.name}'.");
            return null;
        }

        public static T GetOrAddComponent<T>(this Transform transform) where T : Component
        {
            T component = transform.GetComponent<T>();
            if(component == null)
            {
                component = transform.gameObject.AddComponent<T>();
            }
            return component;
        }

        public static T GetOrAddComponentInChildren<T>(this Transform t,string childName) where T : Component
        {
            GameObject childObj = t.gameObject.FindChildGameObject(childName);
            
            if(childObj == null)
            {
                Debug.LogWarning($"Child GameObject with name '{childName}' not found under parent '{t.gameObject.name}'.");
                return null;
            }

            return childObj.transform.GetOrAddComponent<T>();
        }


        public static void PanelApprerance(this Transform t,bool on_off,bool active = false)
        {
            CanvasGroup group = t.GetOrAddComponent<CanvasGroup>();
            int value = on_off ? 1 : 0;

            group.blocksRaycasts = on_off;

            group.interactable = on_off;

            group.alpha = value;

            t.gameObject.SetActive(on_off || active);
        }
    }

}