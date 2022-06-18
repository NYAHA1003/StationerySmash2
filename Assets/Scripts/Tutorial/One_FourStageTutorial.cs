using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class One_FourStageTutorial : AbstractStageTutorial
{
    public override void SetQueue()
    {
        BattleTurtorialComponent.tutorialEventQueue.Enqueue(ExplainWhatIsThrow);
        BattleTurtorialComponent.tutorialEventQueue.Enqueue(ExpainThrowDetail);
        BattleTurtorialComponent.tutorialEventQueue.Enqueue(Advice);
    }

    public override void EndTutorial()
    {
        base.EndTutorial();
    }

    /// <summary>
    /// 던지기가 무엇인지 설명 
    /// </summary>
    private void ExplainWhatIsThrow()
    {
        SetSpeechText();
        SetImpactPos(impactTrans[0].anchoredPosition);
    }
    /// <summary>
    /// 던지기 자세히 설명 
    /// </summary>
    private void ExpainThrowDetail()
    {
        SetSpeechText();
    }
    /// <summary>
    /// 조언 
    /// </summary>
    private void Advice()
    {
        SetSpeechText();
    }
}
