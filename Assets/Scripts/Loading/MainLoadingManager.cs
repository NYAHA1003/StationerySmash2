using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainLoadingManager : LoadingManager
{
    protected override void Start()
    {
        MainSceneLoad();
        base.Start();
    }
    public static void MainSceneLoad()
    {
        nextScene ="MainScene";
    }
}
