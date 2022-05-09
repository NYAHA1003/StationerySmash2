using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill.Load;

// 1-0 스테이지 튜토리얼 
[System.Serializable]
public class One_ZeroStageTutorial : AbstractStageTutorial
{
    public override void SetQueue()
    {
        ResetQueue(); 
        BattleTurtorialComponent.tutorialEventQueue.Enqueue(ExplainSummon);
        BattleTurtorialComponent.tutorialEventQueue.Enqueue(ExplainMoney);
        BattleTurtorialComponent.tutorialEventQueue.Enqueue(ExplainUpgrade);
    }

    /// <summary>
    /// 소환 설명
    /// </summary>
    public void ExplainSummon()
    {
        SetTimeScale(); 
        // 설명 텍스트에 텍스트 넣어야해 
        // 강조하는 부분 변경 
        Debug.Log("소환설명");
    }
    /// <summary>
    /// 돈 설명
    /// </summary>
    public void ExplainMoney()
    {
        SetTimeScale(); 
        Debug.Log("돈 설명"); 
    }
    /// <summary>
    /// 업그레이드 설명 
    /// </summary>
    public void ExplainUpgrade()
    {
        SetTimeScale(); 
        Debug.Log("업그레이드 설명"); 
    }

    public override TextType NextExplain()
    {
        return new TextType(BattleStageType.S1_1, 0);
    }
}