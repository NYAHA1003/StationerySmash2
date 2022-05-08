using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Utill.Load;

public abstract class AbstractStageTutorial
{
    private bool isPause = false;

    /// <summary>
    /// ť ���� 
    /// </summary>
    public abstract void SetQueue();
    /// <summary>
    /// ť �ʱ�ȭ 
    /// </summary>
    public void ResetQueue()
    {
        BattleTurtorialComponent.tutorialEventQueue.Clear();
    }

    /// <summary>
    /// Ÿ�ӽ����� ���� ( timeScale�� 0 �̸� 1 / 1 �̸� 0 ���� ) 
    /// </summary>
    public void SetTimeScale()
    {
        if (isPause == true)
        {
            Time.timeScale = 1;
            isPause = false;
            return;
        }
        Time.timeScale = 0;
        isPause = true;
    }

    /// <summary>
    /// ���� ���� 
    /// </summary>
    public abstract TextType NextExplain();
    // Debug.Log("���� ����");
    // ��ǳ�� �ؽ�Ʈ �ٸ� �ɷ� �ٲ� 
}

public class TextType
{
    public TextType(BattleStageType battleStageType, int orderIndex)
    {
        _battleStageType = battleStageType;
        _orderIndex = orderIndex
    }

    public BattleStageType _battleStageType;
    public int _orderIndex;
}

