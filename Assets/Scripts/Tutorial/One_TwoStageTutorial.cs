using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class One_TwoStageTutorial : AbstractStageTutorial
{
    public static bool isTutorial = false;
    public override void SetQueue()
    {
        BattleTurtorialComponent.tutorialEventQueue.Enqueue(ExplainUseAbility);
        BattleTurtorialComponent.tutorialEventQueue.Enqueue(ExplainAbiility);
        BattleTurtorialComponent.tutorialEventQueue.Enqueue(ExplainAdvice);

    }
    public override void EndTutorial()
    {
        isTutorial = true;
        base.EndTutorial();
    }

    /// <summary>
    /// 특수 능력 설명 
    /// </summary>
    private void ExplainUseAbility()
    {
        SetSpeechText();
        SetImpactPos(impactTrans[0].anchoredPosition);
        //blackImpact.localScale = new Vector2(0.6f, 1f);
    }
    /// <summary>
    /// 특수 능력 설명 
    /// </summary>
    private void ExplainAbiility()
    {
        SetSpeechText();
    }
    /// <summary>
    /// 특수 능력 설명 
    /// </summary>
    private void ExplainAdvice()
    {
        SetSpeechText();
    }
}

