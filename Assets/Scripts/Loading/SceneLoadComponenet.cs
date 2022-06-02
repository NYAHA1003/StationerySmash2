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
    [SerializeField, Header("�����̶�� �־������")]
    private UserDeckDataComponent _userDeckDataComponent;

    private void Awake()
    {
        EventManager.StartListening(EventsType.LoadMainScene, SceneLoadMain);
        EventManager.StartListening(EventsType.LoadBattleScene, SceneLoadBattle);
    }
    /// <summary>
    /// ��Ʋ���� �ε����� ���� �̵��Ѵ�
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
    /// ���ξ��� �ε����� ���� �̵��Ѵ�
    /// </summary>
    public void SceneLoadMain()
    {
        SceneLoadBase();
        LoadingManager.LoadScene("MainScene");
    }

    /// <summary>
    /// ��κ��� ���� �̵��� �� �������� �κ�
    /// </summary>
    public void SceneLoadBase()
    {
        //��� ��Ʈ�� ����
        DOTween.KillAll();
        //�ð��� 1�� �ǵ���
        Time.timeScale = 1f;
        //���̺� �����Ϳ� �����ϴ� ������Ʈ�� ����
        UserSaveManagerSO.ClearObserver();
        // ����ص� �̺�Ʈ �ʱ�ȭ 
        EventManager.ClearEvents(); 
    }
}
