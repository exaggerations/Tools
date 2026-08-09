using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XFramework;

public class StartScene : SceneState
{
  public StartScene()
    {
           sceneName = "StartScene";
    }

    public override void OnEnter()
    {
      panelManager.Push(new StartPanel());
    }
}
