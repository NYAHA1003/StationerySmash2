using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class One_TwoStageTutorial : AbstractStageTutorial
{
    public override void SetQueue()
    {
        BattleTurtorialComponent.tutorialEventQueue.Enqueue(ExplainUseAbility);
        BattleTurtorialComponent.tutorialEventQueue.Enqueue(ExplainAbiility);
        BattleTurtorialComponent.tutorialEventQueue.Enqueue(ExplainAdvice);

    }
    public override void EndTutorial()
    {
    }

    /// <summary>
    /// 특수 능력 설명 
    /// </summary>
    private void ExplainUseAbility()
    {
        SetSpeechText();
        SetImpactPos(impactTrans[0].anchoredPosition);
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

