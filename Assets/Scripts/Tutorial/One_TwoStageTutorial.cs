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
    /// Ư�� �ɷ� ���� 
    /// </summary>
    private void ExplainUseAbility()
    {
        SetSpeechText();
        SetImpactPos(impactTrans[0].anchoredPosition);
    }
    /// <summary>
    /// Ư�� �ɷ� ���� 
    /// </summary>
    private void ExplainAbiility()
    {
        SetSpeechText();
    }
    /// <summary>
    /// Ư�� �ɷ� ���� 
    /// </summary>
    private void ExplainAdvice()
    {
        SetSpeechText();
    }
}

