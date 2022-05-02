using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SceneLoadComponenet : MonoBehaviour
{ 
    /// <summary>
    /// ��Ʋ���� �ε����� ���� �̵��Ѵ�
    /// </summary>
    public void SceneLoadBattle()
    {
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
        SaveManager._instance._saveData.ClearObserver();
    }
}
