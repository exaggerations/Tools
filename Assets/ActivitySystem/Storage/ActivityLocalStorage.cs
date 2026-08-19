using UnityEngine;

namespace ActivityFramework
{
    /// <summary>
    /// 本地存储实现：JSON 序列化到 PlayerPrefs
    /// 适用于单机游戏或离线模式
    /// </summary>
    public class ActivityLocalStorage : IActivityStorage
    {
        const string SaveKey = "ActivitySystem_SaveData";

        public void Save(ActivitySaveData data)
        {
            if (data == null)
            {
                Debug.LogWarning("[ActivityLocalStorage] 存档数据为 null，跳过保存");
                return;
            }

            string json = JsonUtility.ToJson(data);
            PlayerPrefs.SetString(SaveKey, json);
            PlayerPrefs.Save();
        }

        public ActivitySaveData Load()
        {
            if (!HasSave())
                return new ActivitySaveData();

            string json = PlayerPrefs.GetString(SaveKey, "");
            if (string.IsNullOrEmpty(json))
                return new ActivitySaveData();

            try
            {
                return JsonUtility.FromJson<ActivitySaveData>(json);
            }
            catch (System.Exception e)
            {
                Debug.LogError($"[ActivityLocalStorage] 存档解析失败: {e.Message}");
                return new ActivitySaveData();
            }
        }

        public bool HasSave()
        {
            return PlayerPrefs.HasKey(SaveKey);
        }

        public void Clear()
        {
            PlayerPrefs.DeleteKey(SaveKey);
            PlayerPrefs.Save();
        }
    }
}
