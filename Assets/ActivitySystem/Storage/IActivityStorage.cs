namespace ActivityFramework
{
    /// <summary>
    /// 活动存档存储接口：可替换为本地/云端实现
    /// </summary>
    public interface IActivityStorage
    {
        void Save(ActivitySaveData data);
        ActivitySaveData Load();
        bool HasSave();
        void Clear();
    }
}
