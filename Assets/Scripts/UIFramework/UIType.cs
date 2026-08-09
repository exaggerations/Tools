public class UIType
{
   public string Name { get;private set; }

    public string Path { get; private set; }

    private bool isInit;

    public bool IsInit { get => isInit; set => isInit = value; }

    public UIType(string path)
    {
        Path = path;
        Name = path.Substring(path.LastIndexOf('/') + 1);
    }
}
