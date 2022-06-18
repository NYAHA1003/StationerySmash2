using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class One_FiveStageTutorial : AbstractStageTutorial
{
    public override void SetQueue()
    {
        BattleTurtorialComponent.tutorialEventQueue.Enqueue(Explain);
        BattleTurtorialComponent.tutorialEventQueue.Enqueue(Advice);
    }

    public override void EndTutorial()
    {
        base.EndTutorial(); 
    }

    /// <summary>
    /// 지금까지 배운것을 테스트하는 곳입니다 
    /// </summary>
    private void Explain()
    {
        SetSpeechText();
    }
    /// <summary>
    /// 상대 필통을 격파해보세요
    /// </summary>
    private void Advice()
    {
        SetSpeechText();
        SetImpactPos(impactTrans[0].anchoredPosition);
    }
}
