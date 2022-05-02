using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SceneLoadComponenet : MonoBehaviour
{ 
    /// <summary>
    /// 배틀씬을 로딩씬을 거쳐 이동한다
    /// </summary>
    public void SceneLoadBattle()
    {
        DOTween.KillAll();
        LoadingManager.LoadScene("BattleScene");
    }
    /// <summary>
    /// 메인씬을 로딩씬을 거쳐 이동한다
    /// </summary>
    public void SceneLoadMain()
    {
        DOTween.KillAll();
        Time.timeScale = 1f;
        LoadingManager.LoadScene("MainScene");
    }
}
