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
        SceneLoadBase();
        LoadingManager.LoadScene("BattleScene");
    }
    /// <summary>
    /// 메인씬을 로딩씬을 거쳐 이동한다
    /// </summary>
    public void SceneLoadMain()
    {
        SceneLoadBase();
        LoadingManager.LoadScene("MainScene");
    }

    /// <summary>
    /// 대부분의 씬을 이동할 때 공통적인 부분
    /// </summary>
    public void SceneLoadBase()
    {
        //모든 닷트윈 제거
        DOTween.KillAll();
        //시간을 1로 되돌림
        Time.timeScale = 1f;
        //세이브 데이터에 접근하는 오브젝트들 제거
        SaveManager._instance._saveData.ClearObserver();
    }
}
