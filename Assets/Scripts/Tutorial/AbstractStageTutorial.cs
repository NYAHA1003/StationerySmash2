using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Utill.Load;

public abstract class AbstractStageTutorial
{
    private bool isPause = false;

    /// <summary>
    /// 큐 설정 
    /// </summary>
    public abstract void SetQueue();
    /// <summary>
    /// 큐 초기화 
    /// </summary>
    public void ResetQueue()
    {
        BattleTurtorialComponent.tutorialEventQueue.Clear();
    }

    /// <summary>
    /// 타임스케일 조정 ( timeScale이 0 이면 1 / 1 이면 0 으로 ) 
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
    /// 다음 설명 
    /// </summary>
    public abstract TextType NextExplain();
    // Debug.Log("다음 설명");
    // 말풍선 텍스트 다른 걸로 바뀜 
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

