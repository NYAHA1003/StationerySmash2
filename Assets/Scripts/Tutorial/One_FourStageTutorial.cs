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
    /// �����Ⱑ �������� ���� 
    /// </summary>
    private void ExplainWhatIsThrow()
    {
        SetSpeechText();
        SetImpactPos(impactTrans[0].anchoredPosition);
    }
    /// <summary>
    /// ������ �ڼ��� ���� 
    /// </summary>
    private void ExpainThrowDetail()
    {
        SetSpeechText();
    }
    /// <summary>
    /// ���� 
    /// </summary>
    private void Advice()
    {
        SetSpeechText();
    }
}
