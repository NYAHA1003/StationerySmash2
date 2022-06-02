using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Main.Deck;
using Main.Event;
using Utill.Data;
using Utill.Tool;

public class SceneLoadComponenet : MonoBehaviour
{
    [SerializeField, Header("메인이라면 넣어줘야함")]
    private UserDeckDataComponent _userDeckDataComponent;

    private void Awake()
    {
        EventManager.StartListening(EventsType.LoadMainScene, SceneLoadMain);
        EventManager.StartListening(EventsType.LoadBattleScene, SceneLoadBattle);
    }
    /// <summary>
    /// 배틀씬을 로딩씬을 거쳐 이동한다
    /// </summary>
    public void SceneLoadBattle()
    {
        if(_userDeckDataComponent != null)
		{
            if (!_userDeckDataComponent.CheckCanPlayGame())
			{
                return;
			}
		}
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
        UserSaveManagerSO.ClearObserver();
        // 등록해둔 이벤트 초기화 
        EventManager.ClearEvents(); 
    }
}
